using System.Collections;
using UnityEngine;

namespace GameScene {
    public class ExplosionEffect : MonoBehaviour {
        private void OnEnable() {
            StartCoroutine(DisableAfterSeconds(1f));
        }

        private IEnumerator DisableAfterSeconds(float t) {
            yield return new WaitForSeconds(t);
            ObjectPool.Instance.ReturnToPool("ExplosionEffect", gameObject);
        }
    }
}