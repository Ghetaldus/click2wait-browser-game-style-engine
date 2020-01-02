// (c) www.click2wait.net

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using click2wait;

namespace click2wait {
	
	[RequireComponent(typeof(AchievementManager))]
	[RequireComponent(typeof(BuffManager))]
	[RequireComponent(typeof(BuildingManager))]
	[RequireComponent(typeof(CurrencyManager))]
	[RequireComponent(typeof(EnhancementManager))]
	[RequireComponent(typeof(EquipmentManager))]
	[RequireComponent(typeof(EventManager))]
	[RequireComponent(typeof(ItemManager))]
	[RequireComponent(typeof(MissionManager))]
	[RequireComponent(typeof(SkillManager))]
	[RequireComponent(typeof(SpellManager))]
	[RequireComponent(typeof(StatisticManager))]
	[RequireComponent(typeof(TechnologyManager))]
	[RequireComponent(typeof(UnitManager))]
	[RequireComponent(typeof(PermissionManager))]
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class AccountManager : BaseManager
	{
	
		public static Dictionary<string, GameObject> onlinePlayers = new Dictionary<string, GameObject>();
		
		public static GameObject localPlayer;
		
		protected override void Start()
    	{
        	//if (!isServer && !isClient) return;
        	base.Start();
        	onlinePlayers[name] = this.gameObject;
		}
		
		public override void OnStartLocalPlayer()
    	{
        	localPlayer = this.gameObject;
		}
		
		void OnDestroy()
    	{
    		if (isLocalPlayer)
            	localPlayer = null;
            onlinePlayers.Remove(name);
        }
		
		[Server]
		protected override void UpdateServer()
		{
			
		}
		
		[Client]
		protected override void UpdateClient()
		{
			
		}
		
	}

}