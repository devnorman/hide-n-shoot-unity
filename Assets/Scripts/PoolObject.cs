using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool = 10;
    public bool isScalable = false;

    private List<GameObject> pooledObjects;

    void Start () {
        this.pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++) {
            GameObject obj = (GameObject)Instantiate(this.objectToPool);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            this.pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject() {
        return this.GetPooledObject(false);
    }

    public GameObject GetPooledObject(bool value) {
        for (int i = 0; i < this.pooledObjects.Count; i++) {
            if (!this.pooledObjects[i].activeInHierarchy) {
                this.pooledObjects[i].SetActive(value);
                return pooledObjects[i];
            }
        }

        if(this.isScalable) {
            GameObject obj = (GameObject)Instantiate(this.objectToPool);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
            this.pooledObjects.Add(obj);
            obj.SetActive(value);
            return obj;
        }
        return null;
    }

    public void TogglePooledObjects(bool value) {
        for (int i = 0; i < this.pooledObjects.Count; i++) {
            this.pooledObjects[i].SetActive(value);
        }
    }
}
