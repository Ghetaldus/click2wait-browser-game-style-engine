// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerViewAchievement : UIModalPanel
	{
		
		public static UIPlayerViewAchievement singleton;
		
		protected AchievementSyncStruct entry;
		
		protected AchievementManager	manager;
		protected PermissionManager permissionManager;
		
		protected ObjectType		objectType = ObjectType.Achievement;
		protected ActionType		actionType = ActionType.Unacquire;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		public void Init(GameObject localPlayer, AchievementSyncStruct _entry)
		{
		
			entry = _entry;
			
			manager	 			= localPlayer.GetComponent<AchievementManager>();
			permissionManager	= localPlayer.GetComponent<PermissionManager>();
			
			
			Setup();
		
		}
				
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{

			
		}
		
		
		
	}

}