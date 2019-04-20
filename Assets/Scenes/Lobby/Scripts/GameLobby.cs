using System.Collections;
using System.Collections.Generic;
using Unity.SocketIO;
using UnityEngine;
using UnityEngine.UI;

namespace HitNShoot {
    public class GameLobby : MonoBehaviour
    {
        public PoolObject roomItemUIPool;
        public Transform roomItemUIList;

        public InputField roomNameField;

        private NetworkRoomManager roomManager;
        private Dictionary<string, RoomItemUI> roomItems;

        void Start () {
            this.roomItems = new Dictionary<string, RoomItemUI>();
            this.roomManager = NetworkRoomManager.Instance;
            this.roomManager.eventBroadcaster.Subscribe("OnReceiveNewRoom", this.gameObject);
            this.roomManager.eventBroadcaster.Subscribe("OnRefreshRoom", this.gameObject);
        }

        void OnDestroy () {
            this.roomManager.eventBroadcaster.UnSubscribe("OnReceiveNewRoom", this.gameObject);
            this.roomManager.eventBroadcaster.UnSubscribe("OnRefreshRoom", this.gameObject);
        }

        private void OnReceiveNewRoom(NetworkRoom room) {
            RoomItemUI roomItemUI = this.roomItemUIPool.GetPooledObject(true).GetComponent<RoomItemUI>();
            roomItemUI.SetData(room);
            roomItemUI.transform.SetParent(this.roomItemUIList);
            this.roomItems.Add(room.Id, roomItemUI);
        }

        private void OnRefreshRoom(NetworkRoom room) {
            if (this.roomItems.ContainsKey(room.Id)) {
                this.roomItems[room.Id].SetData(room);
            }
        }

        private void OnJoinRoom(NetworkRoom room) {
            Debug.Log("JOINED ROOM!");
        }

        public void CreateRoom () {
            this.roomManager.CreateRoom(this.roomNameField.text);
        }
    }
}
