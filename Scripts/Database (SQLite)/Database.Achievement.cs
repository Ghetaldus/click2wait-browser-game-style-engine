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
		class c2w_achievement
		{
			public string 	owner 		{ get; set; }
			public int		slot		{ get; set; }
			public string 	name 		{ get; set; }
			public long 	value 		{ get; set; }
		}
		
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("Connect")]
	   	public void Connect_Achievement()
	   	{
	   		connection.CreateTable<c2w_achievement>();
        	connection.CreateIndex(nameof(c2w_achievement), new []{"owner", "name"});
	   	}
	   
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("CreateDefaultData")]
	   	public void CreateDefaultData_Achievement(GameObject player)
	   	{
	   		AchievementManager manager = player.GetComponent<AchievementManager>();
	   		manager.CreateDefaultData();
	   	}
	   
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("LoadData")]
	   	public void LoadData_Achievement(GameObject player)
	   	{
	   		AchievementManager manager = player.GetComponent<AchievementManager>();
	   		
			foreach (c2w_achievement row in connection.Query<c2w_achievement>("SELECT * FROM c2w_achievement WHERE owner=?", player.name))
			{
			
				if (String.IsNullOrWhiteSpace(row.name))
					continue;
					
				if (AchievementTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out AchievementTemplate template))
                {
					manager.AddEntry(row.slot, template, row.value);
				}
				else Debug.LogWarning("Load: Skipped achievement " + row.name + " for " + player.name + " as it was not found in Resources.");
			}
		}
	   
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("SaveData")]
	   	public void SaveData_Achievement(GameObject player)
	   	{
	   		connection.Execute("DELETE FROM c2w_achievement WHERE owner=?", player.name);
	   		
	   		AchievementManager manager = player.GetComponent<AchievementManager>();
	   		
	   		foreach (AchievementSyncStruct entry in manager.GetEntries())
	   		{
	   			connection.InsertOrReplace(new c2w_achievement{
                	owner 			= player.name,
                	slot			= entry.slot,
                	name 			= entry.name,
                	value 			= entry.value
            	});
	   		}
	   	}
	   
		// -------------------------------------------------------------------------------

	}

}