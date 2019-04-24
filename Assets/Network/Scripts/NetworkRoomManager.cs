using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using SocketIO;
using UnityEngine;

namespace Unity.SocketIO
{
    [RequireComponent(typeof(EventBroadcaster))]
    public class NetworkRoomManager : Singleton<NetworkRoomManager>
    {
        public NetworkRoom currentRoom { get; private set; }

        public Dictionary<string, NetworkRoom> networkRooms { get; private set; }

        public EventBroadcaster eventBroadcaster { get; private set; }

        private NetworkClient networkClient;
        
        protected override void Awake() {
            base.Awake();
            this.eventBroadcaster = GetComponent<EventBroadcaster>();
        }

        void Start()
        {
            this.networkClient = NetworkClient.Instance;
            this.networkRooms = new Dictionary<string, NetworkRoom>();
            this.SetUpEvents();
        }

        private void SetUpEvents () {
            this.networkClient.Socket.On("onCreateRoom", OnCreateRoom);
            this.networkClient.Socket.On("onReceiveNewRoom", OnReceiveNewRoom);
            this.networkClient.Socket.On("onJoinRoom", OnJoinRoom);
            this.networkClient.Socket.On("onRefreshRoom", OnRefreshRoom);
        }

        private void OnDestroy () {
            this.networkClient.Socket.Off("onCreateRoom", OnCreateRoom);
            this.networkClient.Socket.Off("onReceiveNewRoom", OnReceiveNewRoom);
            this.networkClient.Socket.Off("onJoinRoom", OnJoinRoom);
            this.networkClient.Socket.Off("onRefreshRoom", OnRefreshRoom);
        }

        public void RefreshRooms() {
            this.networkRooms = new Dictionary<string, NetworkRoom>();
            this.networkClient.Socket.Emit("refreshRooms");
        }

        public void CreateRoom(string roomName) {
            var newRoom = new NetworkRoom(this.networkClient.ClientId, roomName);
            this.networkClient.Socket.Emit("createRoom", new JSONObject(JsonUtility.ToJson(newRoom)));
        }

        public void JoinRoom(NetworkRoom room) {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["roomId"] = room.Id;
            data["playerId"] = this.networkClient.ClientId;
            this.networkClient.Socket.Emit("joinRoom", new JSONObject(data));
        }

        public void LeaveRoom() {
            if (this.currentRoom != null) {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data["roomId"] = this.currentRoom.Id;
                data["playerId"] = this.networkClient.ClientId;
                this.networkClient.Socket.Emit("leaveRoom", new JSONObject(data));
            }
            this.currentRoom = null;
        }

        private void OnCreateRoom(SocketIOEvent e) {
            this.OnReceiveNewRoom(e);
            JSONNode json = JSON.Parse(e.data.ToString());
            if (this.networkRooms.ContainsKey(json["id"].Value)) {
                this.networkRooms[json["id"].Value].SetData(json);
                this.currentRoom = this.networkRooms[json["id"].Value];
                this.eventBroadcaster.BroadcastEvent("OnCreateRoom", this.currentRoom);
            }
        }

        // OnReceive new Room data, stored it locally...
        private void OnReceiveNewRoom(SocketIOEvent e) {
            JSONNode json = JSON.Parse(e.data.ToString());
            if (!this.networkRooms.ContainsKey(json["id"].Value)) {
                var newRoom = new NetworkRoom();
                newRoom.SetData(json);
                this.networkRooms.Add(newRoom.Id, newRoom);
                this.eventBroadcaster.BroadcastEvent("OnReceiveNewRoom", newRoom);
            }
        }

        // Monitor Change of a specific Room...
        private void OnRefreshRoom(SocketIOEvent e) {
            Debug.Log(e.data.ToString());
            JSONNode json = JSON.Parse(e.data.ToString());
            if (this.networkRooms.ContainsKey(json["id"].Value)) {
                var room = this.networkRooms[json["id"].Value];
                room.SetData(json);
                if (room.PlayerCount > 0) {
                    this.eventBroadcaster.BroadcastEvent("OnRefreshRoom", room);
                } else {
                    this.networkRooms.Remove(room.Id);
                    this.eventBroadcaster.BroadcastEvent ("OnDeleteRoom", room);
                }
            }
        }

        // OnJoin to a specific Room, set client current room...
        private void OnJoinRoom(SocketIOEvent e) {
            JSONNode json = JSON.Parse(e.data.ToString());
            if (this.networkRooms.ContainsKey(json["id"].Value)) {
                this.networkRooms[json["id"].Value].SetData(json);
                this.currentRoom = this.networkRooms[json["id"].Value];
                this.eventBroadcaster.BroadcastEvent("OnJoinRoom", this.currentRoom);
            }
        }
    }
}
