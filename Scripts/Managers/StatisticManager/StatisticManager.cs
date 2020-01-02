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
	public partial class StatisticManager : SimpleManager
	{
		
		protected StatisticReward[] defaultStatistics;
		protected SyncListStatisticSyncStruct syncStatistics = new SyncListStatisticSyncStruct();
		
		// -------------------------------------------------------------------------------
		[Server]
		public void AddEntry(string _name, byte _type, long _value)
		{
			StatisticSyncStruct syncStruct = new StatisticSyncStruct(_name, _type, _value);
			syncStatistics.Add(syncStruct);
		}
		
		// -------------------------------------------------------------------------------
		public List<StatisticSyncStruct> GetEntries(bool activeOnly=true, SortOrder sortOrder=SortOrder.None, string category="")
		{
			return syncStatistics.ToList();
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void CreateDefaultData()
		{
			syncStatistics.Clear();
			
			if (defaultStatistics == null || defaultStatistics.Length == 0) return;
			
	   		foreach (StatisticReward defaults in defaultStatistics)
	   			AddEntry(defaults.name, defaults.type, UnityEngine.Random.Range(defaults.minAmount, defaults.maxAmount));
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			
		}
		
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		public void TrackCurrency(string _name, byte _type, long _value)
		{
			
			for (int i = 0; i < syncStatistics.Count; i++)
			{
				if (syncStatistics[i].GetMatch(_name, _type))
				{
					StatisticSyncStruct entry = syncStatistics[i];
					entry.value += _value;
					syncStatistics[i] = entry;
					return;
				}
			}
			
			AddEntry(_name, _type, _value);
			
		}
		
		// -------------------------------------------------------------------------------
	
	}

}