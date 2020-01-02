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
	public partial class ItemManager : EntityManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected ItemReward[] defaultItems;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected ItemReward[] dummyItems;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListItemSyncStruct syncItems = new SyncListItemSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, ItemTemplate _template, byte _state, double _timer, long _amount)
		{
			ItemSyncStruct syncStruct = new ItemSyncStruct(_slot, _template, _state, _timer, _amount);
			syncItems.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<ItemSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<ItemSyncStruct> entryList = new List<ItemSyncStruct>();
			
			foreach (ItemSyncStruct entry in syncItems)
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
		
			syncItems.Clear();
			
			int length = (defaultItems == null || defaultItems.Length == 0) ? 0 : defaultItems.Length;
			
			if (defaultItems != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultItems.Length; i++)
					AddEntry(i, defaultItems[i].template, defaultItems[i].state, defaultItems[i].timer, UnityEngine.Random.Range(defaultItems[i].minAmount, defaultItems[i].maxAmount));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncItems.OrderBy( x => random.Next() );
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummyItems == null || dummyItems.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					ItemReward entry = dummyItems[random.Next(0, dummyItems.Length)];
					
					if (entry == null)
						AddEntry(i, null, 0, 0, 0);
					else
						AddEntry(i, entry.template, entry.state, entry.timer, UnityEngine.Random.Range(entry.minAmount, entry.maxAmount));
				
				}
			}
			
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetFreeSlot()
		{
			for (int i = 0; i < syncItems.Count; i++)
				if (!syncItems[i].Active)
					return syncItems[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncItems.Count; i++)
				if (syncItems[i].slot == _slot)
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
			foreach (ItemSyncStruct entry in syncItems)
			{
			
			}
		}
		
		[Client]
		protected override void UpdateClient()
		{
			foreach (ItemSyncStruct entry in syncItems)
			{
			
			}
		}
		
			
	}

}