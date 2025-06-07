using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace PowerUps {
    public class PowerUpManager : MonoBehaviour {
        public static PowerUpManager Instance;
        private PlayerController player;
        private readonly List<Coroutine> activeCoroutines = new();


        private void Awake() {
            Instance = this;
            player = FindObjectOfType<PlayerController>();
        }

        public void ApplyPowerUp(PowerUpEffect effect) {
            effect.Activate(player);
            Coroutine c = StartCoroutine(RemoveAfterDelay(effect));
            activeCoroutines.Add(c);
        }

        private IEnumerator RemoveAfterDelay(PowerUpEffect effect) {
            yield return new WaitForSeconds(effect.duration);
            effect.Deactivate(player);
        }
    }
}