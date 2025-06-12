using Score;
using UnityEngine;

namespace GameScene.PowerUps {
    [CreateAssetMenu(menuName = "PowerUps/ScoreBoost")]
    public class ScoreBoostEffect : PowerUpEffect {
        private const int ScoreBoostAmount = 500;

        public override void Activate(PlayerController player) {
            ScoreManager.instance.AddScore(ScoreBoostAmount);
        }

        public override void Deactivate(PlayerController player) { }
    }
}