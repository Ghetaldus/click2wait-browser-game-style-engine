// (c) www.click2wait.net

using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using Mirror;
using click2wait;

namespace click2wait
{
    [AddComponentMenu("Network/Authenticators/BasicAuthenticator")]
    public class NetworkAuthenticatorC2W : NetworkAuthenticator
    {
        [Header("Custom Properties")]
		public NetworkManagerC2W manager;
		
        [HideInInspector]public string username;
        [HideInInspector]public string password;
		[HideInInspector]public string action;
		
		[Header("Security")]
    	public string passwordSalt = "at_least_16_byte";
		
		// ===============================================================================
		// 
		// ===============================================================================
		
        public class AuthRequestMessage : MessageBase
        {
            public string authUsername;
            public string authPassword;
            public string authAction;
        }

        public class AuthResponseMessage : MessageBase
        {
            public byte code;
            public string message;
        }
		
		public class LoginMessage : MessageBase
		{
			public string authUsername;
			public string authPassword;
		}
		
		protected string GenerateHash()
		{
			return Tools.PBKDF2Hash(password, passwordSalt + username);
		}
		
		// ===============================================================================
		// 
		// ===============================================================================
		
        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
            NetworkServer.RegisterHandler<LoginMessage>(OnLoginMessage, true);
        }

        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
        }

        public override void OnServerAuthenticate(NetworkConnection conn)
        {
            // do nothing...wait for AuthRequestMessage from client
        }
        
		// -------------------------------------------------------------------------------
        public override void OnClientAuthenticate(NetworkConnection conn)
        {
        
            AuthRequestMessage authRequestMessage = new AuthRequestMessage
            {
                authUsername = username,
                authPassword = GenerateHash(),
                authAction = action
            };

            NetworkClient.Send(authRequestMessage);
        }
        
		// -------------------------------------------------------------------------------
        public void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg)
        {
        	
        	int nErrors = 0;
        	
			AuthResponseMessage authResponseMessage = new AuthResponseMessage
			{
				code = 100,
				message = "Success"
			};
			
			if ((msg.authAction == "REGISTER" || msg.authAction == "BOTH") && Database.singleton.TryRegister(msg.authUsername, msg.authPassword))
                manager.CreatePlayer(msg.authUsername);
            else if (msg.authAction == "REGISTER")
            	nErrors++;
            
            if ((msg.authAction == "LOGIN" || msg.authAction == "BOTH") && !Database.singleton.TryLogin(msg.authUsername, msg.authPassword))
            	nErrors++;
            
            if (nErrors > 0)
            {
            	authResponseMessage.code = 200;
            	authResponseMessage.message = "Invalid Credentials";
            }
            else
            {
            	base.OnServerAuthenticated.Invoke(conn);
            	conn.Send(authResponseMessage);
            }
			
			if (nErrors > 0)
			{
				conn.isAuthenticated = false;
				Invoke(nameof(conn.Disconnect), 1);
			}
			
        }
		
		// -------------------------------------------------------------------------------
        public void OnAuthResponseMessage(NetworkConnection conn, AuthResponseMessage msg)
        {
            if (msg.code == 100)
            {
               	base.OnClientAuthenticated.Invoke(conn);
               	ClientScene.Ready(conn);
               	conn.Send(new LoginMessage{ authUsername = username, authPassword = GenerateHash() });
                //UIModalConfirm.singleton.Show(msg.message);
            }
            else
            {
                conn.isAuthenticated = false;
                conn.Disconnect();
                UIModalConfirm.singleton.Show(msg.message);
            }
        }
        
        // -------------------------------------------------------------------------------
        public void OnLoginMessage(NetworkConnection conn, LoginMessage msg)
        {
        	if (Database.singleton.TryLogin(msg.authUsername, msg.authPassword))
        		manager.LoginPlayer(conn, msg.authUsername);
        }
        
        // -------------------------------------------------------------------------------
               
    }
}
