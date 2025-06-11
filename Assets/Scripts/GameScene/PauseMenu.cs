using System.Collections;
using StartScene;
using UnityEngine;

namespace GameScene {
    public class PauseMenu : MonoBehaviour {
        public GameObject pauseMenuCanvas;
        private bool paused;
        private AudioSource Music;
        public AudioClip buttonSound;
        private CanvasGroup canvasGroup;


        private void Start() {
            Music = GameObject.Find("Music").GetComponent<AudioSource>();
            canvasGroup = pauseMenuCanvas.GetComponent<CanvasGroup>();

            paused = false;
            pauseMenuCanvas.SetActive(false);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                TogglePause();
            }
        }

        private void TogglePause() {
            paused = !paused;
            if (paused) {
                StartCoroutine(FadeInPauseMenu());
                Time.timeScale = 0f;
                Music.Pause();
            }
            else {
                StartCoroutine(FadeOutPauseMenu());
                Time.timeScale = 1f;
                Music.UnPause();
            }
        }

        private IEnumerator FadeInPauseMenu() {
            pauseMenuCanvas.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            float duration = 0.3f;
            float t = 0f;

            while (t < duration) {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOutPauseMenu() {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float duration = 0.3f;
            float t = 0f;

            while (t < duration) {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            pauseMenuCanvas.SetActive(false);
        }

        public void OnMenu() {
            SceneFader.Instance.FadeToScene("StartScene");
        }

        public void OnResume() {
            SoundEffectsManager.instance.PlaySoundFX(buttonSound, transform, "SFXVolume");
            TogglePause();
        }
    }
}