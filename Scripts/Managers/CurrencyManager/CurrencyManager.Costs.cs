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

		// ===============================================================================
		// FIXED COSTS
		// ===============================================================================
		
		// -------------------------------------------------------------------------------
		public bool CanPayCost(FixedCurrencyCost[] _cost, long _amount)
		{
			
			bool canPay = false;
			
			foreach (FixedCurrencyCost cost in _cost)
			{
				foreach (CurrencySyncStruct entry in syncCurrencies)
				{
					if (entry.template == cost.template)
						canPay = entry.amount >= cost.amount * _amount;
				}
			}
			
			return canPay;
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void PayCost(FixedCurrencyCost[] _cost, long _amount)
		{
			foreach (FixedCurrencyCost cost in _cost)
			{
				for (int i = 0; i < syncCurrencies.Count; i++)
				{
					if (syncCurrencies[i].template == cost.template)
					{
						CurrencySyncStruct entry = syncCurrencies[i];
						entry.amount -= cost.amount * _amount;
						syncCurrencies[i] = entry;
					}
				}
			}
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddCurrency(FixedCurrencyCost[] _cost, long _amount)
		{
			foreach (FixedCurrencyCost cost in _cost)
			{
				int index = syncCurrencies.FindIndex(x => x.template == cost.template);
				
				if (index != -1)
				{
					CurrencySyncStruct entry = syncCurrencies[index];
					entry.amount += cost.amount * _amount;
					syncCurrencies[index] = entry;
				}
				else
				{
					index = GetFreeSlot();
					if (index != -1)
						AddEntry(index, cost.template, Constants.STATE_NONE, 0, cost.amount * _amount);
				}
			}
		}
		
		// ===============================================================================
		// LEVELLED COSTS
		// ===============================================================================
		
		// -------------------------------------------------------------------------------
		public bool CanPayCost(LevelCurrencyCost[] _cost, int _level)
		{
			
			bool canPay = false;
			
			foreach (LevelCurrencyCost cost in _cost)
			{
				foreach (CurrencySyncStruct entry in syncCurrencies)
				{
					if (entry.template == cost.template)
						canPay = entry.amount >= cost.amount.Get(_level);
				}
			}
			
			return canPay;
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void PayCost(LevelCurrencyCost[] _cost, int _level)
		{
			foreach (LevelCurrencyCost cost in _cost)
			{
				for (int i = 0; i < syncCurrencies.Count; i++)
				{
					if (syncCurrencies[i].template == cost.template)
					{
						CurrencySyncStruct entry = syncCurrencies[i];
						entry.amount -= cost.amount.Get(_level);
						syncCurrencies[i] = entry;
					}
				}
			}
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddCurrency(LevelCurrencyCost[] _cost, int _level)
		{
			foreach (LevelCurrencyCost cost in _cost)
			{
				int index = syncCurrencies.FindIndex(x => x.template == cost.template);
				
				if (index != -1)
				{
					CurrencySyncStruct entry = syncCurrencies[index];
					entry.amount += cost.amount.Get(_level);
					syncCurrencies[index] = entry;
				}
				else
				{
					index = GetFreeSlot();
					if (index != -1)
						AddEntry(index, cost.template, Constants.STATE_NONE, 0, cost.amount.Get(_level));
				}
			}
		}
		
		// -------------------------------------------------------------------------------
		
	}

}