using System.Collections;
using UnityEngine;

namespace StartScene {
    public class FloatingEnemy : MonoBehaviour {
        public float moveSpeed = 0.8f;
        public float lifespan = 10f;
        private float _fireCooldown;
        private float _timer;
        public Transform[] firePoints;

        private void Update() {
            transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);

            _fireCooldown -= Time.deltaTime;
            if (_fireCooldown <= 0f) {
                Shoot();
                _fireCooldown = Random.Range(1.3f, 2.1f);
            }

            _timer += Time.deltaTime;
            if (!(_timer >= lifespan)) return;
            ObjectPool.instance.ReturnToPool("Enemy", gameObject);
            _timer = 0f;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SpawnProjectile(Transform firePoint) {
            var flippedRotation = Quaternion.Euler(-90f, 90f, 0f);
            ObjectPool.instance.SpawnFromPool("EnemyProjectile", firePoint.position, flippedRotation);
        }


        private void Shoot() {
            var mode = Random.Range(0, 3);
            switch (mode) {
                case 0:
                    var index = Random.Range(0, firePoints.Length);
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

        private IEnumerator DelayedFire(Transform fp1, Transform fp2) {
            SpawnProjectile(fp1);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            SpawnProjectile(fp2);
        }
    }
}