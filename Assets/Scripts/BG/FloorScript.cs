using UnityEngine;

namespace BG {
    public class FloorScript : MonoBehaviour {
        private Transform _cam;

        private void Start() {
            _cam = Camera.main.transform;
        }

        private void Update() {
            transform.position = new Vector3(
                0f,
                -2f,
                _cam.position.z
            );
        }
    }
}