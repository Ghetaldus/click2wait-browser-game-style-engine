// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerViewEquipment : UIPlayerViewPanel<EquipmentSyncStruct, EquipmentManager>
	{
		
		public static UIPlayerViewEquipment singleton;
		
		
		
		protected ObjectType		objectType = ObjectType.Equipment;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		public override void Init(GameObject localPlayer, EquipmentSyncStruct _entry)
		{
		
			entry = _entry;
			
			manager 			= localPlayer.GetComponent<EquipmentManager>();
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