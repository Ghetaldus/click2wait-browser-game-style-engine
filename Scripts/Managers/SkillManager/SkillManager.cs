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
	public partial class SkillManager : EntityManager
	{
		[Header("Default Data")]
		[SerializeField]
		protected SkillReward[] defaultSkills;
		[Tooltip("Dummy data is randomly added to empty slots")]
		[SerializeField]
		protected SkillReward[] dummySkills;
		[Tooltip("Randomize the position index of default data?")]
		[SerializeField]
		protected bool randomizeIndex = false;
		
		protected SyncListSkillSyncStruct syncSkills = new SyncListSkillSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, SkillTemplate _template, byte _state, double _timer, int _level)
		{
			SkillSyncStruct syncStruct = new SkillSyncStruct(_slot, _template, _state, _timer, _level);
			syncSkills.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<SkillSyncStruct> GetEntries(bool activeOnly=true, SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			List<SkillSyncStruct> entryList = new List<SkillSyncStruct>();
			
			foreach (SkillSyncStruct entry in syncSkills)
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
		
			syncSkills.Clear();
			
			int length = (defaultSkills == null || defaultSkills.Length == 0) ? 0 : defaultSkills.Length;
			
			if (defaultSkills != null && length < GetCapacity)
			{
				for (int i = 0; i < defaultSkills.Length; i++)
					AddEntry(i, defaultSkills[i].template, defaultSkills[i].state, defaultSkills[i].timer, UnityEngine.Random.Range(defaultSkills[i].minLevel, defaultSkills[i].maxLevel));
			}
					
			InsertDummyData(length, GetCapacity-1);
			
			if (randomizeIndex)
				syncSkills.OrderBy( x => random.Next() );
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public override void InsertDummyData(int startIndex, int endIndex)
		{
			
			for (int i = startIndex; i <= endIndex; i++)
			{
				if (dummySkills == null || dummySkills.Length == 0)
				{
					AddEntry(i, null, 0, 0, 0);
				}
				else
				{
				
					SkillReward entry = dummySkills[random.Next(0, dummySkills.Length)];
					
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
			for (int i = 0; i < syncSkills.Count; i++)
				if (!syncSkills[i].Active)
					return syncSkills[i].slot;
			return -1;
		}
		
		// -------------------------------------------------------------------------------
		protected override int GetIndexBySlot(int _slot)
		{
			for (int i = 0; i < syncSkills.Count; i++)
				if (syncSkills[i].slot == _slot)
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
			foreach (SkillSyncStruct entry in syncSkills)
			{
			
			}
		}
		
		[Client]
		protected override void UpdateClient()
		{
			foreach (SkillSyncStruct entry in syncSkills)
			{
			
			}
		}
				
	}

}