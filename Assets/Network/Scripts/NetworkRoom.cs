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

       public List<string> PlayerIds;

       public string HostPlayerId;

       public int PlayerCount {
           get {
               return this.PlayerIds.Count;
           }
       }

       public int PlayerLimit;

       public NetworkRoom() {}

       public NetworkRoom(string playerId, string name) {
           this.PlayerId = playerId;
           this.Name = name;
           this.PlayerIds = new List<string>();
           this.PlayerIds.Add(playerId);
           this.HostPlayerId = playerId;
       }

       public void SetData(JSONNode data) {
           this.Id = data["id"].Value;
           this.Name = data["name"].Value;
           this.PlayerLimit = int.Parse(data["playerLimit"].Value);
           this.PlayerIds = new List<string>();
           foreach(var item in data["playerIds"].Values) {
               this.PlayerIds.Add(item.Value);
           }
           this.HostPlayerId = data["hostPlayerId"].Value;
       }
    }
}
