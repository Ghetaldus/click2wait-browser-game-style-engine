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
	public partial class AchievementManager : BaseManager
	{
	
		[Header("Default Data")]
		[SerializeField]
		protected AchievementReward[] defaultAchievements;
		protected SyncListAchievementSyncStruct syncAchievements = new SyncListAchievementSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(int _slot, AchievementTemplate _template, long _value)
		{
			AchievementSyncStruct syncStruct = new AchievementSyncStruct(_slot, _template, _value);
			syncAchievements.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<AchievementSyncStruct> GetEntries(SortOrder _sortOrder=SortOrder.None, string _category="")
		{
		
			if (_sortOrder == SortOrder.Priority)
				if (!string.IsNullOrWhiteSpace(_category))
					return syncAchievements.Where(x => x.template.category == _category).OrderBy(x => x.template.priority).ToList();
				else
					return syncAchievements.OrderBy(x => x.template.priority).ToList();
				
			if (!string.IsNullOrWhiteSpace(_category))
				return syncAchievements.Where(x => x.template.category == _category).ToList();
			
			return syncAchievements.ToList();
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void CreateDefaultData()
		{
			syncAchievements.Clear();
			
			if (defaultAchievements == null || defaultAchievements.Length == 0) return;
			
			for (int i = 0; i < defaultAchievements.Length; i++)
	   			AddEntry(i, defaultAchievements[i].template, UnityEngine.Random.Range(defaultAchievements[i].minAmount, defaultAchievements[i].maxAmount));
		
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			foreach (AchievementSyncStruct entry in syncAchievements)
			{
			
			}
		}
		
		[Client]
		protected override void UpdateClient()
		{
			foreach (AchievementSyncStruct entry in syncAchievements)
			{
			
			}
		}
		
			
	}

}