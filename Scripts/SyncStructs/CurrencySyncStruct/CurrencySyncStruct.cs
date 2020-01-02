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
	public partial struct CurrencySyncStruct : ISyncableStruct<CurrencyTemplate>
	{
	
    	public int		slot;
 		public int 		hash;
 		public byte 	state;
 		public long		timeStamp;
		public long		amount;
		
		// -------------------------------------------------------------------------------
		public CurrencySyncStruct(int _slot, CurrencyTemplate template, byte _state, long _timeStamp, long _amount)
		{
			slot		= _slot;
			hash 		= (template == null) ? 0 : template.name.GetDeterministicHashCode();
			state		= _state;
			timeStamp	= _timeStamp == 0 ? DateTime.UtcNow.Ticks : Math.Max(0,_timeStamp);
			amount 		= Math.Max(0,_amount);
		}
		
		// -------------------------------------------------------------------------------
		public CurrencyTemplate template
		{
			get
			{
				if (hash == 0) return null;
				
				if (!CurrencyTemplate.dict.ContainsKey(hash))
					throw new KeyNotFoundException("[Missing] CurrencyTemplate not found in Resources: " + hash);
				return CurrencyTemplate.dict[hash];
			}
		}   
    	
    	// -------------------------------------------------------------------------------
    	public void Update(GameObject player)
    	{
    		
    		int _duration 	= GetDuration(player);
    		
    		if (_duration <= 0) return;
    		
    		timeStamp 	= DateTime.UtcNow.Ticks;
    		
    		long _production = GetProduction(player, _duration);
    		
    		if (amount + _production > GetCapacity(player))
    			_production = GetCapacity(player) - amount;
			
			if (_production <= 0) return;
			
    		amount 		+= _production;
    		
    		player.GetComponent<StatisticManager>().TrackCurrency(template.name, Constants.STATE_PRODUCTION, _production);
    		
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
		}
    	
    	// -------------------------------------------------------------------------------
		public void Reset()
		{
			// ignore - we can never rest a currency
		}
    	    	
	}
	
	public class SyncListCurrencySyncStruct : SyncList<CurrencySyncStruct> { }
	
}