// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListBuilding : UIPanel
	{
		
		[Header("Player Buildings")]
		public DisplayType displayType;
		
		public UIPlayerListBuildingSlot horizontalSlotPrefab;
		public UIPlayerListBuildingSlot	verticalSlotPrefab;
		public UIPlayerListBuildingSlot	gridSlotPrefab;
		
		protected BuildingManager manager;
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
	
			if (manager == null)
				manager = localPlayer.GetComponent<BuildingManager>();
			
			for (int i = 0; i < contentGroup.childCount; i++)
            	GameObject.Destroy(contentGroup.GetChild(i).gameObject);
			
			foreach (BuildingSyncStruct _entry in manager.GetEntries(false, SortOrder.None, category))
			{
			
				BuildingSyncStruct entry = _entry;
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
				go.GetComponent<UIPlayerListBuildingSlot>().Init(localPlayer, ref entry);
				
			}
			
		}
		
	}

}