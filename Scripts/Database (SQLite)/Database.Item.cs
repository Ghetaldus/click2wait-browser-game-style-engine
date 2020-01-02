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
		class c2w_item
		{
			public string 	owner 	{ get; set; }
			public int		slot	{ get; set; }
			public string 	name 	{ get; set; }
			public long 	amount 	{ get; set; }
			
			public byte 	state 	{ get; set; }
			public double 	timer 	{ get; set; }
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Item()
		{
	   		connection.CreateTable<c2w_item>();
        	connection.CreateIndex(nameof(c2w_item), new []{"owner", "name"});
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Item(GameObject player)
		{
	 		ItemManager manager = player.GetComponent<ItemManager>();
	   		manager.CreateDefaultData();  		
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Item(GameObject player)
		{
	   		ItemManager manager = player.GetComponent<ItemManager>();
	   		
			foreach (c2w_item row in connection.Query<c2w_item>("SELECT * FROM c2w_item WHERE owner=?", player.name))
			{
				if (row.slot < manager.GetCapacity)
				{
					if (String.IsNullOrWhiteSpace(row.name))
					{
						manager.AddEntry(row.slot, null, 0, 0, 0);
					}
					else if (ItemTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out ItemTemplate template))
                	{
						manager.AddEntry(row.slot, template, row.state, row.timer, row.amount);
					}
					else Debug.LogWarning("Load: Skipped item " + row.name + " for " + player.name + " as it was not found in Resources.");
				}
            	else Debug.LogWarning("Skipped item slot " + row.slot + " for " + player.name + " because it's bigger than size " + manager.GetCapacity);
			}
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Item(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_item WHERE owner=?", player.name);
	   		
	   		ItemManager manager = player.GetComponent<ItemManager>();
	   		
	   		List<ItemSyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; i++)
	   		{
	   			
	   			ItemSyncStruct entry = list[i];
	   			
	   			connection.InsertOrReplace(new c2w_item{
                	owner 			= player.name,
                	slot			= entry.slot,
                	name 			= entry.name,
                	amount 			= entry.amount,
                	state 			= entry.state,
                	timer 			= entry.timer
            	});
	   		}
		}
	   
		// -------------------------------------------------------------------------------


	}

}