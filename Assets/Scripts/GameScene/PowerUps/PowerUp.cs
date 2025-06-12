using UnityEngine;

namespace GameScene.PowerUps {
    public class PowerUp : MonoBehaviour {
        public PowerUpEffect effect;

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            PowerUpManager.instance.ApplyPowerUp(effect);
            var powerUpName = gameObject.name.Replace("(Clone)", "").Trim();
            ObjectPool.instance.ReturnToPool(powerUpName, gameObject);
        }
    }
}