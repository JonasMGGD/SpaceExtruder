using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour {
    public TMP_Text finalScoreText;

    private void Start() {
        int score = PlayerPrefs.GetInt("LastScore", 0);
        finalScoreText.text = "Score: " + score;
    }

    public void OnRetryClicked() {
        SceneFader.Instance.FadeToScene("GameScene");
    }

    public void OnMenuClicked() {
        SceneFader.Instance.FadeToScene("StartScene");
    }
}