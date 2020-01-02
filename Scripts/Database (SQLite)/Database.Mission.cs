// (c) www.click2wait.net

using UnityEngine;
using Mirror;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using UnityEngine.AI;
using click2wait;

namespace click2wait
{

	public partial class Database
	{
	
		// -------------------------------------------------------------------------------
		class c2w_mission
		{
			public string 	owner 	{ get; set; }
			public int		slot	{ get; set; }
			public string 	name 	{ get; set; }
			public int		level	{ get; set; }
			public int 		id 		{ get; set; }
			
			public byte 	state 	{ get; set; }
			public double 	timer 	{ get; set; }
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Group()
		{
	   		connection.CreateTable<c2w_mission>();
        	connection.CreateIndex(nameof(c2w_mission), new []{"owner", "name"});
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Mission(GameObject player)
		{
	   		MissionManager manager = player.GetComponent<MissionManager>();
	   		manager.CreateDefaultData();
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void Load_Mission(GameObject player)
		{
	   		MissionManager manager = player.GetComponent<MissionManager>();
	   		
			foreach (c2w_mission row in connection.Query<c2w_mission>("SELECT * FROM c2w_mission WHERE owner=?", player.name))
			{
				if (row.slot < manager.GetCapacity)
				{
					if (String.IsNullOrWhiteSpace(row.name))
					{
						manager.AddEntry(row.slot, null, 0, 0, 0, 0);
					}
					else if (MissionTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out MissionTemplate template))
                	{
						manager.AddEntry(row.slot, template, row.state, row.timer, row.level, row.id);
					}
					else Debug.LogWarning("Load: Skipped mission " + row.name + " for " + player.name + " as it was not found in Resources.");
				}
            	else Debug.LogWarning("Skipped mission slot " + row.slot + " for " + player.name + " because it's bigger than size " + manager.GetCapacity);
			}
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Mission(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_mission WHERE owner=?", player.name);
	   		
	   		MissionManager manager = player.GetComponent<MissionManager>();
	   		
	   		List<MissionSyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; i++)
	   		{
	   			
	   			MissionSyncStruct entry = list[i];
	   			
	   			connection.InsertOrReplace(new c2w_mission{
                	owner 			= player.name,
                	slot			= entry.slot,
                	name 			= entry.name,
                	level			= entry.level,
                	id 				= entry.id,
                	state 			= entry.state,
                	timer 			= entry.timer
            	});
	   		}
		}
		
		// -------------------------------------------------------------------------------

	}

}