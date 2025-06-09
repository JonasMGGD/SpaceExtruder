using System.Collections;
using UnityEngine;

namespace StartScene {
    public class FloatingEnemy : MonoBehaviour {
        public float moveSpeed = 0.8f;
        public float lifespan = 10f;
        private float fireCooldown;
        private float timer = 0f;
        public Transform[] firePoints;

        private void Update() {
            transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);

            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f) {
                Shoot();
                fireCooldown = Random.Range(1.3f, 2.1f);
            }

            timer += Time.deltaTime;
            if (timer >= lifespan) {
                ObjectPool.Instance.ReturnToPool("Enemy", gameObject);
                timer = 0f;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void SpawnProjectile(Transform firePoint) {
            var flippedRotation = Quaternion.Euler(-90f, 90f, 0f);
            ObjectPool.Instance.SpawnFromPool("EnemyProjectile", firePoint.position, flippedRotation);
        }


        void Shoot() {
            int mode = Random.Range(0, 3);
            switch (mode) {
                case 0:
                    int index = Random.Range(0, firePoints.Length);
                    SpawnProjectile(firePoints[index]);
                    break;
                case 1:
                    foreach (var firePoint in firePoints) SpawnProjectile(firePoint);
                    break;
                case 2:
                    StartCoroutine(DelayedFire(firePoints[0], firePoints[1]));
                    break;
            }
        }

        IEnumerator DelayedFire(Transform fp1, Transform fp2) {
            SpawnProjectile(fp1);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            SpawnProjectile(fp2);
        }
    }
}