using System.Collections;
using System.Linq;
using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace StartScene {
    public class StartSceneScript : MonoBehaviour {
        [Header("UI References")] public TMP_InputField nameInput;
        public Transform leaderboardParent;
        public GameObject scoreEntryPrefab;
        public GameObject noScoresText;
        public GameObject powerUpInfo;
        public GameObject settings;
        private CanvasGroup _settingsGroup;
        private CanvasGroup _powerUpInfoGroup;
        [Header("Audio References")]
        public AudioClip playButtonClickSound;
        public AudioClip deleteScoresClickSound;
        public AudioClip powerUpAndQuitInfoClickSound;
        public AudioSource musicSource;
        [Header("Settings")]
        public TMP_Text healthValueText;
        private int _healthValue;
        public int minHealth = 1;
        public int maxHealth = 20;
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;


        private void Awake() {
            if (masterSlider) {
                var value = PlayerPrefs.GetFloat("MasterVolume", 1f);
                masterSlider.value = value;
                masterSlider.onValueChanged.AddListener(volume => SoundMixerManager.Instance.SetMasterVolume(volume));
            }

            if (musicSlider) {
                var value = PlayerPrefs.GetFloat("MusicVolume", 1f);
                musicSlider.value = value;
                musicSlider.onValueChanged.AddListener(volume => SoundMixerManager.Instance.SetMusicVolume(volume));
            }

            if (sfxSlider) {
                var value = PlayerPrefs.GetFloat("SFXVolume", 1f);
                sfxSlider.value = value;
                sfxSlider.onValueChanged.AddListener(volume => SoundMixerManager.Instance.SetSFXVolume(volume));
            }

            StartCoroutine(ApplyInitialMixerValuesDelayed());
        }

        private IEnumerator ApplyInitialMixerValuesDelayed() {
            yield return null;

            SoundMixerManager.Instance.SetMasterVolume(masterSlider.value);
            SoundMixerManager.Instance.SetMusicVolume(musicSlider.value);
            SoundMixerManager.Instance.SetSFXVolume(sfxSlider.value);
        }

        private void Start() {
            _settingsGroup = settings.GetComponent<CanvasGroup>();
            _powerUpInfoGroup = powerUpInfo.GetComponent<CanvasGroup>();
            settings.SetActive(false);
            powerUpInfo.SetActive(false);
            _settingsGroup.alpha = 0f;
            _powerUpInfoGroup.alpha = 0f;
            musicSource.loop = true;
            musicSource.Play();
            DisplayScores();
            _healthValue = PlayerPrefs.GetInt("Health", 10);
            UpdateHealthDisplay();
        }

        private void DisplayScores() {
            foreach (Transform child in leaderboardParent)
                Destroy(child.gameObject);

            var scores = LeaderboardManager.instance.scores
                .OrderByDescending(s => s.score)
                .Take(10)
                .ToList();

            if (scores.Count == 0 && noScoresText)
                noScoresText.SetActive(true);
            else if (noScoresText)
                noScoresText.SetActive(false);

            var titles = Instantiate(scoreEntryPrefab, leaderboardParent);
            var titleTexts = titles.GetComponentsInChildren<TMP_Text>();
            titleTexts[0].text = "Name";
            titleTexts[1].text = "Score";

            for (var i = 0; i < scores.Count; i++) {
                var entry = Instantiate(scoreEntryPrefab, leaderboardParent);
                var s = scores[i];
                var texts = entry.GetComponentsInChildren<TMP_Text>();

                texts[0].text = s.playerName;
                texts[1].text = s.score.ToString();

                switch (i) {
                    case 0:
                        SetColor(texts, new Color(1f, 0.84f, 0f));
                        break;
                    case 1:
                        SetColor(texts, new Color(0.75f, 0.75f, 0.75f));
                        break;
                    case 2:
                        SetColor(texts, new Color(0.8f, 0.5f, 0.2f));
                        break;
                    default:
                        SetColor(texts, Color.grey);
                        break;
                }
            }
        }

        private static void SetColor(TMP_Text[] texts, Color color) {
            foreach (var t in texts)
                t.color = color;
        }

        public void OnPlayClicked() {
            var playerName = nameInput.text.Trim();
            if (string.IsNullOrEmpty(playerName)) return;

            PlayerPrefs.SetString("PlayerName", playerName);
            SoundEffectsManager.instance.PlaySoundFX(playButtonClickSound, transform, "SFXVolume");
            SceneFader.instance.FadeToScene("GameScene");
        }

        public void OnClearScoresClicked() {
            SoundEffectsManager.instance.PlaySoundFX(deleteScoresClickSound, transform, "SFXVolume");
            LeaderboardManager.instance.ClearScores();
            DisplayScores();
        }

        public void OnQuitClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            Application.Quit();
        }

        public void OnPowerUpInfoClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            StartCoroutine(FadeCanvasGroup(_powerUpInfoGroup, true));
        }

        public void OnPowerUpInfoCloseClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            StartCoroutine(FadeCanvasGroup(_powerUpInfoGroup, false));
        }

        public void OnSettingsClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            StartCoroutine(FadeCanvasGroup(_settingsGroup, true));
        }

        public void OnSettingsSaveClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            PlayerPrefs.SetInt("Health", _healthValue);
            StartCoroutine(FadeCanvasGroup(_settingsGroup, false));
        }

        public void OnIncreaseHealthClicked() {
            if (_healthValue >= maxHealth) return;
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            _healthValue++;
            UpdateHealthDisplay();
        }

        public void OnDecreaseHealthClicked() {
            if (_healthValue <= minHealth) return;
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            _healthValue--;
            UpdateHealthDisplay();
        }

        private void UpdateHealthDisplay() {
            healthValueText.text = _healthValue.ToString();
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup group, bool fadeIn, float duration = 0.1f) {
            if (fadeIn) group.gameObject.SetActive(true);

            var start = group.alpha;
            var end = fadeIn ? 1f : 0f;
            var time = 0f;

            while (time < duration) {
                time += Time.unscaledDeltaTime;
                group.alpha = Mathf.Lerp(start, end, time / duration);
                yield return null;
            }

            group.alpha = end;
            group.interactable = fadeIn;
            group.blocksRaycasts = fadeIn;

            if (!fadeIn) group.gameObject.SetActive(false);
        }
    }
}