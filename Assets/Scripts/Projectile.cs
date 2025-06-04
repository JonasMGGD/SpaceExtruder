using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 30f;
    public float maxLifetime = 3f;
    public bool isEnemyProjectile = false;
    public Material playerMaterial;
    public Material enemyMaterial;
    private Renderer rend;

    private float lifetime;

    private void OnEnable() {
        lifetime = 0f;

        if (!rend) rend = GetComponent<Renderer>();

        if (isEnemyProjectile) rend.material = enemyMaterial;
        else rend.material = playerMaterial;
    }

    private void Update() {
        transform.position += transform.forward * (speed * Time.deltaTime);

        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime) {
            ObjectPool.Instance.ReturnToPool("Projectile", gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hit: " + other.gameObject.name);
        if (isEnemyProjectile) {
            if (other.CompareTag("Player")) {
                Debug.Log("Player hit!");
                gameObject.SetActive(false);
            }
        }
        else {
            if (other.CompareTag("Enemy")) {
                Debug.Log("Enemy hit!");
                other.gameObject.SetActive(false); // Placeholder: später HP etc.
                gameObject.SetActive(false);
            }
        }
    }
}