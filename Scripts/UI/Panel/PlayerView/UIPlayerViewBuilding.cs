// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerViewBuilding : UIPlayerViewPanel<BuildingSyncStruct, BuildingManager>
	{
		
		public static UIPlayerViewBuilding singleton;
		
		
		
		protected ObjectType		objectType = ObjectType.Building;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		public override void Init(GameObject localPlayer, BuildingSyncStruct _entry)
		{
		
			entry = _entry;
			
			manager 			= localPlayer.GetComponent<BuildingManager>();
			permissionManager	= localPlayer.GetComponent<PermissionManager>();
			
			
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