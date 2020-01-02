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
	public partial class MissionManager : EntityManager
	{
		
		protected SyncListMissionSyncStruct syncMissions = new SyncListMissionSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, MissionTemplate _template, byte _state, double _timer, int _level, int _id)
		{
			MissionSyncStruct entry = new MissionSyncStruct(_slot, _template, _state, _timer, _level, _id);
			syncMissions.Add(entry);
		}
		
		// -------------------------------------------------------------------------------
		public List<MissionSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<MissionSyncStruct> entryList = new List<MissionSyncStruct>();
			
			foreach (MissionSyncStruct entry in syncMissions)
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
			InsertDummyData(0, GetCapacity-1);	
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			for (int i = startIndex; i <= endIndex; i++)
				AddEntry(i, null, 0, 0, 0, 0);
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetFreeSlot()
		{
			for (int i = 0; i < syncMissions.Count; i++)
				if (!syncMissions[i].Active)
					return syncMissions[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncMissions.Count; i++)
				if (syncMissions[i].slot == _slot)
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
			
		}
		
		[Client]
		protected override void UpdateClient()
		{
			
		}
	
	}

}