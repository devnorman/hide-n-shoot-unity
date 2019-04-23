using System.Collections;
using System.Collections.Generic;
using Unity.SocketIO;
using UnityEngine;
using UnityEngine.UI;

namespace HitNShoot {
    public class GameLobby : MonoBehaviour
    {
        [Header("Room Selection")]
        public GameObject gameRoomSelectionUI;
        public Transform roomItemUIList;
        public PoolObject roomItemUIPool;
        public InputField roomNameField;

        [Header("Game Room")]
        public GameObject gameRoomUI;
        public Transform playerItemUIList;
        public PoolObject playerItemUIPool;

        private NetworkRoomManager roomManager;
        private Dictionary<string, RoomItemUI> roomItems;

        void Start () {
            this.roomItems = new Dictionary<string, RoomItemUI>();
            this.roomManager = NetworkRoomManager.Instance;

            this.gameRoomSelectionUI.SetActive(true);
            this.gameRoomUI.SetActive(false);

            this.roomManager.eventBroadcaster.Subscribe("OnCreateRoom", this.gameObject);
            this.roomManager.eventBroadcaster.Subscribe("OnReceiveNewRoom", this.gameObject);
            this.roomManager.eventBroadcaster.Subscribe("OnRefreshRoom", this.gameObject);
            this.roomManager.eventBroadcaster.Subscribe("OnJoinRoom", this.gameObject);
        }

        // TEMPORARY REFRESH ROOMS...
        void Update () {
            if (Input.GetKeyDown(KeyCode.Space)) {
                this.roomManager.RefreshRooms();
            }
        }

        void OnDestroy () {
            this.roomManager.eventBroadcaster.UnSubscribe("OnCreateRoom", this.gameObject);
            this.roomManager.eventBroadcaster.UnSubscribe("OnReceiveNewRoom", this.gameObject);
            this.roomManager.eventBroadcaster.UnSubscribe("OnRefreshRoom", this.gameObject);
            this.roomManager.eventBroadcaster.UnSubscribe("OnJoinRoom", this.gameObject);
        }

        private void OnCreateRoom(NetworkRoom room) {
            this.OnJoinRoom(room);
        }

        private void OnReceiveNewRoom(NetworkRoom room) {
            if (!this.roomItems.ContainsKey(room.Id)) {
                RoomItemUI roomItemUI = this.roomItemUIPool.GetPooledObject(true).GetComponent<RoomItemUI>();
                roomItemUI.SetData(room);
                roomItemUI.transform.SetParent(this.roomItemUIList);
                this.roomItems.Add(room.Id, roomItemUI);
            }
        }

        private void OnRefreshRoom(NetworkRoom room) {
            if (this.roomItems.ContainsKey(room.Id)) {
                this.roomItems[room.Id].SetData(room);
                if (this.roomManager.currentRoom == room) {
                    RefreshGameRoom ();
                } 
            }
        }

        private void RefreshGameRoom() {
            if(this.roomManager.currentRoom != null) {
                this.playerItemUIPool.TogglePooledObjects(false);
                foreach (var playerId in this.roomManager.currentRoom.playerIds) {
                    PlayerItemUI playerItemUI = this.playerItemUIPool.GetPooledObject(true).GetComponent<PlayerItemUI>();
                    playerItemUI.SetData(NetworkClient.Instance.networkPlayers[playerId]);
                    playerItemUI.transform.SetParent(this.playerItemUIList);
                }
            }
        }

        private void OnJoinRoom(NetworkRoom room) {
            Debug.Log("JOINED ROOM!");
            this.gameRoomSelectionUI.SetActive(false);
            this.gameRoomUI.SetActive(true);
            this.RefreshGameRoom();
        }

        public void CreateRoom () {
            this.roomManager.CreateRoom(this.roomNameField.text);
        }

        public void ExitRoom () {
            this.roomItems = new Dictionary<string, RoomItemUI>();
            this.roomItemUIPool.TogglePooledObjects(false);
            this.roomManager.LeaveRoom();
            this.gameRoomSelectionUI.SetActive(true);
            this.gameRoomUI.SetActive(false);
            this.roomManager.RefreshRooms();
        }
    }
}
