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
		class c2w_level
		{
			public string 	owner 		{ get; set; }
			public string 	name 		{ get; set; }
			public int 		level 		{ get; set; }
			// PRIMARY KEY (owner, name) created manually.
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Level()
		{
			connection.CreateTable<c2w_level>();
        	connection.CreateIndex(nameof(c2w_level), new []{"owner", "name"});
		}
		
		// -------------------------------------------------------------------------------
		//
		// Executed manually from "Database.cs" because it has to run before all other
		// data is loaded to ensure that all manager levels are up to date.
		//
		// -------------------------------------------------------------------------------
		//[DevExtMethods("LoadData")]
		public void LoadData_Level(GameObject player)
		{
			
			Component[] components = player.GetComponents<BaseManager>();
			
	   		foreach (c2w_level row in connection.Query<c2w_level>("SELECT * FROM c2w_level WHERE owner=?", player.name))
			{
				foreach (Component component in components)
	   			{
	   				if (component is BaseManager)
	   				{
	   				
	   					BaseManager manager = (BaseManager)component;
	   				
	   					if (manager.GetType().ToString() == row.name)
	   						manager.level = row.level;
	   					
	   				}
	   			}
			}
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Level(GameObject player)
		{

	   		connection.Execute("DELETE FROM c2w_level WHERE owner=?", player.name);
	   		
	   		Component[] components = player.GetComponents<BaseManager>();
	   		
	   		foreach (Component component in components)
	   		{
	   			if (component is BaseManager)
	   			{
	   			
	   				BaseManager manager = (BaseManager)component;
	   			
	   				connection.InsertOrReplace(new c2w_level{
                		owner 			= player.name,
                		name 			= manager.GetType().ToString(),
                		level 			= manager.level
            		});
            	
            	}
	   		}
		
		}
		
		// -------------------------------------------------------------------------------
		
	}

}