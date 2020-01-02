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
	public partial class SpellManager : EntityManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected SpellReward[] defaultSpells;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected SpellReward[] dummySpells;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListSpellSyncStruct syncSpells = new SyncListSpellSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, SpellTemplate _template, byte _state, double _timer, int _level)
		{
			SpellSyncStruct syncStruct = new SpellSyncStruct(_slot, _template, _state, _timer, _level);
			syncSpells.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<SpellSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<SpellSyncStruct> entryList = new List<SpellSyncStruct>();
			
			foreach (SpellSyncStruct entry in syncSpells)
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
		
			syncSpells.Clear();
			
			int length = (defaultSpells == null || defaultSpells.Length == 0) ? 0 : defaultSpells.Length;
			
			if (defaultSpells != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultSpells.Length; i++)
					AddEntry(i, defaultSpells[i].template, defaultSpells[i].state, defaultSpells[i].timer, UnityEngine.Random.Range(defaultSpells[i].minLevel, defaultSpells[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncSpells.OrderBy( x => random.Next() );
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummySpells == null || dummySpells.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					SpellReward entry = dummySpells[random.Next(0, dummySpells.Length)];
					
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
			for (int i = 0; i < syncSpells.Count; i++)
				if (!syncSpells[i].Active)
					return syncSpells[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncSpells.Count; i++)
				if (syncSpells[i].slot == _slot)
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
			foreach (SpellSyncStruct entry in syncSpells)
			{
			
			}
		}
		
		[Client]
		protected override void UpdateClient()
		{
			foreach (SpellSyncStruct entry in syncSpells)
			{
			
			}
		}
		
	}

}