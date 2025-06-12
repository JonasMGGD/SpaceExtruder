using System.Collections;
using TMPro;
using UnityEngine;

namespace Score {

    public class ScoreManager : MonoBehaviour {
        public static ScoreManager instance;

        public TextMeshProUGUI scoreText;
        private int _score;
        private Coroutine _colorRoutine;
        private readonly Color _baseColor = Color.white;

        private void Awake() {
            if (!instance)
                instance = this;
        }

        public void AddScore(int amount) {
            _score += amount;
            UpdateUI();

            if (scoreText) {
                if (_colorRoutine != null) StopCoroutine(_colorRoutine);

                var flashColor = amount > 0 ? Color.green : (amount < 0 ? Color.red : _baseColor);
                _colorRoutine = StartCoroutine(FlashColor(flashColor, 0.1f));
            }
        }

        private void UpdateUI() {
            if (scoreText)
                scoreText.text = "Score: " + _score;
        }

        public int GetScore() {
            return _score;
        }

        private IEnumerator FlashColor(Color flashColor, float duration) {
            scoreText.color = flashColor;

            var t = 0f;
            while (t < duration) {
                t += Time.deltaTime;
                scoreText.color = Color.Lerp(flashColor, _baseColor, t / duration);
                yield return null;
            }

            scoreText.color = _baseColor;
        }
    }
}