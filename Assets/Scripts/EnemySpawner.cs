using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public float spawnInterval = 2f;
    public float spawnRadius = 15f;
    public int minEnemiesPerWave = 1;
    public int maxEnemiesPerWave = 5;
    public float spawnZOffset = 30f;

    [Range(0f, 1f)] public float heinzSpawnChance = 0.05f;

    private void Start() {
        StartCoroutine(SpawnLoop());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator SpawnLoop() {
        while (true) {
            int amount = Random.Range(minEnemiesPerWave, maxEnemiesPerWave + 1);

            for (int i = 0; i < amount; i++) {
                Vector3 spawnPos = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    0f,
                    Camera.main.transform.position.z + spawnZOffset + Random.Range(-2f, 2f)
                );

                string tagToSpawn = (Random.value < heinzSpawnChance) ? "EnemyHeinz" : "Enemy";

                GameObject enemy = ObjectPool.Instance.SpawnFromPool(tagToSpawn, spawnPos, Quaternion.identity);

                yield return new WaitForSeconds(0.15f);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}