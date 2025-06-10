using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioSource backgroundMusic;

    private void Start() {
        backgroundMusic.volume = 0f;
        backgroundMusic.Play();
        StartCoroutine(MusicFade(0.4f, 5f));
    }

    public System.Collections.IEnumerator MusicFade(float targetVolume, float duration) {
        float startVolume = backgroundMusic.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            backgroundMusic.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
            yield return null;
        }

        backgroundMusic.volume = targetVolume;
    }
}