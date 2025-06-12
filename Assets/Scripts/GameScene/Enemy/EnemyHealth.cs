using Score;
using StartScene;
using UnityEngine;

namespace GameScene.Enemy {
    public class EnemyHealth : MonoBehaviour {
        public int maxHealth = 3;
        private int _currentHealth;
        public AudioClip scoreUp;

        public EnemyHealthUI healthUI;

        private string _poolTag;

        private void OnEnable() {
            _currentHealth = maxHealth;
            _poolTag = gameObject.tag;
            if (healthUI) {
                healthUI.UpdateHearts(_currentHealth);
            }
        }

        public void TakeDamage(int amount) {
            _currentHealth -= amount;

            if (healthUI) {
                healthUI.UpdateHearts(_currentHealth);
            }

            if (_currentHealth <= 0) {
                Die();
            }
        }

        void Die() {
            var enemyTag = gameObject.tag;
            if (enemyTag == "EnemyHeinz") {
                ScoreManager.instance.AddScore(100);
                FloatingScoreManager.instance.SpawnScore(transform.position, 100);
            }
            else {
                ScoreManager.instance.AddScore(10);
                FloatingScoreManager.instance.SpawnScore(transform.position, 10);
            }

            SoundEffectsManager.instance.PlaySoundFX(scoreUp, transform, "SFXVolume");
            ObjectPool.instance.SpawnFromPool("ExplosionEffect", transform.position, Quaternion.identity);
            ObjectPool.instance.ReturnToPool(_poolTag, gameObject);
        }
    }
}