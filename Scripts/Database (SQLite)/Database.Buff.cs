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
		class c2w_buff
		{
			public string 	owner 			{ get; set; }
			public int		slot			{ get; set; }
			public string 	name 			{ get; set; }
			public int 		level 			{ get; set; }
			
			public byte	state 			{ get; set; }
			public double 	timer		 	{ get; set; }
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Buff()
		{
	   		connection.CreateTable<c2w_buff>();
       		connection.CreateIndex(nameof(c2w_buff), new []{"owner", "name"});
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Buff(GameObject player)
		{
	   		BuffManager manager = player.GetComponent<BuffManager>();
	   		manager.CreateDefaultData();
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Buff(GameObject player)
		{
	   
	   		BuffManager manager = player.GetComponent<BuffManager>();
	   		
			foreach (c2w_buff row in connection.Query<c2w_buff>("SELECT * FROM c2w_buff WHERE owner=?", player.name))
			{
				if (row.slot < manager.GetCapacity)
				{
					if (String.IsNullOrWhiteSpace(row.name))
					{
						manager.AddEntry(row.slot, null, 0, 0, 0);
					}
					else if (BuffTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out BuffTemplate template))
                	{
						manager.AddEntry(row.slot, template, row.state, row.timer, row.level);
					}
					else Debug.LogWarning("Load: Skipped buff " + row.name + " for " + player.name + " as it was not found in Resources.");
				}
            	else Debug.LogWarning("Skipped buff slot " + row.slot + " for " + player.name + " because it's bigger than size " + manager.GetCapacity);
			}
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Buff(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_buff WHERE owner=?", player.name);
	   		
	   		BuffManager manager = player.GetComponent<BuffManager>();
	   		
	   		List<BuffSyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; i++)
	   		{
	   			
	   			BuffSyncStruct entry = list[i];
	   			
	   			connection.InsertOrReplace(new c2w_buff{
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