// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	public partial class CurrencyManager
	{
		
		// -------------------------------------------------------------------------------
		[Command]
		public override void CmdSellEntry(int _slot, long _amount)
		{
			int index = GetIndexBySlot(_slot);
			if (syncCurrencies[index].CanSell)
				SellEntry(index, _amount);
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void SellEntry(int _index, long _amount)
		{
			CurrencySyncStruct entry = syncCurrencies[_index];
			entry.Remove(_amount);
			syncCurrencies[_index] = entry;
			
			AddCurrency(entry.template.sellCost, _amount);
		}

		/*
		public override bool CanBuyEntry()
		{
		}

		[Command]
		public override void CmdBuyEntry()
		{
		}
		
		[Server]
		protected override void BuyEntry()
		{
		}
		*/
		
	}

}