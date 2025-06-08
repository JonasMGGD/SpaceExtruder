using System.Collections;
using UnityEngine;

namespace Floor {
    public class FloorDecorationSpawner : MonoBehaviour {
        public float spawnInterval = 1.5f;
        public Vector2 xRange = new(-70f, -55f);
        public Vector2 xRangeRight = new(55f, 70f);
        public float spawnZOffset = 300f;

        private void Start() {
            StartCoroutine(SpawnLoop());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        IEnumerator SpawnLoop() {
            while (true) {
                Vector3 pos = new Vector3(
                    Random.value < 0.5f
                        ? Random.Range(xRange.x, xRange.y)
                        : Random.Range(xRangeRight.x, xRangeRight.y),
                    4f,
                    Camera.main.transform.position.z + spawnZOffset + Random.Range(-2f, 2f)
                );

                string tag = Random.Range(0, 3) switch {
                    0 => "ContainerRed",
                    1 => "ContainerGreen",
                    2 => "Cylinder",
                    _ => "ContainerBlue"
                };

                float yRot = Random.Range(0, 1) * 90f;
                ObjectPool.Instance.SpawnFromPool(tag, pos, Quaternion.Euler(0f, yRot, 0f));
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}