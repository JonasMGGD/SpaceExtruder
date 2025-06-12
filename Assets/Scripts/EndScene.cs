using Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScene : MonoBehaviour {
    public TMP_Text finalScoreText;
    private MusicManager _musicManager;

    private void Start() {
        _musicManager = FindFirstObjectByType<MusicManager>();
        StartCoroutine(_musicManager.MusicFade(1f, 0.7f));
        var score = PlayerPrefs.GetInt("LastScore", 0);
        finalScoreText.text = "Score: " + score;
    }

    public void OnRetryClicked() {
        StartCoroutine(_musicManager.MusicFade(0f, 0.7f));
        SceneFader.instance.FadeToScene("GameScene");
    }

    public void OnMenuClicked() {
        StartCoroutine(_musicManager.MusicFade(0f, 0.7f));
        SceneFader.instance.FadeToScene("StartScene");
    }
}