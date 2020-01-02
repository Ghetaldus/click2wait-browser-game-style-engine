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
	public partial class EquipmentManager : EntityManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected EquipmentReward[] defaultEquipment;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected EquipmentReward[] dummyEquipment;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListEquipmentSyncStruct syncEquipment = new SyncListEquipmentSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, EquipmentTemplate _template, byte _state, double _timer, int _level)
		{
			EquipmentSyncStruct syncStruct = new EquipmentSyncStruct(_slot, _template, _state, _timer, _level);
			syncEquipment.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<EquipmentSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<EquipmentSyncStruct> entryList = new List<EquipmentSyncStruct>();
			
			foreach (EquipmentSyncStruct entry in syncEquipment)
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
			syncEquipment.Clear();
			
			int length = (defaultEquipment == null || defaultEquipment.Length == 0) ? 0 : defaultEquipment.Length;
			
			if (defaultEquipment != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultEquipment.Length; i++)
					AddEntry(i, defaultEquipment[i].template, defaultEquipment[i].state, defaultEquipment[i].timer, UnityEngine.Random.Range(defaultEquipment[i].minLevel, defaultEquipment[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncEquipment.OrderBy( x => random.Next() );
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummyEquipment == null || dummyEquipment.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					EquipmentReward entry = dummyEquipment[random.Next(0, dummyEquipment.Length)];
					
					if (entry == null)
						AddEntry(i, null, 0, 0, 0);
					else
						AddEntry(i, entry.template, entry.state, entry.timer, UnityEngine.Random.Range(entry.minLevel, entry.maxLevel));
				
				}
			}
			
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetFreeSlot()
		{
			for (int i = 0; i < syncEquipment.Count; i++)
				if (!syncEquipment[i].Active)
					return syncEquipment[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncEquipment.Count; i++)
				if (syncEquipment[i].slot == _slot)
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
			foreach (EquipmentSyncStruct entry in syncEquipment)
			{
			
			}
		}
		
		[Client]
		protected override void UpdateClient()
		{
			foreach (EquipmentSyncStruct entry in syncEquipment)
			{
			
			}
		}
				
	}

}