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
		class c2w_enhancement
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
		public void Connect_Enhancement()
		{
	   		connection.CreateTable<c2w_enhancement>();
        	connection.CreateIndex(nameof(c2w_enhancement), new []{"owner", "name"});
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Enhancement(GameObject player)
		{
	 		EnhancementManager manager = player.GetComponent<EnhancementManager>();
	   		manager.CreateDefaultData();		
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Enhancement(GameObject player)
		{
	   		EnhancementManager manager = player.GetComponent<EnhancementManager>();
	   		
			foreach (c2w_enhancement row in connection.Query<c2w_enhancement>("SELECT * FROM c2w_enhancement WHERE owner=?", player.name))
			{
				if (row.slot < manager.GetCapacity)
				{
					if (String.IsNullOrWhiteSpace(row.name))
					{
						manager.AddEntry(row.slot, null, 0, 0, 0);
					}
					else if (EnhancementTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out EnhancementTemplate template))
                	{
						manager.AddEntry(row.slot, template, row.state, row.timer, row.level);
					}
					else Debug.LogWarning("Load: Skipped enhancement " + row.name + " for " + player.name + " as it was not found in Resources.");
				}
            	else Debug.LogWarning("Skipped enhancement slot " + row.slot + " for " + player.name + " because it's bigger than size " + manager.GetCapacity);
			}
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Enhancement(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_enhancement WHERE owner=?", player.name);
	   		
	   		EnhancementManager manager = player.GetComponent<EnhancementManager>();
	   		
	   		List<EnhancementSyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; i++)
	   		{
	   			
	   			EnhancementSyncStruct entry = list[i];
	   			
	   			connection.InsertOrReplace(new c2w_enhancement{
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