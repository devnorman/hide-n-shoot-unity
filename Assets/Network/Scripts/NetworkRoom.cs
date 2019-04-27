using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Unity.SocketIO {

    public class NetworkRoom 
    {
       public string Id;

       public string PlayerId;

       public string Name;

       public List<string> playerIds;

       public string hostPlayerId;

       public int PlayerCount {
           get {
               return this.playerIds.Count;
           }
       }

       public NetworkRoom() {}

       public NetworkRoom(string playerId, string name) {
           this.PlayerId = playerId;
           this.Name = name;
           this.playerIds = new List<string>();
           this.playerIds.Add(playerId);
           this.hostPlayerId = playerId;
       }

       public void SetData(JSONNode data) {
           this.Id = data["id"].Value;
           this.Name = data["name"].Value;
           this.playerIds = new List<string>();
           foreach(var item in data["playerIds"].Values) {
               this.playerIds.Add(item.Value);
           }
           this.hostPlayerId = data["hostPlayerId"].Value;
       }
    }
}
