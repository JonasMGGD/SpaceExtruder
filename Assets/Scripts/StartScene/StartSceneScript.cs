using System.Collections;
using System.Linq;
using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StartScene {
    public class StartSceneScript : MonoBehaviour {
        [Header("UI References")] public TMP_InputField nameInput;
        public Transform leaderboardParent;
        public GameObject scoreEntryPrefab;
        public GameObject noScoresText;
        public GameObject powerUpInfo;
        public GameObject settings;
        private CanvasGroup settingsGroup;
        private CanvasGroup powerUpInfoGroup;
        [Header("Audio References")]
        public AudioClip playButtonClickSound;
        public AudioClip deleteScoresClickSound;
        public AudioClip powerUpAndQuitInfoClickSound;
        [Header("Settings")]
        public TMP_Text healthValueText;
        private int healthValue;
        public int minHealth = 1;
        public int maxHealth = 20;
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;

        public AudioSource musicSource;

        private void Awake() {
            if (masterSlider) {
                float value = PlayerPrefs.GetFloat("MasterVolume", 1f);
                masterSlider.value = value;
                masterSlider.onValueChanged.AddListener(volume => SoundMixerManager.Instance.SetMasterVolume(volume));
            }

            if (musicSlider) {
                float value = PlayerPrefs.GetFloat("MusicVolume", 1f);
                musicSlider.value = value;
                musicSlider.onValueChanged.AddListener(volume => SoundMixerManager.Instance.SetMusicVolume(volume));
            }

            if (sfxSlider) {
                float value = PlayerPrefs.GetFloat("SFXVolume", 1f);
                sfxSlider.value = value;
                sfxSlider.onValueChanged.AddListener(volume => SoundMixerManager.Instance.SetSFXVolume(volume));
            }
        }

        private void Start() {
            settingsGroup = settings.GetComponent<CanvasGroup>();
            powerUpInfoGroup = powerUpInfo.GetComponent<CanvasGroup>();
            settings.SetActive(false);
            powerUpInfo.SetActive(false);
            settingsGroup.alpha = 0f;
            powerUpInfoGroup.alpha = 0f;

            musicSource.loop = true;
            musicSource.Play();
            DisplayScores();
            healthValue = PlayerPrefs.GetInt("Health", 10);
            UpdateHealthDisplay();
        }

        private void DisplayScores() {
            foreach (Transform child in leaderboardParent)
                Destroy(child.gameObject);

            var scores = LeaderboardManager.Instance.scores
                .OrderByDescending(s => s.score)
                .Take(10)
                .ToList();

            if (scores.Count == 0 && noScoresText)
                noScoresText.SetActive(true);
            else if (noScoresText != null)
                noScoresText.SetActive(false);

            for (int i = 0; i < scores.Count; i++) {
                var s = scores[i];
                GameObject entry = Instantiate(scoreEntryPrefab, leaderboardParent);
                var texts = entry.GetComponentsInChildren<TMP_Text>();

                texts[0].text = s.playerName;
                texts[1].text = s.score.ToString();

                if (i == 0)
                    SetColor(texts, new Color(1f, 0.84f, 0f));
                else if (i == 1)
                    SetColor(texts, new Color(0.75f, 0.75f, 0.75f));
                else if (i == 2)
                    SetColor(texts, new Color(0.8f, 0.5f, 0.2f));
                else
                    SetColor(texts, Color.grey);
            }

            void SetColor(TMP_Text[] texts, Color color) {
                foreach (var t in texts)
                    t.color = color;
            }
        }

        public void OnPlayClicked() {
            string name = nameInput.text.Trim();
            if (string.IsNullOrEmpty(name)) return;

            PlayerPrefs.SetString("PlayerName", name);
            SoundEffectsManager.instance.PlaySoundFX(playButtonClickSound, transform, "SFXVolume");
            SceneFader.Instance.FadeToScene("GameScene");
        }

        public void OnClearScoresClicked() {
            SoundEffectsManager.instance.PlaySoundFX(deleteScoresClickSound, transform, "SFXVolume");
            LeaderboardManager.Instance.ClearScores();
            DisplayScores();
        }

        public void OnQuitClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            Application.Quit();
        }

        public void OnPowerUpInfoClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            StartCoroutine(FadeCanvasGroup(powerUpInfoGroup, true));
        }

        public void OnPowerUpInfoCloseClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            StartCoroutine(FadeCanvasGroup(powerUpInfoGroup, false));
        }

        public void OnSettingsClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            StartCoroutine(FadeCanvasGroup(settingsGroup, true));
        }

        public void OnSettingsSaveClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            PlayerPrefs.SetInt("Health", healthValue);
            StartCoroutine(FadeCanvasGroup(settingsGroup, false));
        }

        public void OnIncreaseHealthClicked() {
            if (healthValue < maxHealth) {
                SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
                healthValue++;
                UpdateHealthDisplay();
            }
        }

        public void OnDecreaseHealthClicked() {
            if (healthValue > minHealth) {
                SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
                healthValue--;
                UpdateHealthDisplay();
            }
        }

        private void UpdateHealthDisplay() {
            healthValueText.text = healthValue.ToString();
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup group, bool fadeIn, float duration = 0.1f) {
            if (fadeIn) group.gameObject.SetActive(true);

            float start = group.alpha;
            float end = fadeIn ? 1f : 0f;
            float time = 0f;

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