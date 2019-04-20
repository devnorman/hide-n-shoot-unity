using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;

    public static T Instance { 
        get {
            if (m_instance == null) {
                var obj = new GameObject();
                m_instance = obj.AddComponent<T> ();
                obj.name = typeof(T).ToString();
            }
            return m_instance;
        }
    }

    protected virtual void Awake () {
        if (m_instance == null) {
            m_instance = this.GetComponent<T>();
            DontDestroyOnLoad (this.gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
