// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	public partial struct CurrencySyncStruct
	{
	
    	// -------------------------------------------------------------------------------
    	public int GetDuration(GameObject player)
    	{
    	
    		CurrencyTemplate _template 	= template;
    		int _level 					= player.GetComponent<AccountManager>().level;
    		float _baseValue			= template.autoProduction.duration.Get(_level);
    		float _modifier 			= 0;
    		
    		_modifier 					+= player.GetComponent<BuildingManager>().GetDuration_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<TechnologyManager>().GetDuration_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<EnhancementManager>().GetDuration_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<UnitManager>().GetDuration_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<EventManager>().GetDuration_AutoCurrencyProduction(_template, _baseValue);
    		
    		return Convert.ToInt32(DateTime.UtcNow.Subtract(new DateTime(timeStamp)).TotalSeconds / _baseValue);
    	}
    	
    	// -------------------------------------------------------------------------------
    	public long GetCapacity(GameObject player)
    	{
    		
    		CurrencyTemplate _template 	= template;
    		int _level 					= player.GetComponent<AccountManager>().level;
    		long _baseValue				= template.capacity.Get(_level);
    		long _modifier 				= 0;
    		
    		_modifier 					+= player.GetComponent<BuildingManager>().GetCapacity_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<TechnologyManager>().GetCapacity_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<EnhancementManager>().GetCapacity_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<UnitManager>().GetCapacity_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<EventManager>().GetCapacity_AutoCurrencyProduction(_template, _baseValue);
    		
    		return _baseValue + _modifier;
    	}
    	
    	// -------------------------------------------------------------------------------
    	public long GetProduction(GameObject player, int _intervals=0)
    	{
    	
    		CurrencyTemplate _template 	= template;
    		int _level 					= player.GetComponent<AccountManager>().level;
    		long _baseValue				= template.autoProduction.amount.Get(_level);
    		long _modifier 				= 0;
    		
    		_modifier 					+= player.GetComponent<BuildingManager>().GetProduction_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<TechnologyManager>().GetProduction_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<EnhancementManager>().GetProduction_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<UnitManager>().GetProduction_AutoCurrencyProduction(_template, _baseValue);
    		_modifier 					+= player.GetComponent<EventManager>().GetProduction_AutoCurrencyProduction(_template, _baseValue);
    		
    		return Convert.ToInt64(_intervals * (_baseValue - _modifier) );
    	}
    	
	}
	
}