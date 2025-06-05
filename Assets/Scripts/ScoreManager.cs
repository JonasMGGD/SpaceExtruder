using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    private int score = 0;

    private void Awake() {
        if (!Instance)
            Instance = this;
    }

    public void AddScore(int amount) {
        score += amount;
        UpdateUI();
    }

    private void UpdateUI() {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore() {
        return score;
    }
}