using TMPro;
using UnityEngine;

public class FloatingScore : MonoBehaviour {
    public float floatSpeed = 40f;
    public float fadeDuration = 0.5f;

    private TextMeshProUGUI tmp;
    private RectTransform rect;
    private Color startColor;

    private void Awake() {
        tmp = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
        startColor = tmp.color;
    }

    public void Init(string text, Color color) {
        tmp.text = text;
        tmp.color = color;
        startColor = color;
        Destroy(gameObject, fadeDuration);
    }

    private void Update() {
        rect.anchoredPosition += Vector2.up * (floatSpeed * Time.deltaTime);

        float alpha = Mathf.Lerp(1f, 0f, Time.deltaTime / fadeDuration);
        tmp.color = new Color(startColor.r, startColor.g, startColor.b, tmp.color.a * alpha);
    }
}