using System.Collections;
using UnityEngine;

namespace PowerUps {
    public class PowerUpSpawner : MonoBehaviour {
        public GameObject[] powerUps;

        public float spawnInterval = 10f;
        public float spawnRadius = 15f;
        public float spawnZOffset = 30f;

        private void Start() {
            StartCoroutine(SpawnLoop());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator SpawnLoop() {
            while (true) {
                yield return new WaitForSeconds(spawnInterval);
                string tag = powerUps[Random.Range(0, powerUps.Length)].name;

                Vector3 spawnPos = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    0f,
                    spawnZOffset + Random.Range(-2f, 2f)
                );

                ObjectPool.Instance.SpawnFromPool(tag, spawnPos, Quaternion.identity);
            }
        }
    }
}