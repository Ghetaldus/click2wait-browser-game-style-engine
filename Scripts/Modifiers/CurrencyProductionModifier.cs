// (c) www.click2wait.net

using System;
using System.Text;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	[System.Serializable]
	public partial class CurrencyProductionModifier
	{
		
		public CurrencyTemplate template;
		
		[Header("Fixed Value Modifiers")]
		public LinearGrowthLong productionFlat;
		public LinearGrowthLong capacityFlat;
		public LinearGrowthFloat durationFlat;
		
		[Header("Percentage Modifiers")]
		public LinearGrowthFloat productionPercent;
		public LinearGrowthFloat capacityPercent;
		public LinearGrowthFloat durationPercent;
		
		// -------------------------------------------------------------------------------
		public long GetCapacityModifier(CurrencyTemplate _template, int _level, long _baseValue)
		{
			long value = 0;
			
			if (template != _template)
				return value;
			
			value += capacityFlat.Get(_level);
			value += Convert.ToInt64(_baseValue * capacityPercent.Get(_level));
			
			return value;
		}
		
		// -------------------------------------------------------------------------------
		public long GetProductionModifier(CurrencyTemplate _template, int _level, long _baseValue)
		{
			long value = 0;
			
			if (template != _template)
				return value;
			
			value += productionFlat.Get(_level);
			value += Convert.ToInt64(_baseValue * productionPercent.Get(_level));
			
			return value;
		}
		
		// -------------------------------------------------------------------------------
		public float GetDurationModifier(CurrencyTemplate _template, int _level, float _baseValue)
		{
			float value = 0;
			
			if (template != _template)
				return value;
				
			value += durationFlat.Get(_level);
			value += _baseValue * durationPercent.Get(_level);
			
			return value;
		}
		
	}
	
}