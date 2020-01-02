// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {

	[System.Serializable]
	public abstract partial class BaseManager : NetworkBehaviour
	{
	
		[Header("Level")]
		[SyncVar]
		public int level = 1;
		public int maxLevel = 3;
		public LinearGrowthInt capacity = new LinearGrowthInt{baseValue=99, bonusPerLevel=0};
		public LevelCurrencyCost[] upgradeCost;
		
		[Header("Caching")]
		[Range(0.01f, 99)]
		public float refreshInterval = 1f;
		float _cacheTimer = 0;
		
		public int GetCapacity => capacity.Get(level);
		
		protected System.Random random = new System.Random();
		
		protected bool Check => Time.time > _cacheTimer;
		
		// -------------------------------------------------------------------------------
		protected virtual void Start() {}
		
		// -------------------------------------------------------------------------------
		void Refresh()
		{
			_cacheTimer = Time.time + refreshInterval;
		}
		
		// -------------------------------------------------------------------------------
		protected void Update()
		{
			if (Check)
			{
				if (isClient)
					UpdateClient();
				if (isServer)
					UpdateServer();
				
				Refresh();
			}
		}
		
		[Server]
		protected abstract void UpdateServer();
		
		[Client]
		protected abstract void UpdateClient();
		
		// -------------------------------------------------------------------------------
		public bool CanUpgradeLevel()
		{
			return (level < maxLevel &&
					GetComponentInParent<CurrencyManager>().CanPayCost(upgradeCost, level)
					);
		}
		
		// -------------------------------------------------------------------------------
		[Command]
		public void CmdUpgradeLevel()
		{
			if (CanUpgradeLevel())
				UpgradeLevel();
		}
		
		// -------------------------------------------------------------------------------
		[Server]
		protected virtual void UpgradeLevel()
		{
			GetComponentInParent<CurrencyManager>().PayCost(upgradeCost, level);
			level++;
		}
		
	}

}