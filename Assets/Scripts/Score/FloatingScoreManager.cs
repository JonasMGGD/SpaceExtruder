using UnityEngine;

namespace Score {
    public class FloatingScoreManager : MonoBehaviour {
        public static FloatingScoreManager instance;
        public GameObject floatingScorePrefab;
        public Canvas canvas;

        private void Awake() {
            if (!instance)
                instance = this;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void SpawnScore(Vector3 worldPos, int amount) {
            if (!floatingScorePrefab || !canvas) return;

            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            var fs = Instantiate(floatingScorePrefab, canvas.transform);
            fs.transform.position = screenPos;

            var script = fs.GetComponent<FloatingScore>();
            var prefix = amount > 0 ? "+" : "";
            var color = amount > 0 ? Color.green : Color.red;

            script.Init(prefix + amount.ToString(), color);
        }
    }
}