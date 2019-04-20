using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroadcaster : MonoBehaviour
{
   public Dictionary<string, List<GameObject>> subscribers = new Dictionary<string, List<GameObject>>();

   public void BroadcastEvent(string e)
   {
      if (this.subscribers.ContainsKey(e))
      {
         foreach (var item in this.subscribers[e])
         {
            if (item != null)
            {
               item.SendMessage(e, SendMessageOptions.DontRequireReceiver);
            }
         }
      }
   }

   public void BroadcastEvent(string e, object obj)
   {
      if (this.subscribers.ContainsKey(e))
      {
         foreach (var item in this.subscribers[e])
         {
            if (item != null)
            {
               item.SendMessage(e, obj, SendMessageOptions.DontRequireReceiver);
            }
         }
      }
   }

   public void Subscribe(string e, GameObject obj)
   {
      if (!this.subscribers.ContainsKey(e))
      {
         this.subscribers.Add(e, new List<GameObject>());
      }
      this.subscribers[e].Add(obj);
      this.subscribers[e].RemoveAll(item => item == null);
   }

   public void UnSubscribe(string e, GameObject obj)
   {
      if (this.subscribers.ContainsKey(e))
      {
         this.subscribers[e].Remove(obj);
      }
      this.subscribers[e].RemoveAll(item => item == null);
   }
}
