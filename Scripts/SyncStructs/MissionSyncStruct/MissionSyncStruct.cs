// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	[System.Serializable]
	public partial struct MissionSyncStruct : ISyncableStruct<MissionTemplate>
	{
	
    	public int		slot;
 		public int 		hash;
 		public byte 	state;
 		public double	timer;
 		
 		public int		level;
		public int 		id;
	
		// -------------------------------------------------------------------------------
		public MissionSyncStruct(int _slot, MissionTemplate template, byte _state, double _timer, int _level, int _id)
		{
			slot	= _slot;
			hash 	= (template == null) ? 0 : template.name.GetDeterministicHashCode();
			state	= _state;
			timer	= Math.Max(0,_timer);
			level 	= Mathf.Max(1,_level);
			id 		= Mathf.Max(0,_id);
		}
		
		// -------------------------------------------------------------------------------
    	public MissionTemplate template
		{
			get
			{
				if (hash == 0) return null;
				
				if (!MissionTemplate.dict.ContainsKey(hash))
					throw new KeyNotFoundException("[Missing] MissionTemplate not found in Resources: " + hash);
				return MissionTemplate.dict[hash];
			}
		}
		
    	// -------------------------------------------------------------------------------
    	public void Update(GameObject player)
    	{
    		
    		// check sell timer
    		// THEN
    		// Reset();
    		
    	}
    	
    	// -------------------------------------------------------------------------------
    	public bool Active
    	{
    		get { return hash != 0; }
    	}
    	
    	// -------------------------------------------------------------------------------
		public bool CanSell
		{
			get
			{
				foreach (LevelCurrencyCost cost in template.sellCost)
				{
					if (cost.Valid && Active && id > 0)
						return true;
				}
				return false;
			}
		}
    	
    	// -------------------------------------------------------------------------------
		public void Remove(long _amount=1)
		{
			// set sell timer
			// OR
			Reset();
		}
    	
    	// -------------------------------------------------------------------------------
		public void Reset()
		{
			level = 0;
			hash = 0;
			state = Constants.STATE_NONE;
			timer = 0;
		}
    	
    	// -------------------------------------------------------------------------------
    	public float TimeRemaining()
    	{
        	return NetworkTime.time >= timer ? 0 : (float)(timer - NetworkTime.time);
    	}
    	
	}
	
	public class SyncListMissionSyncStruct : SyncList<MissionSyncStruct> { }
	
}