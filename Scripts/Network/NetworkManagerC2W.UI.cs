// (c) www.click2wait.net

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif

using click2wait;

public partial class NetworkManagerC2W : NetworkManager
{
   
   
   // -----------------------------------------------------------------------------------
	public bool CanInput()
	{
		return (!isNetworkActive ||
				!IsConnecting());
	}
   
   	// -----------------------------------------------------------------------------------
	public bool CanLoginAccount(string _name, string _password)
	{
		return !isNetworkActive && 
			Tools.IsAllowedName(_name) && 
			Tools.IsAllowedPassword(_password) &&
			!IsConnecting();
	}
	
	// -----------------------------------------------------------------------------------
	public void TryLoginAccount(string _name, string _password)
	{
		((NetworkAuthenticatorC2W)authenticator).action = "LOGIN";
		StartClient();
	}
	
	// -----------------------------------------------------------------------------------
	public bool CanRegisterAccount(string _name, string _password)
	{
		return !isNetworkActive &&
			Tools.IsAllowedName(_name) && 
			Tools.IsAllowedPassword(_password) &&
			!IsConnecting();
	}
	
	// -----------------------------------------------------------------------------------
	public void TryRegisterAccount(string _name, string _password)
	{
		((NetworkAuthenticatorC2W)authenticator).action = "REGISTER";
		StartClient();
	}
	
	// -----------------------------------------------------------------------------------
	public bool CanDeleteAccount(string _name, string _password)
	{
		return false;
	}
	
	// -----------------------------------------------------------------------------------
	public void TryDeleteAccount(string _name, string _password)
	{
	}
	
	// -----------------------------------------------------------------------------------
	public bool CanDedicatedServer()
	{
		return (Application.platform != RuntimePlatform.WebGLPlayer && 
				!isNetworkActive &&
				!IsConnecting());
	}
	
	// -----------------------------------------------------------------------------------
	public void TryDedicatedServer()
	{
		if (!CanDedicatedServer()) return;
		StartServer();
	}
	
	// -----------------------------------------------------------------------------------
	public bool CanCancel()
	{
		return IsConnecting();
	}
	
	// -----------------------------------------------------------------------------------
	public void TryCancel()
	{
		StopClient();
	}
	
	// -----------------------------------------------------------------------------------
	public bool CanHostAndPlay(string _name, string _password)
	{
		return Application.platform != RuntimePlatform.WebGLPlayer && 
			!isNetworkActive && 
			Tools.IsAllowedName(_name) && 
			Tools.IsAllowedPassword(_password) &&
			!IsConnecting();
	}
	
	// -----------------------------------------------------------------------------------
	public void TryHostAndPlay(string _name, string _password)
	{
		((NetworkAuthenticatorC2W)authenticator).action = "BOTH";
		StartHost();
	}
	
	// -----------------------------------------------------------------------------------
	
}
