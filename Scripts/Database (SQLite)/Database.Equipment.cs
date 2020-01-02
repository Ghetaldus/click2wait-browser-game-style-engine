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
		class c2w_equipment
		{
			public string 	owner 	{ get; set; }
			public int		slot	{ get; set; }
			public string 	name 	{ get; set; }
			public int 		level 	{ get; set; }
			
			public byte 	state 	{ get; set; }
			public double 	timer 	{ get; set; }
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Equipment()
		{
	   		connection.CreateTable<c2w_equipment>();
        	connection.CreateIndex(nameof(c2w_equipment), new []{"owner", "name"});
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Equipment(GameObject player)
		{
	 		EquipmentManager manager = player.GetComponent<EquipmentManager>();
	   		manager.CreateDefaultData();		
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Equipment(GameObject player)
		{
			EquipmentManager manager = player.GetComponent<EquipmentManager>();
	   		
			foreach (c2w_equipment row in connection.Query<c2w_equipment>("SELECT * FROM c2w_equipment WHERE owner=?", player.name))
			{
				if (row.slot < manager.GetCapacity)
				{
					if (String.IsNullOrWhiteSpace(row.name))
					{
						manager.AddEntry(row.slot, null, 0, 0, 0);
					}
					else if (EquipmentTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out EquipmentTemplate template))
                	{
						manager.AddEntry(row.slot, template, row.state, row.timer, row.level);
					}
					else Debug.LogWarning("Load: Skipped equipment " + row.name + " for " + player.name + " as it was not found in Resources.");
				}
            	else Debug.LogWarning("Skipped equipment slot " + row.slot + " for " + player.name + " because it's bigger than size " + manager.GetCapacity);
			}
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Equipment(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_equipment WHERE owner=?", player.name);
	   		
	   		EquipmentManager manager = player.GetComponent<EquipmentManager>();
	   		
	   		List<EquipmentSyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; i++)
	   		{
	   			
	   			EquipmentSyncStruct entry = list[i];
	   			
	   			connection.InsertOrReplace(new c2w_equipment{
                	owner 			= player.name,
                	slot			= entry.slot,
                	name 			= entry.name,
                	level 			= entry.level,
                	state 			= entry.state,
                	timer 			= entry.timer
            	});
	   		}
		}
		
		// -------------------------------------------------------------------------------

	}

}