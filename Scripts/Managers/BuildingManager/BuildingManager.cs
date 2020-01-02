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
	public partial class BuildingManager : EntityManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected BuildingReward[] defaultBuildings;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected BuildingReward[] dummyBuildings;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = true;
		
		protected SyncListBuildingSyncStruct syncBuildings = new SyncListBuildingSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, BuildingTemplate _template, byte _state, double _timer, int _level)
		{
			BuildingSyncStruct syncStruct = new BuildingSyncStruct(_slot, _template, _state, _timer, _level);
			syncBuildings.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<BuildingSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<BuildingSyncStruct> entryList = new List<BuildingSyncStruct>();
			
			foreach (BuildingSyncStruct entry in syncBuildings)
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
		
			syncBuildings.Clear();
			
			int length = (defaultBuildings == null || defaultBuildings.Length == 0) ? 0 : defaultBuildings.Length;
			
			if (defaultBuildings != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultBuildings.Length; i++)
					AddEntry(i, defaultBuildings[i].template, defaultBuildings[i].state, defaultBuildings[i].timer, UnityEngine.Random.Range(defaultBuildings[i].minLevel, defaultBuildings[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncBuildings.OrderBy( x => random.Next() );
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummyBuildings == null || dummyBuildings.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					BuildingReward entry = dummyBuildings[random.Next(0, dummyBuildings.Length)];
					
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
			for (int i = 0; i < syncBuildings.Count; i++)
				if (!syncBuildings[i].Active)
					return syncBuildings[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncBuildings.Count; i++)
				if (syncBuildings[i].slot == _slot)
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
			foreach (BuildingSyncStruct entry in syncBuildings)
			{
			
			}
		}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			foreach (BuildingSyncStruct entry in syncBuildings)
			{
			
			}
		}
		
		// -------------------------------------------------------------------------------
		
	}

}