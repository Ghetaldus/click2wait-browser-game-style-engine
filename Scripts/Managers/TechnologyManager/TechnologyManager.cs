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
	public partial class TechnologyManager : EntityManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected TechnologyReward[] defaultTechnologies;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected TechnologyReward[] dummyTechnologies;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListTechnologySyncStruct syncTechnologies = new SyncListTechnologySyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, TechnologyTemplate _template, byte _state, double _timer, int _level)
		{
			TechnologySyncStruct syncStruct = new TechnologySyncStruct(_slot, _template, _state, _timer, _level);
			syncTechnologies.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<TechnologySyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<TechnologySyncStruct> entryList = new List<TechnologySyncStruct>();
			
			foreach (TechnologySyncStruct entry in syncTechnologies)
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
		
			syncTechnologies.Clear();
			
			int length = (defaultTechnologies == null || defaultTechnologies.Length == 0) ? 0 : defaultTechnologies.Length;
			
			if (defaultTechnologies != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultTechnologies.Length; i++)
					AddEntry(i, defaultTechnologies[i].template, defaultTechnologies[i].state, defaultTechnologies[i].timer, UnityEngine.Random.Range(defaultTechnologies[i].minLevel, defaultTechnologies[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncTechnologies.OrderBy( x => random.Next() );
			
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
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummyTechnologies == null || dummyTechnologies.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					TechnologyReward entry = dummyTechnologies[random.Next(0, dummyTechnologies.Length)];
					
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
			for (int i = 0; i < syncTechnologies.Count; i++)
				if (!syncTechnologies[i].Active)
					return syncTechnologies[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncTechnologies.Count; i++)
				if (syncTechnologies[i].slot == _slot)
					return i;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			foreach (TechnologySyncStruct entry in syncTechnologies)
			{
			
			}
		}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			foreach (TechnologySyncStruct entry in syncTechnologies)
			{
			
			}
		}
		
		// -------------------------------------------------------------------------------
			
	}

}