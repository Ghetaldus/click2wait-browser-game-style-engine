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
	public partial class EventManager : SimpleManager
	{
	
		[Header("Event Data")]
		[SerializeField]
		protected EventReward[] defaultEvents;
		protected SyncListEventSyncStruct syncEvents = new SyncListEventSyncStruct();
		
		// -------------------------------------------------------------------------------
		[ServerCallback]
		protected override void Start()
		{
			CreateDefaultData();
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void CreateDefaultData()
		{
			syncEvents.Clear();
			
			if (defaultEvents == null || defaultEvents.Length == 0) return;
			
	   		foreach (EventReward defaults in defaultEvents)
	   			AddEntry(defaults.template);
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(EventTemplate _template)
		{
			EventSyncStruct syncStruct = new EventSyncStruct(_template);
			syncEvents.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<EventSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<EventSyncStruct> entryList = new List<EventSyncStruct>();
			
			foreach (EventSyncStruct entry in syncEvents)
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
		protected override void UpdateServer()
		{
			
		}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			
		}
				
	}

}