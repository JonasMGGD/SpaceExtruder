using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Enemy {
    public class EnemyHealthUI : MonoBehaviour {
        public Image[] hearts;

        public void UpdateHearts(int currentHealth) {
            for (var i = 0; i < hearts.Length; i++) {
                hearts[i].enabled = i < currentHealth;
            }
        }
    }
}