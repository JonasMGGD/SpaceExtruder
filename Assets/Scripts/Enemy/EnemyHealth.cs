using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int maxHealth = 3;
    private int currentHealth;

    public EnemyHealthUI healthUI;

    private string poolTag;

    private void OnEnable() {
        currentHealth = maxHealth;
        poolTag = gameObject.tag;
        if (healthUI) {
            healthUI.UpdateHearts(currentHealth);
        }
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        if (healthUI) {
            healthUI.UpdateHearts(currentHealth);
        }

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        ScoreManager.Instance.AddScore(10);
        FloatingScoreManager.Instance.SpawnScore(transform.position, 10);
        ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
    }
}