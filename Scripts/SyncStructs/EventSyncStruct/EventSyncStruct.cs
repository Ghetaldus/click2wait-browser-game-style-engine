// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	[System.Serializable]
	public partial struct EventSyncStruct : ISyncableStruct<EventTemplate>
	{
	
 		public int 		hash;
 		public bool		active;
 		
		// -------------------------------------------------------------------------------
		public EventSyncStruct(EventTemplate template)
		{
			hash 	= (template == null) ? 0 : template.name.GetDeterministicHashCode();
			active 	= false;
		}
		
		// -------------------------------------------------------------------------------
		public EventTemplate template
		{
			get
			{
				if (hash == 0) return null;
				
				if (!EquipmentTemplate.dict.ContainsKey(hash))
					throw new KeyNotFoundException("[Missing] EventTemplate not found in Resources: " + hash);
				return EventTemplate.dict[hash];
			}
		}
		
    	// -------------------------------------------------------------------------------
    	public void Update(GameObject player)
    	{
    		active = 
    				DateTime.Compare(DateTime.UtcNow, template.startDate) >= 0 &&
    				DateTime.Compare(DateTime.UtcNow, template.endDate) <= 0;
    	
    	}
    	
    	// -------------------------------------------------------------------------------
    	public int level
    	{
    		get { return template.level; }
    	}
    	
    	// -------------------------------------------------------------------------------
    	public bool Active
    	{
    		get { return hash != 0 && active; }
    	}
    	
    	// -------------------------------------------------------------------------------
		public bool CanSell
		{
			get { return false; }
		}
		
		// -------------------------------------------------------------------------------
		public void Remove(long _amount=1)
		{
			// do nothing - we can never sell a event
		}
    	
    	// -------------------------------------------------------------------------------
		public void Reset()
		{
			hash = 0;
			active = false;
		}
    	
	}
	
	public class SyncListEventSyncStruct : SyncList<EventSyncStruct> { }
	
}