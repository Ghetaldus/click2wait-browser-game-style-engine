﻿// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListTechnologySlot : UISlot<TechnologySyncStruct>
	{
		
		// -------------------------------------------------------------------------------
		public override void Init(GameObject _localPlayer, ref TechnologySyncStruct _entry)
		{
			
			base.Init(_localPlayer, ref _entry);
			
			if (entry.Active)
			{
			
				backgroundImage.sprite 	= entry.template.backgroundIcon;
				borderImage.sprite 		= entry.template.rarity.borderImage;
				image.sprite 	= entry.template.smallIcon;
				
				textValue.text = entry.Level();
				
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => { UIPlayerViewTechnology.singleton.Init(localPlayer, entry); });
				
				ThrottledUpdate();
			
			}
			else
			{
				Reset();
				//button.interactable = permissionManager.CheckRequirements(objectType, actionType);
				//button.onClick.AddListener(() => { BUY });
			}



			
			
			textValue.text = entry.Level();
			
			
			
		}
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			if (!entry.Active) return;
			
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