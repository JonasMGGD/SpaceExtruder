using UnityEngine;

public class Billboard : MonoBehaviour {
    private Transform cam;

    private void Start() {
        cam = Camera.main.transform;
    }

    private void LateUpdate() {
        if (cam) {
            Vector3 lookPos = cam.position;
            lookPos.x = transform.position.x;
            transform.LookAt(lookPos);
        }
    }
}