using UnityEngine;

namespace StartScene {
    public class FloatingEnemySpawner : MonoBehaviour {
        public float spawnInterval = 3f;
        public Vector2 xSpawnRange = new(-40f, 40f);
        public float yStart = -20f;
        public float zPlane;

        private float _timer;

        private void Update() {
            _timer -= Time.deltaTime;

            if (!(_timer <= 0f)) return;
            Spawn();
            _timer = spawnInterval + Random.Range(-1f, 1f);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Spawn() {
            var spawnPos = new Vector3(Random.Range(xSpawnRange.x, xSpawnRange.y), yStart, zPlane);
            ObjectPool.instance.SpawnFromPool("Enemy", spawnPos, Quaternion.Euler(180f, 90f, 90f));
        }
    }
}