// (c) www.click2wait.net

using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	
	public partial struct AchievementSyncStruct
	{
	
		// -------------------------------------------------------------------------------
		public string name
		{
			get { return (template == null) ? "" : template.name; }
		}
		
	}
	
}