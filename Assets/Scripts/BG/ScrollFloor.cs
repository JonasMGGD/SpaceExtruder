using UnityEngine;

namespace BG {
    public class ScrollBackground : MonoBehaviour {
        public float scrollSpeed = 0.2f;
        private Renderer _rend;
        private Vector2 _offset;

        private void Start() {
            _rend = GetComponent<Renderer>();
        }

        public void Update() {
            _offset.y += Time.deltaTime * scrollSpeed;
            _rend.material.mainTextureOffset = _offset;
        }
    }
}