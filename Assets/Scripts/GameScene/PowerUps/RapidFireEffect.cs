using UnityEngine;

namespace GameScene.PowerUps {
    [CreateAssetMenu(menuName = "PowerUps/RapidFire")]
    public class RapidFireEffect : PowerUpEffect {
        public override void Activate(PlayerController player) {
            player.SetRapidFire(true);
        }

        public override void Deactivate(PlayerController player) {
            player.SetRapidFire(false);
        }
    }
}