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
		class c2w_statistic
		{
			public string 	owner 		{ get; set; }
			public string 	name 		{ get; set; }
			public byte	type		{ get; set; }
			public long 	value 		{ get; set; }
		}
		
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("Connect")]
	   	public void Connect_Statistic()
	   	{
	   		connection.CreateTable<c2w_statistic>();
        	connection.CreateIndex(nameof(c2w_statistic), new []{"owner", "name"});
	   	}
	   
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("CreateDefaultData")]
	   	public void CreateDefaultData_Statistic(GameObject player)
	   	{
	   		StatisticManager manager = player.GetComponent<StatisticManager>();
	   		manager.CreateDefaultData();
	   	}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Statistic(GameObject player)
		{
	   		StatisticManager manager = player.GetComponent<StatisticManager>();
	   		
	   		foreach (c2w_statistic row in connection.Query<c2w_statistic>("SELECT * FROM c2w_statistic WHERE owner=?", player.name))
				manager.AddEntry(row.name, row.type, row.value);
			
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Statistic(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_statistic WHERE owner=?", player.name);
	   		
	   		StatisticManager manager = player.GetComponent<StatisticManager>();
	   		
	   		foreach (StatisticSyncStruct entry in manager.GetEntries(false))
	   		{
	   			connection.InsertOrReplace(new c2w_statistic{
                	owner 			= player.name,
                	name 			= entry.name,
                	type 			= entry.type,
                	value 			= entry.value
            	});
	   		}
		}
	   
		// -------------------------------------------------------------------------------
		
	}

}