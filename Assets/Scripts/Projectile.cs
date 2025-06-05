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
            other.GetComponent<PlayerHealth>()?.TakeDamage(1);
            ObjectPool.Instance.ReturnToPool("EnemyProjectile", gameObject);
        }
        else if (!isEnemyProjectile && other.CompareTag("Enemy")) {
            ScoreManager.Instance.AddScore(10);
            string enemyTag = other.gameObject.tag;
            ObjectPool.Instance.ReturnToPool(enemyTag, other.gameObject);
            ObjectPool.Instance.ReturnToPool("PlayerProjectile", gameObject);
        }
    }
}