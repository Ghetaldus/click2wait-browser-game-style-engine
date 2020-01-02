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
		class c2w_currency
		{
			public string 	owner 		{ get; set; }
			public string 	name 		{ get; set; }
			public int		slot		{ get; set; }
			public long 	amount 		{ get; set; }
			public byte 	state 		{ get; set; }
			public long 	timeStamp 	{ get; set; }
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Currency()
		{
	   		connection.CreateTable<c2w_currency>();
        	connection.CreateIndex(nameof(c2w_currency), new []{"owner", "name"});
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Currency(GameObject player)
		{
	   		CurrencyManager manager = player.GetComponent<CurrencyManager>();
	   		manager.CreateDefaultData();
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Currency(GameObject player)
		{
	   		CurrencyManager manager = player.GetComponent<CurrencyManager>();
	   		
			foreach (c2w_currency row in connection.Query<c2w_currency>("SELECT * FROM c2w_currency WHERE owner=?", player.name))
			{
				if (row.slot < manager.GetCapacity)
				{
					if (String.IsNullOrWhiteSpace(row.name))
					{
						manager.AddEntry(row.slot, null, 0, 0, 0);
					}
					else if (CurrencyTemplate.dict.TryGetValue(row.name.GetDeterministicHashCode(), out CurrencyTemplate template))
                	{
						manager.AddEntry(row.slot, template, row.state, row.timeStamp, row.amount);
					}
					else Debug.LogWarning("Load: Skipped currency " + row.name + " for " + player.name + " as it was not found in Resources.");
				}
            	else Debug.LogWarning("Skipped currency slot " + row.slot + " for " + player.name + " because it's bigger than size " + manager.GetCapacity);
			}
		}
	   
		// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Currency(GameObject player)
		{
	   		connection.Execute("DELETE FROM c2w_currency WHERE owner=?", player.name);
	   		
	   		CurrencyManager manager = player.GetComponent<CurrencyManager>();
	   		
	   		List<CurrencySyncStruct> list = manager.GetEntries(false);
	   		
	   		for (int i = 0; i < list.Count; i++)
	   		{
	   			
	   			CurrencySyncStruct entry = list[i];
	   			
	   			connection.InsertOrReplace(new c2w_currency{
                	owner 			= player.name,
                	name 			= entry.name,
                	slot			= entry.slot,
                	amount 			= entry.amount,
                	state			= entry.state,
                	timeStamp 		= entry.timeStamp
            	});
	   		}
		}
		
		// -------------------------------------------------------------------------------

	}

}