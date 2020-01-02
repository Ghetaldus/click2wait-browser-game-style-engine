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
	public partial struct ItemSyncStruct : ISyncableStruct<ItemTemplate>
	{
	
    	public int		slot;
 		public int 		hash;
 		public byte 	state;
 		public double	timer;
 		
		public long		amount;
	
		// -------------------------------------------------------------------------------
		public ItemSyncStruct(int _slot, ItemTemplate template, byte _state, double _timer, long _amount)
		{
			slot	= _slot;
			hash 	= (template == null) ? 0 : template.name.GetDeterministicHashCode();
			state	= _state;
			timer	= Math.Max(0,_timer);
			amount 	= Math.Max(0,_amount);
		}
		
		// -------------------------------------------------------------------------------
		public ItemTemplate template
		{
			get
			{
				if (hash == 0) return null;
				
				if (!ItemTemplate.dict.ContainsKey(hash))
					throw new KeyNotFoundException("[Missing] ItemTemplate not found in Resources: " + hash);
				return ItemTemplate.dict[hash];
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
				foreach (FixedCurrencyCost cost in template.sellCost)
				{
					if (cost.Valid && Active && amount > 0)
						return true;
				}
				return false;
			}
		}
    	
    	// -------------------------------------------------------------------------------
		public void Remove(long _amount=1)
		{
			amount -= _amount;
			if (amount <= 0)
			{
				// set sell timer
				// OR
				Reset();
			}
		}
		
    	// -------------------------------------------------------------------------------
		public void Reset()
		{
			amount = 0;
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
	
	public class SyncListItemSyncStruct : SyncList<ItemSyncStruct> { }
	
}