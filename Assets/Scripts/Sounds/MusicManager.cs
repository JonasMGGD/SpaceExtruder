using UnityEngine;

namespace Sounds {
    public class MusicManager : MonoBehaviour {
        public AudioSource backgroundMusic;

        private void Start() {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.4f);
            backgroundMusic.volume = 0f;
            backgroundMusic.Play();
            StartCoroutine(MusicFade(musicVolume, 5f));
        }

        public System.Collections.IEnumerator MusicFade(float targetVolume, float duration) {
            var startVolume = backgroundMusic.volume;
            var elapsedTime = 0f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                backgroundMusic.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
                yield return null;
            }

            backgroundMusic.volume = targetVolume;
        }
    }
}