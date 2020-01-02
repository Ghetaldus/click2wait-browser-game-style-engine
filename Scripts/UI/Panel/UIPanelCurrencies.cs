// (c) www.click2wait.net

using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using click2wait;

namespace click2wait
{

	public partial class UIPanelCurrencies : UIPanel
	{
		
		[Header("Currency Panel")]
		public UICurrencySlotHorizontal slotPrefab;
		[Range(1,99)]public int maxEntries;
		
		protected CurrencyManager manager;
		
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
			
			windowPanel.SetActive(localPlayer != null);
				
			if (localPlayer == null)
				return;
			
			if (manager == null)
				manager = localPlayer.GetComponent<CurrencyManager>();
			
			for (int i = 0; i < contentGroup.childCount; i++)
            	GameObject.Destroy(contentGroup.GetChild(i).gameObject);
			
			int count = 0;
			
			foreach (CurrencySyncStruct _entry in manager.GetEntries(true, SortOrder.Priority))
			{
				
				CurrencySyncStruct entry = _entry;
				
				GameObject go = GameObject.Instantiate(slotPrefab.gameObject);
            	go.transform.SetParent(contentGroup, false);

				go.GetComponent<UICurrencySlotHorizontal>().Init(localPlayer, ref entry);
				
				count++;
				
				if (count > maxEntries)
					break;
				
			}
			
		}
		
	}

}