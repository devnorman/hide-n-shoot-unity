using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.SocketIO {
    public class RoomItemUI : MonoBehaviour
    {
        public Text roomName;
        public Text playerNumber;
        public Text playerLimit;

        private NetworkRoom room;
        
        public void SetData(NetworkRoom room) {
            this.room = room;
            this.roomName.text = room.Name;
            this.playerNumber.text = room.PlayerCount.ToString();
            this.playerLimit.text = room.PlayerLimit.ToString();
        }

        public void Join () {
            NetworkRoomManager.Instance.JoinRoom(this.room);
        }
    }
}
