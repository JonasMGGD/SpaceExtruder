using UnityEngine;

namespace GameScene.Enemy {
    public class Billboard : MonoBehaviour {
        private Transform _cam;

        private void Start() {
            _cam = Camera.main.transform;
        }

        private void LateUpdate() {
            if (!_cam) return;
            Vector3 lookPos = _cam.position;
            lookPos.x = transform.position.x;
            transform.LookAt(lookPos);
        }
    }
}