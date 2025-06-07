using UnityEngine;
using UnityEngine.Serialization;

namespace PowerUps {
    public class PowerUpSpawner : MonoBehaviour {
        public string[] powerUps = {"Heal", "RapidFire", "ScoreBoost", "Invincibility"};

        public float spawnInterval = 10f;
        public Vector2 spawnXRange;
        public float spawnZ;

        private void Start() {
            Debug.Log(powerUps[1]);
            InvokeRepeating(nameof(Spawn), spawnInterval, spawnInterval);
        }

        void Spawn() {
            int index = Random.Range(0, powerUps.Length - 1);
            Vector3 pos = new Vector3(Random.Range(spawnXRange.x, spawnXRange.y), 0f, spawnZ);
            ObjectPool.Instance.SpawnFromPool(powerUps[index], pos, Quaternion.identity);
        }
    }
}