using UnityEngine;

public class FloorScript : MonoBehaviour {
    private Transform cam;

    void Start() {
        cam = Camera.main.transform;
    }

    void Update() {
        transform.position = new Vector3(
            0f,
            -2f,
            cam.position.z
        );
    }
}