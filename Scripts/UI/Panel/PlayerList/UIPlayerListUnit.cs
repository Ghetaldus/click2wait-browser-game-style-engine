// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListUnit : UIPanel
	{
		
		[Header("Player Achievements")]
		public DisplayType displayType;
		
		public UIPlayerListUnitSlot horizontalSlotPrefab;
		public UIPlayerListUnitSlot	verticalSlotPrefab;
		public UIPlayerListUnitSlot	gridSlotPrefab;
		
		protected UnitManager manager;
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			if (manager == null)
				manager = localPlayer.GetComponent<UnitManager>();
			
			for (int i = 0; i < contentGroup.childCount; i++)
            	GameObject.Destroy(contentGroup.GetChild(i).gameObject);
			
			foreach (UnitSyncStruct _entry in manager.GetEntries(false, SortOrder.None, category))
			{
				
				UnitSyncStruct entry = _entry;
				GameObject prefab = null;
				
				if (displayType == DisplayType.Horizontal) {
					prefab = horizontalSlotPrefab.gameObject;
				} else if (displayType == DisplayType.Vertical) {
					prefab = verticalSlotPrefab.gameObject;
				} else if (displayType == DisplayType.Grid) {
					prefab = gridSlotPrefab.gameObject;
				}
				
				GameObject go = GameObject.Instantiate(prefab);
            	go.transform.SetParent(contentGroup, false);
				go.GetComponent<UIPlayerListUnitSlot>().Init(localPlayer, ref entry);
				
			}
			
		}
		
	}

}