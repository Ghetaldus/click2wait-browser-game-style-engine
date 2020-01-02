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

	public partial class Database : MonoBehaviour
	{
		
		public static Database singleton;
		public string databaseFile = "Database.sqlite";
		public SQLiteConnection connection;
		
		// -------------------------------------------------------------------------------
		void Awake()
		{
			if (singleton == null) singleton = this;
		}
		
		// -------------------------------------------------------------------------------
		public void Connect()
		{
#if UNITY_EDITOR
			string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, databaseFile);
#elif UNITY_ANDROID
			string path = Path.Combine(Application.persistentDataPath, databaseFile);
#elif UNITY_IOS
			string path = Path.Combine(Application.persistentDataPath, databaseFile);
#else
			string path = Path.Combine(Application.dataPath, databaseFile);
#endif

			connection = new SQLiteConnection(path);

			this.InvokeInstanceDevExtMethods("Connect");
		}
		
		// -------------------------------------------------------------------------------
		void OnApplicationQuit()
		{
			connection?.Close();
		}
		
		// -------------------------------------------------------------------------------
		public void SavePlayers(IEnumerable<GameObject> players, bool online = true)
    	{
        	connection.BeginTransaction();
        	
        	foreach (GameObject player in players)
            	SaveData(player, online, false);
            	
        	connection.Commit();
    	}
    	
    	// -------------------------------------------------------------------------------
		public void CreateDefaultData(GameObject player)
		{
			this.InvokeInstanceDevExtMethods("CreateDefaultData", player);
		}
		
		// -------------------------------------------------------------------------------
		public GameObject LoadData(GameObject prefab, string _name)
		{
			
			GameObject player = Instantiate(prefab);
			player.name = _name;
			
			LoadData_Level(player); // must be called first
			
			this.InvokeInstanceDevExtMethods("LoadData", player);
			
			return player;
			
		}
		
		// -------------------------------------------------------------------------------
		public void SaveData(GameObject player, bool online, bool useTransaction = true)
		{
			if (useTransaction)
				connection.BeginTransaction();
				
			this.InvokeInstanceDevExtMethods("SaveData", player);
			
			if (useTransaction)
				connection.Commit();
		}
		
		// -----------------------------------------------------------------------------------

	}

}