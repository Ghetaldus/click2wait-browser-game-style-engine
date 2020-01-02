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

[RequireComponent(typeof(Database))]
public partial class NetworkManagerC2W : NetworkManager
{

    public NetworkState state = NetworkState.Offline;

    [Header("Database")]
    public float saveInterval = 60f;
	
	// ===============================================================================
	// 
	// ===============================================================================
	
	public partial class ErrorMsg : MessageBase
	{
   		public string text;
   		public bool causesDisconnect;
	}
		
	// ===============================================================================
	// 
	// ===============================================================================
    
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (ClientScene.localPlayer != null)
            state = NetworkState.Game;
    }

	public bool AccountLoggedIn(string _name)
    {
        return AccountManager.onlinePlayers.Values.Any(x => x.name == _name);
    }
    
    public void ServerSendError(NetworkConnection conn, string error, bool disconnect)
    {
        conn.Send(new ErrorMsg{text=error, causesDisconnect=disconnect});
    }

    void OnClientError(NetworkConnection conn, ErrorMsg message)
    {
        
		UIModalConfirm.singleton.Show(message.text);
		
        if (message.causesDisconnect)
        {
            conn.Disconnect();
            if (NetworkServer.active) StopHost();
        }
    }

    public override void OnStartClient() {}

    public override void OnStartServer()
    {
        Database.singleton.Connect();
        InvokeRepeating(nameof(SavePlayers), saveInterval, saveInterval);
    }

    public override void OnStopServer()
    {
        CancelInvoke(nameof(SavePlayers));
    }

    public bool IsConnecting() => NetworkClient.active && !ClientScene.ready;

    public override void OnClientConnect(NetworkConnection conn) {}

    public override void OnServerConnect(NetworkConnection conn) {}

    public override void OnClientSceneChanged(NetworkConnection conn) {}

    public void CreatePlayer(string _name)
    {
        GameObject player = Instantiate(playerPrefab);
        player.name = _name;
        Database.singleton.CreateDefaultData(player);
        Database.singleton.SaveData(player, false);
		Destroy(player);
    }
    
	public void LoginPlayer(NetworkConnection conn, string _name)
    {
        if (!AccountLoggedIn(_name))
        {
            GameObject player = Database.singleton.LoadData(playerPrefab, _name);
            NetworkServer.AddPlayerForConnection(conn, player);
            state = NetworkState.Game;
        }
        else
            ServerSendError(conn, "Login: account already online", true);
    }
    
    public override void OnServerAddPlayer(NetworkConnection conn) {}
    
    void SavePlayers()
    {
        Database.singleton.SavePlayers(AccountManager.onlinePlayers.Values);
        if (AccountManager.onlinePlayers.Count > 0) Debug.Log("saved " + AccountManager.onlinePlayers.Count + " player(s)");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        float delay = 1;
        StartCoroutine(DoServerDisconnect(conn, delay));
    }

    IEnumerator<WaitForSeconds> DoServerDisconnect(NetworkConnection conn, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (conn.identity != null)
        {
            Database.singleton.SaveData(conn.identity.gameObject, false);
            print("saved:" + conn.identity.name);
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
    	UIModalConfirm.singleton.Show("Disconnected.");
        base.OnClientDisconnect(conn);
        state = NetworkState.Offline;
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
