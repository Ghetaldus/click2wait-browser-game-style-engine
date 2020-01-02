// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class BuffManager : EntityManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected BuffReward[] defaultBuffs;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected BuffReward[] dummyBuffs;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListBuffSyncStruct syncBuffs = new SyncListBuffSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, BuffTemplate _template, byte _state, double _timer, int _level)
		{
			BuffSyncStruct syncStruct = new BuffSyncStruct(_slot, _template, _state, _timer, _level);
			syncBuffs.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<BuffSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<BuffSyncStruct> entryList = new List<BuffSyncStruct>();
			
			foreach (BuffSyncStruct entry in syncBuffs)
			{
				if (entry.Active)
				{
					
					if (string.IsNullOrWhiteSpace(_category) ||
						entry.template.category == _category)
						entryList.Add(entry);
										
				}
				else if (!activeOnly && !entry.Active)
					entryList.Add(entry);
			
			}
			
			
			if (activeOnly && _sortOrder == SortOrder.Priority)
				entryList.OrderBy(x => x.template.priority).ToList();
			
			return entryList;
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void CreateDefaultData()
		{
		
			syncBuffs.Clear();
			
			int length = (defaultBuffs == null || defaultBuffs.Length == 0) ? 0 : defaultBuffs.Length;
			
			if (defaultBuffs != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultBuffs.Length; i++)
					AddEntry(i, defaultBuffs[i].template, defaultBuffs[i].state, defaultBuffs[i].timer, UnityEngine.Random.Range(defaultBuffs[i].minLevel, defaultBuffs[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncBuffs.OrderBy( x => random.Next() );
						
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummyBuffs == null || dummyBuffs.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
					BuffReward entry = dummyBuffs[random.Next(0, dummyBuffs.Length)];
					
					if (entry == null)
						AddEntry(i, null, 0, 0, 0);
					else
						AddEntry(i, entry.template, entry.state, entry.timer, UnityEngine.Random.Range(entry.minLevel, entry.maxLevel));
				}
			}
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetFreeSlot()
		{
			for (int i = 0; i < syncBuffs.Count; i++)
				if (!syncBuffs[i].Active)
					return syncBuffs[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncBuffs.Count; i++)
				if (syncBuffs[i].slot == _slot)
					return i;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpgradeLevel()
		{
			
			int startIndex = GetCapacity-1;
			
			base.UpgradeLevel();
		
			InsertDummyData(startIndex, GetCapacity-1);
		
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			foreach (BuffSyncStruct entry in syncBuffs)
			{
			
			}
		}
		
		[Client]
		protected override void UpdateClient()
		{
			foreach (BuffSyncStruct entry in syncBuffs)
			{
			
			}
		}
		
			
	}

}