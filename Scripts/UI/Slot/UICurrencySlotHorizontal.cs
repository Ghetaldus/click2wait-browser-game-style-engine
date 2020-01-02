// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UICurrencySlotHorizontal : UISlot<CurrencySyncStruct>
	{
		
		// -------------------------------------------------------------------------------	
		public override void Init(GameObject _localPlayer, ref CurrencySyncStruct _entry)
		{
			
			base.Init(_localPlayer, ref _entry);
			
			if (entry.Active)
			{
			
				if (backgroundImage)
					backgroundImage.sprite 	= entry.template.backgroundIcon;
			
				if (borderImage)
					borderImage.sprite 		= entry.template.rarity.borderImage;
			
			
				image.sprite 	= entry.template.smallIcon;
				
				textValue.text	= entry.amount + "/" + entry.GetCapacity(_localPlayer);
			
			}
			
		}
		
		
	}

}