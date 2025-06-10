using UnityEngine;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour {
       public GameObject pauseMenuCanvas;
       private bool paused;
       private AudioSource Music;

       private void Start() {
              Music = GameObject.Find("Music").GetComponent<AudioSource>();
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
              if (paused) Music.Pause();
              else Music.UnPause();
              pauseMenuCanvas.SetActive(paused);
              Time.timeScale = paused ? 0f : 1f;
       }

       public void OnMenu() {
              SceneFader.Instance.FadeToScene("StartScene");
       }

       public void OnResume() {
              TogglePause();
       }
}