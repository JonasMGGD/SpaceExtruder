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
        SceneManager.LoadScene("GameScene");
    }

    public void OnMenuClicked() {
        SceneManager.LoadScene("StartScene");
    }
}