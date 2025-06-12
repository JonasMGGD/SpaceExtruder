using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Enemy {
    public class EnemySpawner : MonoBehaviour {
        [FormerlySerializedAs("spawnInterval")]
        public float baseSpawnInterval = 2f;

        public float minSpawnInterval = 0.5f;
        public float difficultyIncreaseRate = 0.5f;
        public float timeElapsed;
        public float spawnRadius = 15f;
        public int minEnemiesPerWave = 1;
        public int maxEnemiesPerWave = 5;
        public float spawnZOffset = 30f;
        private float _interval = 2f;

        [Range(0f, 1f)] public float heinzSpawnChance = 0.005f;

        private void Start() {
            StartCoroutine(SpawnLoop());
        }

        private void Update() {
            timeElapsed += Time.deltaTime;

            _interval = Mathf.Max(
                baseSpawnInterval - timeElapsed * difficultyIncreaseRate,
                minSpawnInterval
            );
        }

        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator SpawnLoop() {
            while (true) {
                int amount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);

                for (int i = 0; i < amount; i++) {
                    Vector3 spawnPos = new Vector3(
                        Random.Range(-spawnRadius, spawnRadius),
                        1f,
                        Camera.main.transform.position.z + spawnZOffset + Random.Range(-2f, 2f)
                    );

                    string tagToSpawn = (Random.value < heinzSpawnChance) ? "EnemyHeinz" : "Enemy";

                    Quaternion rotation = Quaternion.LookRotation(Vector3.left);
                    ObjectPool.instance.SpawnFromPool(tagToSpawn, spawnPos, rotation);


                    yield return new WaitForSeconds(0.15f);
                }

                yield return new WaitForSeconds(_interval);
            }
        }
    }
}