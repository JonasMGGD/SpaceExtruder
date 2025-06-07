using Player;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 30f;
    public float maxLifetime = 3f;
    public bool isEnemyProjectile;

    private float lifetime;

    private void OnEnable() {
        lifetime = 0f;
    }

    private void Update() {
        transform.position += transform.forward * (speed * Time.deltaTime);

        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime && !isEnemyProjectile) {
            ObjectPool.Instance.ReturnToPool("PlayerProjectile", gameObject);
        }
        else if (lifetime >= maxLifetime && isEnemyProjectile) {
            ObjectPool.Instance.ReturnToPool("EnemyProjectile", gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (isEnemyProjectile && other.CompareTag("Player")) {
            other.GetComponent<PlayerController>()?.TakeDamage(1);
            ObjectPool.Instance.ReturnToPool("EnemyProjectile", gameObject);
        }
        else if (!isEnemyProjectile && (other.CompareTag("Enemy") || other.CompareTag("EnemyHeinz"))) {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(1);
            }
            ObjectPool.Instance.ReturnToPool("PlayerProjectile", gameObject);
        }
    }
}