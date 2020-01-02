// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListAchievementSlot : UISlot<AchievementSyncStruct>
	{
		
		// -------------------------------------------------------------------------------
		public override void Init(GameObject _localPlayer, ref AchievementSyncStruct _entry)
		{
			
			base.Init(_localPlayer, ref _entry);
			
			if (entry.Active)
			{
			
				backgroundImage.sprite 	= entry.template.backgroundIcon;
				borderImage.sprite 		= entry.template.rarity.borderImage;
				image.sprite 	= entry.template.smallIcon;
				
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => { UIPlayerViewAchievement.singleton.Init(localPlayer, entry); });
				
				ThrottledUpdate();
				
			}
			else
			{
				Reset();
				//button.interactable = permissionManager.CheckRequirements(objectType, actionType);
				//button.onClick.AddListener(() => { BUY });
			}			
			
		}
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!entry.Active) return;
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}