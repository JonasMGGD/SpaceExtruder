using UnityEngine;

namespace StartScene {
    public class FloatingEnemySpawner : MonoBehaviour {
        public float spawnInterval = 3f;
        public Vector2 xSpawnRange = new(-40f, 40f);
        public float yStart = -20f;
        public float zPlane = 0f;

        private float timer;

        private void Update() {
            timer -= Time.deltaTime;

            if (timer <= 0f) {
                Spawn();
                timer = spawnInterval + Random.Range(-1f, 1f);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void Spawn() {
            Vector3 spawnPos = new Vector3(Random.Range(xSpawnRange.x, xSpawnRange.y), yStart, zPlane);
            ObjectPool.Instance.SpawnFromPool("Enemy", spawnPos, Quaternion.Euler(180f, 90f, 90f));
        }
    }
}