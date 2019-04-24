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

        public NetworkPlayer Player {
            get {
                if (this.networkPlayers.ContainsKey(this.ClientId)) {
                    return this.networkPlayers[this.ClientId];
                }
                return null;
            }
        }

        private void SetUpEvents() {
            this.Socket.On("onConnect", OnConnect);
            this.Socket.On("disconnect", OnDisconnect);
            this.Socket.On("onOtherPlayerConnect", OnOtherPlayerConnect);
            this.Socket.On("onUpdatePlayer", OnUpdatePlayer);
        }

        // OnConnect to the server, assign the ClientID...
        private void OnConnect(SocketIOEvent e) {
            JSONNode json = JSON.Parse(e.data.ToString());
            this.ClientId = json["id"].Value;
            var player = new NetworkPlayer(json["id"].Value);
            player.DisplayName = json["displayName"].Value;
            this.networkPlayers.Add(this.ClientId, player);
            this.eventBroadcaster.BroadcastEvent("OnConnect");
            Debug.Log("Connected to the Server...");
        }

        // OnOtherPlayerConnect: Update the NetworkPlayer Dictionary...
        private void OnOtherPlayerConnect(SocketIOEvent e) {
            JSONNode json = JSON.Parse(e.data.ToString());
            if(!this.networkPlayers.ContainsKey(json["id"].Value)) {
                var player = new NetworkPlayer(json["id"].Value);
                player.DisplayName = json["displayName"].Value;
                this.networkPlayers.Add(player.Id, player);
            }
        }

        private void OnUpdatePlayer (SocketIOEvent e) {
            JSONNode json = JSON.Parse(e.data.ToString());
            if(this.networkPlayers.ContainsKey(json["id"].Value)) {
                this.networkPlayers[json["id"].Value].DisplayName = json["displayName"].Value;
                this.eventBroadcaster.BroadcastEvent("OnUpdatePlayer", this.networkPlayers[json["id"].Value]);
            }
        }

        private void OnDisconnect(SocketIOEvent e) {
            Debug.Log("Disconnected from the Server...");
        }

        

        public void ChangeDisplayName(string displayName) {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["displayName"] = displayName;
            this.Socket.Emit("changeDisplayName", new JSONObject(data));   
        }

    }
}

