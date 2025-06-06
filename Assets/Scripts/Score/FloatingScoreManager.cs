using UnityEngine;

public class FloatingScoreManager : MonoBehaviour {
    public static FloatingScoreManager Instance;
    public GameObject floatingScorePrefab;
    public Canvas canvas;

    private void Awake() {
        if (!Instance)
            Instance = this;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SpawnScore(Vector3 worldPos, int amount) {
        if (!floatingScorePrefab || !canvas) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        GameObject fs = Instantiate(floatingScorePrefab, canvas.transform);
        fs.transform.position = screenPos;

        var script = fs.GetComponent<FloatingScore>();
        string prefix = amount > 0 ? "+" : "";
        Color color = amount > 0 ? Color.green : Color.red;

        script.Init(prefix + amount.ToString(), color);
    }
}