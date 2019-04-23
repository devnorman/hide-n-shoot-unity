using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.SocketIO {
    public class RoomItemUI : MonoBehaviour
    {
        public Text roomName;
        public Text playerNumber;

        private NetworkRoom room;
        
        public void SetData(NetworkRoom room) {
            this.room = room;
            this.roomName.text = room.Name;
            this.playerNumber.text = room.PlayerCount.ToString();
        }

        public void Join () {
            NetworkRoomManager.Instance.JoinRoom(this.room);
        }
    }
}
