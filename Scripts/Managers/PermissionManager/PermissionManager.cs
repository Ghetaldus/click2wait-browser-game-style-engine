// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class PermissionManager : SimpleManager
	{
		
		public Requirements[] requirements;
		
		// -------------------------------------------------------------------------------
		public bool CheckRequirements(ObjectType _objectType, ActionType _actionType)
		{
		
			foreach (Requirements requirement in requirements)
			{
				if (requirement.objectType == _objectType && requirement.actionType == _actionType)
				{
					// now check the requirement
					// return true;
					// return false;
				}
		
			}
			
			return true; //TODO: set to false
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			
		}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			
		}
		
		// -------------------------------------------------------------------------------
	
	}

}