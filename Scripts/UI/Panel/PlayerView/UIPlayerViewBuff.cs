// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerViewBuff : UIPlayerViewPanel<BuffSyncStruct, BuffManager>
	{
		
		public static UIPlayerViewBuff singleton;
		
		
		
		protected ObjectType		objectType = ObjectType.Buff;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		public override void Init(GameObject localPlayer, BuffSyncStruct _entry)
		{
		
			entry = _entry;
			
			manager		 		= localPlayer.GetComponent<BuffManager>();
			permissionManager	= localPlayer.GetComponent<PermissionManager>();
			
			buttonSell.interactable = permissionManager.CheckRequirements(objectType, ActionType.Unacquire) && entry.CanSell;
			
			
			Setup();
		
		}
				
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			
		}
		
		// -------------------------------------------------------------------------------
		public void OnClickSell()
		{
			UIModalPrompt.singleton.Setup("Do you really want to sell?", "Sell", "Cancel", onClickConfirmSell, onClickCancel);
		}
		
		// -------------------------------------------------------------------------------
		public void onClickConfirmSell()
		{
			manager.CmdSellEntry(entry.slot, 1);
			base.onClickConfirm();
		}
		
		
	}

}