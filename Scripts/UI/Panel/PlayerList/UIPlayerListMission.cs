// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListMission : UIPanel
	{
		
		[Header("Player Achievements")]
		public DisplayType displayType;
		
		public UIPlayerListMissionSlot horizontalSlotPrefab;
		public UIPlayerListMissionSlot	verticalSlotPrefab;
		public UIPlayerListMissionSlot	gridSlotPrefab;
		
		protected MissionManager manager;
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			if (manager == null)
				manager = localPlayer.GetComponent<MissionManager>();
			
			for (int i = 0; i < contentGroup.childCount; i++)
            	GameObject.Destroy(contentGroup.GetChild(i).gameObject);
			
			foreach (MissionSyncStruct _entry in manager.GetEntries(true, SortOrder.None, category))
			{
				
				MissionSyncStruct entry = _entry;
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
				go.GetComponent<UIPlayerListMissionSlot>().Init(localPlayer, ref entry);
				
			}
			
		}
		
	}

}