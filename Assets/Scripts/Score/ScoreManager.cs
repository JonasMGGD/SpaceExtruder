using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    private int score = 0;
    private Coroutine colorRoutine;
    private Color baseColor = Color.white;

    private void Awake() {
        if (!Instance)
            Instance = this;
    }

    public void AddScore(int amount) {
        score += amount;
        UpdateUI();

        if (scoreText) {
            if (colorRoutine != null) StopCoroutine(colorRoutine);

            Color flashColor = amount > 0 ? Color.green : (amount < 0 ? Color.red : baseColor);
            colorRoutine = StartCoroutine(FlashColor(flashColor, 0.1f));
        }
    }

    private void UpdateUI() {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public int GetScore() {
        return score;
    }

    IEnumerator FlashColor(Color flashColor, float duration) {
        scoreText.color = flashColor;

        float t = 0f;
        while (t < duration) {
            t += Time.deltaTime;
            scoreText.color = Color.Lerp(flashColor, baseColor, t / duration);
            yield return null;
        }

        scoreText.color = baseColor;
    }
}