// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using click2wait;

namespace click2wait {

	public abstract partial class EntityTemplate : PropertyTemplate
	{
	
		
		[Header("Costs")]
		public LevelCurrencyCost[] buyCost;
		public LevelCurrencyCost[] sellCost;
		
	}

}