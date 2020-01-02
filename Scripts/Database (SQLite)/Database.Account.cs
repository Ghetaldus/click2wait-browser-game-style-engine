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
		class c2w_account
		{
			[PrimaryKey]
			public string name 			{ get; set; }
			public string password 		{ get; set; }
			public DateTime created 	{ get; set; }
			public DateTime lastlogin 	{ get; set; }
			public bool deleted 		{ get; set; }
			public bool banned 			{ get; set; }
			public bool online 			{ get; set; }
        	public DateTime lastsaved 	{ get; set; }
		}
		
		// -------------------------------------------------------------------------------
		[DevExtMethods("Connect")]
		public void Connect_Account()
		{
	   		connection.CreateTable<c2w_account>();
		}
		
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods("CreateDefaultData")]
		public void CreateDefaultData_Account(GameObject player)
		{
	   
		}
		
	   	// -------------------------------------------------------------------------------
		[DevExtMethods("LoadData")]
		public void LoadData_Account(GameObject player)
		{
	   	
		}
		
	   	// -------------------------------------------------------------------------------
		[DevExtMethods("SaveData")]
		public void SaveData_Account(GameObject player)
		{
	   
		}
		
		// -------------------------------------------------------------------------------
		public bool TryLogin(string _name, string _password)
		{
		
			if (!string.IsNullOrWhiteSpace(_name) && !string.IsNullOrWhiteSpace(_password))
			{
				
				if (!AccountExists(_name))
					return false;

				if (AccountValid(_name, _password))
				{
					AccountSetOnline(_name);
					return true;
				}
			}
			return false;
		}
		
		// -------------------------------------------------------------------------------
		public bool TryRegister(string _name, string _password)
		{
		
			if (!string.IsNullOrWhiteSpace(_name) && !string.IsNullOrWhiteSpace(_password))
			{
				
				if (AccountExists(_name))
					return false;

				AccountCreate(_name, _password);
				AccountSetOnline(_name);
				return true;
				
			}
			return false;
		}
		
		// -------------------------------------------------------------------------------
		public bool TryDelete(string _name, string _password)
		{
		
			if (!string.IsNullOrWhiteSpace(_name) && !string.IsNullOrWhiteSpace(_password))
			{
				
				if (!AccountExists(_name))
					return false;

				if (AccountValid(_name, _password))
				{
					AccountDelete(_name);
					return true;	
				}
			}
			return false;
		
		}
		
		// -------------------------------------------------------------------------------
		public bool AccountValidate(string _name, string _password)
		{
			
			if (AccountValid(_name, _password))
				return true;
				
			return false;
		}
		
		// -------------------------------------------------------------------------------
		public void AccountCreate(string _name, string _password)
		{
			connection.Insert(new c2w_account{ name=_name, password=_password, created=DateTime.UtcNow, lastlogin=DateTime.Now, banned=false});
		}
		
		// -------------------------------------------------------------------------------
		public bool AccountValid(string _name, string _password)
		{
			return connection.FindWithQuery<c2w_account>("SELECT * FROM c2w_account WHERE name=? AND password=? and banned=0", _name, _password) != null;
		}
		
		// -------------------------------------------------------------------------------
		public bool AccountExists(string _name)
		{
			return connection.FindWithQuery<c2w_account>("SELECT * FROM c2w_account WHERE name=?", _name) != null;
		}
		
		// -------------------------------------------------------------------------------
		public void AccountSetOnline(string _name)
		{
			connection.Execute("UPDATE c2w_account SET lastlogin=? WHERE name=?", DateTime.UtcNow, _name);
		}
		
		// -------------------------------------------------------------------------------
		public void AccountDelete(string _name)
		{
			connection.Execute("UPDATE c2w_account SET deleted=1 WHERE name=?", _name);
		}
		
		// -------------------------------------------------------------------------------
		
	}

}