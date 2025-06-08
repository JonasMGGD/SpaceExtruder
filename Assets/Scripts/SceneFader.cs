using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour {
    public static SceneFader Instance;
    public Image fadeImage;
    public float fadeDuration = 0.7f;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    public void FadeToScene(string sceneName) {
        StartCoroutine(FadeRoutine(sceneName));
    }

    IEnumerator FadeRoutine(string sceneName) {
        yield return StartCoroutine(FadeIn());
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        yield return null;
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn() {
        float t = 0;
        while (t < fadeDuration) {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(0, 1, t / fadeDuration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(1);
    }

    IEnumerator FadeOut() {
        float t = 0;
        while (t < fadeDuration) {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(1, 0, t / fadeDuration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(0);
    }

    void SetAlpha(float a) {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}