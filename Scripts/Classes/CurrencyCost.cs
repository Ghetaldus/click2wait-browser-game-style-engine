// (c) www.click2wait.net

using System;
using System.Text;
using UnityEngine;
using click2wait;

namespace click2wait {
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class LevelCurrencyCost : CurrencyCost
	{
		public LinearGrowthLong amount;
		
		public override bool Valid
		{
			get { return (template != null && amount.Get(1) != 0); }
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class FixedCurrencyCost : CurrencyCost
	{
		public long amount;
		
		public override bool Valid
		{
			get { return (template != null && amount != 0); }
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	[System.Serializable]
	public partial class CurrencyCost
	{
		public CurrencyTemplate template;
		
		public virtual bool Valid
		{
			get { return (template != null); }
		}
		
	}
	
	// -----------------------------------------------------------------------------------
	
}