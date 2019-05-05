using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.SocketIO {

    public class PlayerItemUI : MonoBehaviour
    {
        public Text playerName;
        public GameObject hostTag; 

        private NetworkPlayer player;

        public void SetData(NetworkPlayer player) {
            this.player = player;
            this.playerName.text = player.DisplayName;
            this.hostTag.SetActive(
                NetworkRoomManager.Instance.currentRoom.HostPlayerId == player.Id
            );
        }

        public void Kick() {
            // KICK PLAYER...
        }
    }
}
