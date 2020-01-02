// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPanelNavigation : UIPanel
	{
		
		public GameObject[] navigationButtons;
		
		protected UIButtonGroup buttonGroup;
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			windowPanel.SetActive(localPlayer != null);
				
			if (localPlayer == null)
				return;
			
			if (buttonGroup == null)
				buttonGroup = GetComponent<UIButtonGroup>();
				
			if (buttonGroup != null)
				buttonGroup.Clear();
								
			foreach (GameObject go in navigationButtons)
				go.GetComponent<UIButton>().init(localPlayer, buttonGroup);
				
		}
		
	}

}