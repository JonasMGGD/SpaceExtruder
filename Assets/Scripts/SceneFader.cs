using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour {
    public static SceneFader instance;
    public Image fadeImage;
    public float fadeDuration = 0.7f;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void FadeToScene(string sceneName) {
        StartCoroutine(FadeRoutine(sceneName));
    }

    private IEnumerator FadeRoutine(string sceneName) {
        yield return StartCoroutine(FadeIn());
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        yield return null;
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn() {
        float t = 0;
        while (t < fadeDuration) {
            t += Time.unscaledDeltaTime;
            var a = Mathf.Lerp(0, 1, t / fadeDuration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(1);
    }

    private IEnumerator FadeOut() {
        float t = 0;
        while (t < fadeDuration) {
            t += Time.unscaledDeltaTime;
            var a = Mathf.Lerp(1, 0, t / fadeDuration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(0);
    }

    private void SetAlpha(float a) {
        var c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}