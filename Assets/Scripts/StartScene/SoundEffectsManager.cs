using UnityEngine;

namespace StartScene {
    public class SoundEffectsManager : MonoBehaviour {
        public static SoundEffectsManager instance;
        [SerializeField] private AudioSource audioSource;

        private void Awake() {
            if (!instance) {
                instance = this;
            }
        }

        public void PlaySoundFX(AudioClip clip, Transform spawnPosition, string groupName) {
            var source = Instantiate(audioSource, spawnPosition.position, Quaternion.identity);

            if (clip.name == "shootSound")source.pitch = Random.Range(0.8f, 1.2f);
            source.clip = clip;
            source.volume = PlayerPrefs.GetFloat(groupName, 1f);
            source.Play();

            Destroy(source.gameObject, clip.length);
        }
    }
}