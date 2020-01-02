// (c) www.click2wait.net

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public abstract partial class UIPlayerViewPanel<T, M> : UIModalPanel
	{
		
		[Header("Buttons")]
		[SerializeField] protected Button buttonSell;
		
		protected T entry;
		protected M manager;
		protected PermissionManager 	permissionManager;
		
		// -------------------------------------------------------------------------------
		public virtual void Init(GameObject localPlayer, T _entry)
		{
		
			entry = _entry;
			
			manager 			= localPlayer.GetComponent<M>();
			permissionManager	= localPlayer.GetComponent<PermissionManager>();
			
			
			Setup();
		
		}
		
		// -------------------------------------------------------------------------------
		
	}

}