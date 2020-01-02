// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListTechnology : UIPanel
	{
		
		[Header("Player Achievements")]
		public DisplayType displayType;
		
		public UIPlayerListTechnologySlot 	horizontalSlotPrefab;
		public UIPlayerListTechnologySlot	verticalSlotPrefab;
		public UIPlayerListTechnologySlot	gridSlotPrefab;
		
		protected TechnologyManager manager;
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			if (manager == null)
				manager = localPlayer.GetComponent<TechnologyManager>();
			
			for (int i = 0; i < contentGroup.childCount; i++)
            	GameObject.Destroy(contentGroup.GetChild(i).gameObject);
			
			foreach (TechnologySyncStruct _entry in manager.GetEntries(true, SortOrder.None, category))
			{
				
				TechnologySyncStruct entry = _entry;
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
				go.GetComponent<UIPlayerListTechnologySlot>().Init(localPlayer, ref entry);
				
			}
			
		}
		
	}

}