using UnityEngine;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour {
       public GameObject pauseMenuCanvas;
       private bool paused;

       private void Start() {
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
              pauseMenuCanvas.SetActive(paused);
              Time.timeScale = paused ? 0f : 1f;
              Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
              Cursor.visible = paused;
       }

       public void OnMenu() {
              SceneFader.Instance.FadeToScene("StartScene");
       }

       public void OnResume() {
              TogglePause();
       }
}