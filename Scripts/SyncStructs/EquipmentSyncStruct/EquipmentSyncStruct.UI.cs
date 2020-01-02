// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	public partial struct EquipmentSyncStruct
	{
	
		// -------------------------------------------------------------------------------
		public string name
		{
			get { return (template == null) ? "" : template.name; }
		}
		
    	// -------------------------------------------------------------------------------
    	public string Level()
    	{
    		return "L" + level.ToString();
    	}
    
	}
	
}