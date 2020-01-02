// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class CurrencyManager : PropertyManager
	{
		
		[Header("Default Data")]
		[SerializeField]
		protected CurrencyReward[] defaultCurrencies;
				
		protected SyncListCurrencySyncStruct syncCurrencies = new SyncListCurrencySyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, CurrencyTemplate _template, byte _state, long _timeStamp, long _amount)
		{
			CurrencySyncStruct syncStruct = new CurrencySyncStruct(_slot, _template, _state, _timeStamp, _amount);
			syncCurrencies.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<CurrencySyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
			
			List<CurrencySyncStruct> entryList = new List<CurrencySyncStruct>();
			
			foreach (CurrencySyncStruct entry in syncCurrencies)
			{
				if (entry.Active)
				{
					
					if (string.IsNullOrWhiteSpace(_category) ||
						entry.template.category == _category)
						entryList.Add(entry);
										
				}
				else if (!activeOnly && !entry.Active)
					entryList.Add(entry);
			
			}
			
			
			if (activeOnly && _sortOrder == SortOrder.Priority)
				entryList.OrderBy(x => x.template.priority).ToList();
			
			return entryList;
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void CreateDefaultData()
		{
			syncCurrencies.Clear();
			
			int length = (defaultCurrencies == null || defaultCurrencies.Length == 0) ? 0 : defaultCurrencies.Length;
			
			for (int i = 0; i < defaultCurrencies.Length; i++)
	   			AddEntry(i, defaultCurrencies[i].template, defaultCurrencies[i].state, defaultCurrencies[i].timer, UnityEngine.Random.Range(defaultCurrencies[i].minAmount, defaultCurrencies[i].maxAmount));
	   		
	   		InsertDummyData(length, GetCapacity-1);
	   		
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			for (int i = startIndex; i < endIndex; i++)
				AddEntry(i, null, 0, 0, 0);
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetFreeSlot()
		{
			for (int i = 0; i < syncCurrencies.Count; i++)
				if (!syncCurrencies[i].Active)
					return syncCurrencies[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncCurrencies.Count; i++)
				if (syncCurrencies[i].slot == _slot)
					return i;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpgradeLevel()
		{
			
			int startIndex = GetCapacity-1;
			
			base.UpgradeLevel();
		
			InsertDummyData(startIndex, GetCapacity-1);
		
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			for (int i = 0; i < syncCurrencies.Count; i++)
			{
				if (!syncCurrencies[i].Active) continue;
				CurrencySyncStruct entry = syncCurrencies[i];
				entry.Update(this.gameObject);
				syncCurrencies[i] = entry;
			}
		}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient() {}
		
		// -------------------------------------------------------------------------------
		
	}

}