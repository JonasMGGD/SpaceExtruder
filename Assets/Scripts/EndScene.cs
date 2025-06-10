using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour {
    public TMP_Text finalScoreText;
    private MusicManager musicManager;

    private void Start() {
        musicManager = FindFirstObjectByType<MusicManager>();
        StartCoroutine(musicManager.MusicFade(1f, 0.7f));
        int score = PlayerPrefs.GetInt("LastScore", 0);
        finalScoreText.text = "Score: " + score;
    }

    public void OnRetryClicked() {
        StartCoroutine(musicManager.MusicFade(0f, 0.7f));
        SceneFader.Instance.FadeToScene("GameScene");
    }

    public void OnMenuClicked() {
        StartCoroutine(musicManager.MusicFade(0f, 0.7f));
        SceneFader.Instance.FadeToScene("StartScene");
    }
}