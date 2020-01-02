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
	public partial class EnhancementManager : EntityManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected EnhancementReward[] defaultEnhancements;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected EnhancementReward[] dummyEnhancements;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListEnhancementSyncStruct syncEnhancements = new SyncListEnhancementSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, EnhancementTemplate _template, byte _state, double _timer, int _level)
		{
			EnhancementSyncStruct syncStruct = new EnhancementSyncStruct(_slot, _template, _state, _timer, _level);
			syncEnhancements.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<EnhancementSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<EnhancementSyncStruct> entryList = new List<EnhancementSyncStruct>();
			
			foreach (EnhancementSyncStruct entry in syncEnhancements)
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
		
			syncEnhancements.Clear();
			
			int length = (defaultEnhancements == null || defaultEnhancements.Length == 0) ? 0 : defaultEnhancements.Length;
			
			if (defaultEnhancements != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultEnhancements.Length; i++)
					AddEntry(i, defaultEnhancements[i].template, defaultEnhancements[i].state, defaultEnhancements[i].timer, UnityEngine.Random.Range(defaultEnhancements[i].minLevel, defaultEnhancements[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncEnhancements.OrderBy( x => random.Next() );
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummyEnhancements == null || dummyEnhancements.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					EnhancementReward entry = dummyEnhancements[random.Next(0, dummyEnhancements.Length)];
					
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
			for (int i = 0; i < syncEnhancements.Count; i++)
				if (!syncEnhancements[i].Active)
					return syncEnhancements[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncEnhancements.Count; i++)
				if (syncEnhancements[i].slot == _slot)
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
			foreach (EnhancementSyncStruct entry in syncEnhancements)
			{
			
			}
		}
		
		[Client]
		protected override void UpdateClient()
		{
			foreach (EnhancementSyncStruct entry in syncEnhancements)
			{
			
			}
		}
		
		
		
	}

}