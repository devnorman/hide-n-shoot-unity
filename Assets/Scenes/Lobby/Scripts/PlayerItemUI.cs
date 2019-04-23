using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.SocketIO {

    public class PlayerItemUI : MonoBehaviour
    {
        public Text playerName;

        private NetworkPlayer player;

        public void SetData(NetworkPlayer player) {
            this.player = player;
            this.playerName.text = player.Id;
        }

        public void Kick() {
            // KICK PLAYER...
        }
    }
}
