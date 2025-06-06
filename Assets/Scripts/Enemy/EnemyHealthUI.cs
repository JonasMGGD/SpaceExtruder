using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyHealthUI : MonoBehaviour {
    public Image[] hearts;

    public void UpdateHearts(int currentHealth) {
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].enabled = i < currentHealth;
        }
    }
}