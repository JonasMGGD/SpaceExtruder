using UnityEngine;

public class ScrollBackground : MonoBehaviour {
    public float scrollSpeed = 0.2f;
    private Renderer rend;
    private Vector2 offset;

    void Start() {
        rend = GetComponent<Renderer>();
    }

    void Update() {
        offset.y += Time.deltaTime * scrollSpeed;
        rend.material.mainTextureOffset = offset;
    }
}