using UnityEngine;
using UnityEngine.Audio;

namespace Sounds {
    public class SoundMixerManager : MonoBehaviour {
        public static SoundMixerManager Instance { get; private set; }

        public AudioMixer audioMixer;

        private void Awake() {
            if (Instance && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetMasterVolume(float volume) {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
            PlayerPrefs.SetFloat("MasterVolume", volume);
        }

        public void SetMusicVolume(float volume) {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void SetSFXVolume(float volume) {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        public float GetMasterVolume() {
            return audioMixer.GetFloat("MasterVolume", out var value) ? Mathf.Pow(10, value / 20) : 1f;
        }

        public float GetMusicVolume() {
            return audioMixer.GetFloat("MusicVolume", out var value) ? Mathf.Pow(10, value / 20) : 1f;
        }

        public float GetSFXVolume() {
            return audioMixer.GetFloat("SFXVolume", out var value) ? Mathf.Pow(10, value / 20) : 1f;
        }
    }
}
