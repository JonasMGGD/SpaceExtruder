using UnityEngine;

namespace Floor {
    public class FloorDecorationMover : MonoBehaviour {
        public float speed = -0.2f;

        private void Update() {
            Vector3 pos = transform.position;
            Vector3 forwardMove = -transform.forward * speed;

            transform.position = new Vector3(pos.x, pos.y, pos.z + forwardMove.z * Time.deltaTime);

            if (transform.position.z <= -280f) {
                string name = gameObject.name;
                ObjectPool.Instance.ReturnToPool(name, gameObject);
            }
        }
    }
}