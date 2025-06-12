using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene.PowerUps {
    public class PowerUpManager : MonoBehaviour {
        public static PowerUpManager instance;
        private PlayerController _player;


        private void Awake() {
            instance = this;
            _player = FindFirstObjectByType<PlayerController>();
        }

        public void ApplyPowerUp(PowerUpEffect effect) {
            effect.Activate(_player);
            StartCoroutine(RemoveAfterDelay(effect));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator RemoveAfterDelay(PowerUpEffect effect) {
            yield return new WaitForSeconds(effect.duration);
            effect.Deactivate(_player);
        }
    }
}