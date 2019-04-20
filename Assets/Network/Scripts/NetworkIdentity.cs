using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.SocketIO {
    public class NetworkIdentity : MonoBehaviour
    {
        public string id;

        public void Initialize(string id) {
            this.id = id;
        }
    }
}
