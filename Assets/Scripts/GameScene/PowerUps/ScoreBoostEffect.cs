using Player;
using UnityEngine;

namespace PowerUps {
    [CreateAssetMenu(menuName = "PowerUps/ScoreBoost")]
    public class ScoreBoostEffect : PowerUpEffect {
        private readonly int scoreBoostAmount = 500;

        public override void Activate(PlayerController player) {
            ScoreManager.Instance.AddScore(scoreBoostAmount);
        }

        public override void Deactivate(PlayerController player) { }
    }
}