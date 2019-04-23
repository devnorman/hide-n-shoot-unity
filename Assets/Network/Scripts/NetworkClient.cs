using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;
using SimpleJSON;

namespace Unity.SocketIO
{
    // Network Client handles Server infos like player network IDs...
    // Source of truth of the SocketIO networking...
    [RequireComponent(typeof(SocketIOComponent))]
    [RequireComponent(typeof(EventBroadcaster))]
    public class NetworkClient : Singleton<NetworkClient>
    {
        public string ClientId { get; private set; }
        public SocketIOComponent Socket { get; private set; }
        public EventBroadcaster eventBroadcaster { get; private set; }

        public Dictionary<string, NetworkPlayer> networkPlayers { get; private set; }

        protected override void Awake() {
            base.Awake();
            this.Socket = GetComponent<SocketIOComponent>();
            this.eventBroadcaster = GetComponent<EventBroadcaster>();
        }

        void Start () {
            this.networkPlayers = new Dictionary<string, NetworkPlayer>();
            this.SetUpEvents();
        }

        private void SetUpEvents() {
            this.Socket.On("onConnect", OnConnect);
            this.Socket.On("disconnect", OnDisconnect);
            this.Socket.On("onOtherPlayerConnect", OnOtherPlayerConnect);
        }

        // OnConnect to the server, assign the ClientID...
        private void OnConnect(SocketIOEvent e) {
            JSONNode json = JSON.Parse(e.data.ToString());
            this.ClientId = json["id"].Value;
            this.networkPlayers.Add(this.ClientId, new NetworkPlayer(json["id"].Value));
            this.eventBroadcaster.BroadcastEvent("OnConnect");
            Debug.Log("Connected to the Server...");
        }

        // OnOtherPlayerConnect: Update the NetworkPlayer Dictionary...
        private void OnOtherPlayerConnect(SocketIOEvent e) {
            JSONNode json = JSON.Parse(e.data.ToString());
            if(!this.networkPlayers.ContainsKey(json["id"].Value)) {
                this.networkPlayers.Add(json["id"].Value, new NetworkPlayer(json["id"].Value));
            }
        }

        private void OnDisconnect(SocketIOEvent e) {
            Debug.Log("Disconnected from the Server...");
        }

    }
}

