using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene {
    public class UIManager : MonoBehaviour {
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;

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
    }
}