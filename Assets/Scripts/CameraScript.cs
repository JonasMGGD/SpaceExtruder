using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float scrollSpeed = 20f;

    private void Update() {
        transform.position += Vector3.forward * (scrollSpeed * Time.deltaTime);
    }
}