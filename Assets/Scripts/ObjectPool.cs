using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake() {
        if (!Instance)
            Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (poolDictionary[tag].Count == 0) {
            GameObject newObj = Instantiate(pools.Find(p => p.tag == tag).prefab);
            return newObj;
        }
        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    public void ReturnToPool(string tag, GameObject obj) {
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}