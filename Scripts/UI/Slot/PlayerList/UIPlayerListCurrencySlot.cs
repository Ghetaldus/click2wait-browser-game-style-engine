// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPlayerListCurrencySlot : UISlot<CurrencySyncStruct>
	{
		
		// -------------------------------------------------------------------------------
		public override void Init(GameObject _localPlayer, ref CurrencySyncStruct _entry)
		{
		
			base.Init(_localPlayer, ref _entry);
			
			if (entry.Active)
			{
			
				backgroundImage.sprite 	= entry.template.backgroundIcon;
				borderImage.sprite 		= entry.template.rarity.borderImage;
				image.sprite 			= entry.template.smallIcon;
				
				textValue.text 			= entry.Amount;
			
				button.interactable = permissionManager.CheckRequirements(objectType, actionType) && entry.CanSell;
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => { OnClickSell(); });
			
			}
			else
			{
				Reset();
				//button.interactable = permissionManager.CheckRequirements(objectType, actionType);
				//button.onClick.AddListener(() => { BUY });
			}
			
		}
				
		// -------------------------------------------------------------------------------
		public void OnClickSell()
		{
			UIModalSlider.singleton.Setup("How many do you want to sell?", "Sell", entry.amount, OnConfirmedSell);
		}
		
		// -------------------------------------------------------------------------------
		public void OnConfirmedSell(long _amount)
		{
			
			if (_amount <= 0 || _amount > entry.amount)
				return;
			
			currencyManager.CmdSellEntry(entry.slot, _amount);
			
		}
			
	}

}