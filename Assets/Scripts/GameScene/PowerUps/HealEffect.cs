using Player;
using UnityEngine;

namespace PowerUps {
    [CreateAssetMenu(menuName = "PowerUps/Heal")]
    public class HealEffect : PowerUpEffect{
        public override void Activate(PlayerController player) {
            player.Heal(1);
        }
        public override void Deactivate(PlayerController player) { }
    }
}