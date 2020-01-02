// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIButtonNavigation : UIButton
	{
	
		public UIPanel	 		targetPanel;
		public string			category;
		
		public ObjectType		objectType;
		public ActionType		actionType;
		
		protected PermissionManager manager;
		
		// -------------------------------------------------------------------------------
		public override void init(GameObject _localPlayer, UIButtonGroup _buttonGroup = null)
		{
			
			base.init(_localPlayer, _buttonGroup);
			
			if (manager == null)
				manager = localPlayer.GetComponent<PermissionManager>();
			
			button.interactable = manager.CheckRequirements(objectType, actionType);
			button.onClick.AddListener(() => { targetPanel.Show(category); });
			
		}
		
		// -------------------------------------------------------------------------------
		public override void OnPressed()
		{
			targetPanel.Hide();
			base.OnPressed();
		}
		
	}

}