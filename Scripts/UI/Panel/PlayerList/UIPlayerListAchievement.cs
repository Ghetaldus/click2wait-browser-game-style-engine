// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListAchievement : UIPanel
	{
		
		[Header("Player Achievements")]
		public DisplayType displayType;
		
		public UIPlayerListAchievementSlot 	horizontalSlotPrefab;
		public UIPlayerListAchievementSlot	verticalSlotPrefab;
		public UIPlayerListAchievementSlot	gridSlotPrefab;
		
		protected AchievementManager manager;
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (manager == null)
				manager = localPlayer.GetComponent<AchievementManager>();
			
			for (int i = 0; i < contentGroup.childCount; i++)
            	GameObject.Destroy(contentGroup.GetChild(i).gameObject);
			
			foreach (AchievementSyncStruct _entry in manager.GetEntries(SortOrder.None, category))
			{
			
				AchievementSyncStruct entry = _entry;
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
				go.GetComponent<UIPlayerListAchievementSlot>().Init(localPlayer, ref entry);
				
			}
			
		}
		
	}

}