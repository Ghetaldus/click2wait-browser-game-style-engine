// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	public partial class UnitManager
	{
		
		// -------------------------------------------------------------------------------
		[Command]
		public override void CmdSellEntry(int _slot, long _amount)
		{
			int index = GetIndexBySlot(_slot);
			if (syncUnits[index].CanSell)
				SellEntry(index, _amount);
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void SellEntry(int _index, long _amount)
		{
			UnitSyncStruct entry = syncUnits[_index];
			entry.Remove(_amount);
			syncUnits[_index] = entry;
			
			GetComponentInParent<CurrencyManager>().AddCurrency(entry.template.sellCost, entry.level);
			
		}
		
		// -------------------------------------------------------------------------------
    	void OnDragAndDrop_InventorySlot_EquipmentSlot(int _fromSlot, int _toSlot)
    	{
        	CmdSwapUnit(_fromSlot, _toSlot);
    	}
		
		// -------------------------------------------------------------------------------
		[Command]
		protected void CmdSwapUnit(int _fromSlot, int _toSlot)
		{
			
		}
		
		public override bool CanEntryAction(int index)
		{
			return true;
		}
		
		[Command]
		public override void CmdCancelEntryAction(int index)
		{
		}
	
		[Server]
		protected override void CancelEntryAction(int index)
		{
		}

		public override bool CanUpgradeEntry(int index)
		{
			return true;
		}

		[Command]
		public override void CmdUpgradeEntry(int index)
		{
		}

		[Server]
		protected override void UpgradeEntry(int index)
		{
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