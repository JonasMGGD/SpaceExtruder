using TMPro;
using UnityEngine;

namespace Score {
    public class FloatingScore : MonoBehaviour {
        public float floatSpeed = 40f;
        public float fadeDuration = 0.5f;

        private TextMeshProUGUI _tmp;
        private RectTransform _rect;
        private Color _startColor;

        private void Awake() {
            _tmp = GetComponent<TextMeshProUGUI>();
            _rect = GetComponent<RectTransform>();
            _startColor = _tmp.color;
        }

        public void Init(string text, Color color) {
            _tmp.text = text;
            _tmp.color = color;
            _startColor = color;
            Destroy(gameObject, fadeDuration);
        }

        private void Update() {
            _rect.anchoredPosition += Vector2.up * (floatSpeed * Time.deltaTime);

            var alpha = Mathf.Lerp(1f, 0f, Time.deltaTime / fadeDuration);
            _tmp.color = new Color(_startColor.r, _startColor.g, _startColor.b, _tmp.color.a * alpha);
        }
    }
}