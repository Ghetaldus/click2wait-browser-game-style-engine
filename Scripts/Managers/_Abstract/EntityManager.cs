// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	[System.Serializable]
	public abstract partial class EntityManager : PropertyManager
	{
		
		
		// assign
		// cmd assign
		
		// unassign
		// cmd unassign
		
		
		// ----------------------------------------------------------------------- ACTIONS
		
		public abstract bool CanEntryAction(int index);
			
		[Command]
		public abstract void CmdCancelEntryAction(int index);
		
		[Server]
		protected abstract void CancelEntryAction(int index);
		
		// ----------------------------------------------------------------------- UPGRADE
		
		public abstract bool CanUpgradeEntry(int index);
		
		[Command]
		public abstract void CmdUpgradeEntry(int index);
		
		[Server]
		protected abstract void UpgradeEntry(int index);
	
	}

}