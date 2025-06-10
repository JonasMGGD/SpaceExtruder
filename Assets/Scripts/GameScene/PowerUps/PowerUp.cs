using Unity.VisualScripting;
using UnityEngine;

namespace PowerUps {
    public class PowerUp : MonoBehaviour {
        public PowerUpEffect effect;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                PowerUpManager.Instance.ApplyPowerUp(effect);
                string name = gameObject.name.Replace("(Clone)", "").Trim();
                ObjectPool.Instance.ReturnToPool(name, gameObject);
            }
        }
    }
}