// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	public partial class UnitManager
	{

		// -------------------------------------------------------------------------------
		public long GetCapacity_AutoCurrencyProduction(CurrencyTemplate _template, long _baseValue)
		{
			long value = 0;
			
			foreach (UnitSyncStruct entry in syncUnits)
			{
				if (!entry.Active) continue;
				
				foreach (CurrencyProductionModifier modifier in entry.template.autoCurrencyProductionModifier)
					value += modifier.GetCapacityModifier(_template, entry.level, _baseValue);
			}
			
			return value;
		}
		
		// -------------------------------------------------------------------------------
		public long GetProduction_AutoCurrencyProduction(CurrencyTemplate _template, long _baseValue)
		{
			long value = 0;
			
			foreach (UnitSyncStruct entry in syncUnits)
			{
				if (!entry.Active) continue;
				
				foreach (CurrencyProductionModifier modifier in entry.template.autoCurrencyProductionModifier)
					value += modifier.GetProductionModifier(_template, entry.level, _baseValue);
			}
			
			return value;
		}
		
		// -------------------------------------------------------------------------------
		public float GetDuration_AutoCurrencyProduction(CurrencyTemplate _template, float _baseValue)
		{
			float value = 0;
			
			foreach (UnitSyncStruct entry in syncUnits)
			{
				if (!entry.Active) continue;
				
				foreach (CurrencyProductionModifier modifier in entry.template.autoCurrencyProductionModifier)
					value += modifier.GetDurationModifier(_template, entry.level, _baseValue);
			}
			
			return value;
		}
		
	}

}