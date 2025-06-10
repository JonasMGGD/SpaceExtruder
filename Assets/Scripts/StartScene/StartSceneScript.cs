using System.Linq;
using TMPro;
using UnityEngine;

namespace StartScene {
    public class StartSceneScript : MonoBehaviour {
        [Header("UI References")] public TMP_InputField nameInput;
        public Transform leaderboardParent;
        public GameObject scoreEntryPrefab;
        public GameObject noScoresText;
        public GameObject powerUpInfo;
        public GameObject settings;
        [Header("Audio References")]
        public AudioClip playButtonClickSound;
        public AudioClip deleteScoresClickSound;
        public AudioClip powerUpAndQuitInfoClickSound;

        public AudioSource musicSource;

        private void Start() {
            musicSource.loop = true;
            musicSource.Play();
            powerUpInfo.SetActive(false);
            settings.SetActive(false);
            DisplayScores();
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
            powerUpInfo.SetActive(true);
        }

        public void OnPowerUpInfoCloseClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            powerUpInfo.SetActive(false);
        }

        public void OnSettingsClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            settings.SetActive(true);
        }

        public void OnSettingsCloseClicked() {
            SoundEffectsManager.instance.PlaySoundFX(powerUpAndQuitInfoClickSound, transform, "SFXVolume");
            settings.SetActive(false);
        }

    }
}