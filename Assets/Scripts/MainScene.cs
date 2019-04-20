using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.SocketIO;
    
public class MainScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(1);
    }
}
