using GameScene;
using GameScene.Enemy;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 30f;
    public float maxLifetime = 3f;
    public bool isEnemyProjectile;

    private float _lifetime;

    private void OnEnable() {
        _lifetime = 0f;
    }

    private void Update() {
        transform.position += transform.forward * (speed * Time.deltaTime);

        _lifetime += Time.deltaTime;
        if (_lifetime >= maxLifetime && !isEnemyProjectile) {
            ObjectPool.instance.ReturnToPool("PlayerProjectile", gameObject);
        }
        else if (_lifetime >= maxLifetime && isEnemyProjectile) {
            ObjectPool.instance.ReturnToPool("EnemyProjectile", gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        switch (isEnemyProjectile) {
            case true when other.CompareTag("Player"):
                other.GetComponent<PlayerController>()?.TakeDamage(1);
                ObjectPool.instance.ReturnToPool("EnemyProjectile", gameObject);
                break;
            case false when (other.CompareTag("Enemy") || other.CompareTag("EnemyHeinz")): {
                var enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null) {
                    enemyHealth.TakeDamage(1);
                }
                ObjectPool.instance.ReturnToPool("PlayerProjectile", gameObject);
                break;
            }
        }
    }
}