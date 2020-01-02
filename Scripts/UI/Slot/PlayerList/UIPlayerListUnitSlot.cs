// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListUnitSlot : UISlot<UnitSyncStruct>
	{
		
		// -------------------------------------------------------------------------------
		public override void Init(GameObject _localPlayer, ref UnitSyncStruct _entry)
		{
			
			base.Init(_localPlayer, ref _entry);
			
			if (entry.Active)
			{
				
				backgroundImage.sprite 	= entry.template.backgroundIcon;
				borderImage.sprite 		= entry.template.rarity.borderImage;
				image.sprite 	= entry.template.smallIcon;
				
				textValue.text = entry.Level();
			
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => { UIPlayerViewUnit.singleton.Init(localPlayer, entry); });
				
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
		
			if (entry.TimeRemaining() > 0)
			{
				//timerText.text = entry.PrettyTimeRemaining;
				//timerCircle = entry.PercentTimeRemaining;
				timerOverlay.SetActive(true);
			}
			else
				timerOverlay.SetActive(false);
		
		}
		
		// -------------------------------------------------------------------------------
	}

}