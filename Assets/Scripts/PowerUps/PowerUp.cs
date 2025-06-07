using UnityEngine;

namespace PowerUps {
    public class PowerUp : MonoBehaviour {
        public PowerUpEffect effect;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                PowerUpManager.Instance.ApplyPowerUp(effect);
                string effectName = effect.name;
                ObjectPool.Instance.ReturnToPool(effectName, gameObject);
            }
        }
    }
}