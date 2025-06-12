using UnityEngine;

namespace GameScene.PowerUps {
    [CreateAssetMenu(menuName = "PowerUps/Invincibility")]
    public class InvincibilityEffect : PowerUpEffect{
        public override void Activate(PlayerController player) {
            player.SetInvincible(true);
        }
        public override void Deactivate(PlayerController player) {
            player.SetInvincible(false);
        }
    }
}