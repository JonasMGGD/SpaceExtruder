using UnityEngine;

namespace GameScene.PowerUps {
    public abstract class PowerUpEffect : ScriptableObject {
        public float duration = 5f;

        public abstract void Activate(PlayerController player);
        public abstract void Deactivate(PlayerController player);
    }
}