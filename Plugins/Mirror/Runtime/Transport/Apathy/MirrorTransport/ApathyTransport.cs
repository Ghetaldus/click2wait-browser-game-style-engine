// uses Apathy for both client & server
#if MIRROR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class ApathyTransport : Transport
    {
        public ushort port = 7777;

        [Tooltip("Nagle Algorithm can be disabled by enabling NoDelay")]
        public bool NoDelay = true;

        [Tooltip("Protect against allocation attacks by keeping the max message size small. Otherwise an attacker might send multiple fake packets with 2GB headers, causing the server to run out of memory after allocating multiple large packets.")]
        public int serverMaxMessageSize = 16 * 1024;

        [Tooltip("Protect against allocation attacks by keeping the max message size small. Otherwise an attacker host might send multiple fake packets with 2GB headers, causing the connected clients to run out of memory after allocating multiple large packets.")]
        public int clientMaxMessageSize = 16 * 1024;

        [Tooltip("Client tick rate is often higher than server tick rate, especially if server is under heavy load or limited to 20Hz or similar. Server needs to process 'a few' messages per tick per connection. Processing only one per tick can cause an ever growing backlog, hence ever growing latency on the client. Set this to a reasonable amount, but not too big so that the server never deadlocks reading too many messages per tick (which would be way worse than one of the clients having high latency.")]
        public int MaxReceivesPerTickPerConnection = 10;

        protected Apathy.Client client = new Apathy.Client();
        protected Apathy.Server server = new Apathy.Server();

        // cache GetNextMessages queue to avoid allocations
        // -> with capacity to avoid rescaling as long as possible!
        Queue<Apathy.Message> queue = new Queue<Apathy.Message>(1000);

        void Awake()
        {
            // tell Telepathy to use Unity's Debug.Log
            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;

            // configure
            client.NoDelay = NoDelay;
            client.MaxMessageSize = clientMaxMessageSize;
            client.MaxReceivesPerTickPerConnection = MaxReceivesPerTickPerConnection;
            server.NoDelay = NoDelay;
            server.MaxMessageSize = serverMaxMessageSize;
            server.MaxReceivesPerTickPerConnection = MaxReceivesPerTickPerConnection;

            Debug.Log("ApathyTransport initialized!");
        }

        public override bool Available()
        {
            return Application.platform == RuntimePlatform.OSXEditor ||
                   Application.platform == RuntimePlatform.OSXPlayer ||
                   Application.platform == RuntimePlatform.WindowsEditor ||
                   Application.platform == RuntimePlatform.WindowsPlayer ||
                   Application.platform == RuntimePlatform.LinuxEditor ||
                   Application.platform == RuntimePlatform.LinuxPlayer;
        }

        // client
        public override bool ClientConnected() => client.Connected;
        public override void ClientConnect(string address) => client.Connect(address, port);
        public override bool ClientSend(int channelId, ArraySegment<byte> segment) => client.Send(segment);
        void ProcessClientMessages()
        {
            client.GetNextMessages(queue);
            while (queue.Count > 0)
            {
                Apathy.Message message = queue.Dequeue();
                switch (message.eventType)
                {
                    case Apathy.EventType.Connected:
                        OnClientConnected.Invoke();
                        break; // breaks switch, not while
                    case Apathy.EventType.Data:
                        OnClientDataReceived.Invoke(message.data, Channels.DefaultReliable);
                        break; // breaks switch, not while
                    case Apathy.EventType.Disconnected:
                        OnClientDisconnected.Invoke();
                        break; // breaks switch, not while
                }
            }
        }
        public override void ClientDisconnect() => client.Disconnect();

        // IMPORTANT: set script execution order to >1000 to call Transport's
        //            LateUpdate after all others. Fixes race condition where
        //            e.g. in uSurvival Transport would apply Cmds before
        //            ShoulderRotation.LateUpdate, resulting in projectile
        //            spawns at the point before shoulder rotation.
        public void LateUpdate()
        {
            // note: we need to check enabled in case we set it to false
            // when LateUpdate already started.
            // (https://github.com/vis2k/Mirror/pull/379)
            if (enabled) ProcessClientMessages();
            if (enabled) ProcessServerMessages();
        }

        // server
        public override bool ServerActive() => server.Active;
        public override void ServerStart() => server.Start(port);
        public override bool ServerSend(List<int> connectionIds, int channelId, ArraySegment<byte> segment)
        {
            // send to all
            bool result = true;
            foreach (int connectionId in connectionIds)
                result &= server.Send(connectionId, segment);
            return result;
        }
        public void ProcessServerMessages()
        {
            if (server.Active)
            {
                server.GetNextMessages(queue);
                while (queue.Count > 0)
                {
                    Apathy.Message message = queue.Dequeue();
                    switch (message.eventType)
                    {
                        case Apathy.EventType.Connected:
                            OnServerConnected.Invoke(message.connectionId);
                            break; // breaks switch, not while
                        case Apathy.EventType.Data:
                            OnServerDataReceived.Invoke(message.connectionId, message.data, Channels.DefaultReliable);
                            break; // breaks switch, not while
                        case Apathy.EventType.Disconnected:
                            OnServerDisconnected.Invoke(message.connectionId);
                            break; // breaks switch, not while
                    }
                }
            }
        }
        public override bool ServerDisconnect(int connectionId) => server.Disconnect(connectionId);
        public override string ServerGetClientAddress(int connectionId) => server.GetClientAddress(connectionId);
        public override void ServerStop() => server.Stop();

        // common
        public override void Shutdown()
        {
            Debug.Log("ApathyTransport Shutdown()");
            client.Disconnect();
            server.Stop();
        }

        public override int GetMaxPacketSize(int channelId)
        {
            return serverMaxMessageSize;
        }

        public override string ToString()
        {
            if (server.Active)
            {
                return "Apathy Server port: " + port;
            }
            else if (client.Connecting || client.Connected)
            {
                return "Apathy Client port: " + port;
            }
            return "Apathy (inactive/disconnected)";
        }
    }
}
#endif
