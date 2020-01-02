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
		class c2w_unit
		{
			public string 	owner 	{ get; set; }
			public int		slot	{ get; set; }
			public string 	name 	{ get; set; }
			public int 		level 	{ get; set; }
			
			public byte 	state 	{ get; set; }
			public double 	timer 	{ get; set; }
			
			// PRIMARY KEY (owner, name) created manually.
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Unit()
		{
			connection.CreateTable<c2w_unit>();
        	connection.CreateIndex(nameof(c2w_unit), new []{"owner", "name"});
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Unit(GameObject player)
		{
			UnitManager manager = player.GetComponent<UnitManager>();
	   		manager.CreateDefaultData();
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Unit(GameObject player)
		{
			UnitManager manager = player.GetComponent<UnitManager>();
	   		
			foreach (c2w_unit row in connection.Query<c2w_unit>("SELECT * FROM c2w_unit WHERE owner=?", player.name))
			{
				if (row.slot < manager.GetCapacity)
				{
					if (String.IsNullOrWhiteSpace(row.name))
					{
						manager.AddEntry(row.slot, null, 0, 0, 0);
					}
					else if (UnitTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out UnitTemplate template))
                	{
						manager.AddEntry(row.slot, template, row.state, row.timer, row.level);
					}
					else Debug.LogWarning("Load: Skipped unit " + row.name + " for " + player.name + " as it was not found in Resources.");
				}
            	else Debug.LogWarning("Skipped unit slot " + row.slot + " for " + player.name + " because it's bigger than size " + manager.GetCapacity);
			}
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Unit(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_unit WHERE owner=?", player.name);
	   		
	   		UnitManager manager = player.GetComponent<UnitManager>();
	   		
	   		List<UnitSyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; ++i)
	   		{
	   			UnitSyncStruct entry = list[i];
	   			
	   			connection.InsertOrReplace(new c2w_unit{
                	owner 			= player.name,
                	slot			= entry.slot,
                	name 			= entry.name,
                	level 			= entry.level,
                	state 			= entry.state,
                	timer		 	= entry.timer
            	});
	   		}
		}
        
		// -------------------------------------------------------------------------------
	   	
	}

}