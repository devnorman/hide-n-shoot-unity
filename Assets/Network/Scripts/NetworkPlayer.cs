using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Unity.SocketIO
{
   public class NetworkPlayer
   {
      public string Name { get; set; }
      public string Id { get; private set; }

      public NetworkPlayer(string id) {
         this.Id = id;
      }
   }
}