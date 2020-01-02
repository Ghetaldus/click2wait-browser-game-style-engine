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
	public partial class UnitManager : EntityManager
	{
		
		[Header("Default Data")]
		[SerializeField]
		protected UnitReward[] defaultUnits;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected UnitReward[] dummyUnits;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListUnitSyncStruct syncUnits = new SyncListUnitSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, UnitTemplate _template, byte _state, double _timer, int _level)
		{
			UnitSyncStruct syncStruct = new UnitSyncStruct(_slot, _template, _state, _timer, _level);
			syncUnits.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<UnitSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<UnitSyncStruct> entryList = new List<UnitSyncStruct>();
			
			foreach (UnitSyncStruct entry in syncUnits)
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
			syncUnits.Clear();
			
			int length = (defaultUnits == null || defaultUnits.Length == 0) ? 0 : defaultUnits.Length;
			
			if (defaultUnits != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultUnits.Length; i++)
					AddEntry(i, defaultUnits[i].template, defaultUnits[i].state, defaultUnits[i].timer, UnityEngine.Random.Range(defaultUnits[i].minLevel, defaultUnits[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncUnits.OrderBy( x => random.Next() );
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummyUnits == null || dummyUnits.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					UnitReward entry = dummyUnits[random.Next(0, dummyUnits.Length)];
					
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
			for (int i = 0; i < syncUnits.Count; i++)
				if (!syncUnits[i].Active)
					return syncUnits[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncUnits.Count; i++)
				if (syncUnits[i].slot == _slot)
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
			foreach (UnitSyncStruct entry in syncUnits)
			{
			
			}
		}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			foreach (UnitSyncStruct entry in syncUnits)
			{
			
			}
		}
		
		// -------------------------------------------------------------------------------
		
	}

}