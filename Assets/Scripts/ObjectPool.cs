using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool instance;

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Awake() {
        if (!instance)
            instance = this;

        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools) {
            var objectPool = new Queue<GameObject>();

            for (var i = 0; i < pool.size; i++) {
                var obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if (!_poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (_poolDictionary[tag].Count == 0) {
            var newObj = Instantiate(pools.Find(p => p.tag == tag).prefab);
            return newObj;
        }
        var obj = _poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    public void ReturnToPool(string tag, GameObject obj) {
        obj.SetActive(false);
        _poolDictionary[tag].Enqueue(obj);
    }
}