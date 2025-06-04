using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 20f;

    public Vector2 xLimits = new(-10f, 10f);
    public Vector2 zOffsetLimits = new(-5f, 5f);

    private Rigidbody rb;
    private Transform cam;

    private Vector3 offset = Vector3.zero;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        cam = Camera.main.transform;
    }

    private void Update() {
        HandleInput();
        UpdatePosition();
    }

    void HandleInput() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, 0, v).normalized * (moveSpeed * Time.deltaTime);
        offset += input;

        offset.x = Mathf.Clamp(offset.x, xLimits.x, xLimits.y);
        offset.z = Mathf.Clamp(offset.z, zOffsetLimits.x, zOffsetLimits.y);
    }

    void UpdatePosition() {
        Vector3 basePosition = cam.position + offset;
        basePosition.y = transform.position.y;

        rb.MovePosition(basePosition);
    }
}