using System.Collections;
using StartScene;
using UnityEngine;

namespace GameScene {
    public class PauseMenu : MonoBehaviour {
        public GameObject pauseMenuCanvas;
        private bool _paused;
        private AudioSource _music;
        public AudioClip buttonSound;
        private CanvasGroup _canvasGroup;


        private void Start() {
            _music = GameObject.Find("Music").GetComponent<AudioSource>();
            _canvasGroup = pauseMenuCanvas.GetComponent<CanvasGroup>();

            _paused = false;
            pauseMenuCanvas.SetActive(false);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                TogglePause();
            }
        }

        private void TogglePause() {
            _paused = !_paused;
            if (_paused) {
                StartCoroutine(FadeInPauseMenu());
                Time.timeScale = 0f;
                _music.Pause();
            }
            else {
                StartCoroutine(FadeOutPauseMenu());
                Time.timeScale = 1f;
                _music.UnPause();
            }
        }

        private IEnumerator FadeInPauseMenu() {
            pauseMenuCanvas.SetActive(true);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            var duration = 0.3f;
            var t = 0f;

            while (t < duration) {
                t += Time.unscaledDeltaTime;
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOutPauseMenu() {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            var duration = 0.3f;
            var t = 0f;

            while (t < duration) {
                t += Time.unscaledDeltaTime;
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            pauseMenuCanvas.SetActive(false);
        }

        public void OnMenu() {
            SceneFader.instance.FadeToScene("StartScene");
        }

        public void OnResume() {
            SoundEffectsManager.instance.PlaySoundFX(buttonSound, transform, "SFXVolume");
            TogglePause();
        }
    }
}