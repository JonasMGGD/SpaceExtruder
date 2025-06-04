using UnityEngine;

public class Enemy : MonoBehaviour {
    public float moveSpeed = 10f;
    public float fireRate = 2f;
    public Transform[] firePoints;
    public string projectileTag = "Projectile";

    private float fireCooldown = 0f;
    private Transform player;
    private Transform cam;
    private CameraScript _cameraScript;
    public Transform modelRoot;

    private void Start() {
        _cameraScript = cam.GetComponent<CameraScript>();
    }

    private void OnEnable()
    {
        fireCooldown = 0f;
        cam = Camera.main.transform;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Nur Optik um 90° rotieren (nicht Bewegung)
        if (modelRoot != null)
        {
            modelRoot.localRotation = Quaternion.Euler(0, 90f, 0); // oder was du brauchst
        }

        // Optional: Gegner auf Player ausrichten
        if (player)
        {
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    private void Update() {
        // Bewegung nach unten + mit Kamera scroll
        Vector3 scroll = Vector3.forward * _cameraScript.scrollSpeed;
        Vector3 combinedMove = (-transform.forward * moveSpeed + scroll) * Time.deltaTime;
        transform.position += combinedMove;

        // Schießen
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f) {
            Shoot();
            fireCooldown = fireRate;
        }

        // Deaktivieren, wenn weit hinter der Kamera
        if (transform.position.z < cam.position.z - 20f) {
            gameObject.SetActive(false);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Shoot() {
        foreach (Transform firePoint in firePoints)
        {
            GameObject proj = ObjectPool.Instance.SpawnFromPool(projectileTag, firePoint.position, firePoint.rotation);
            if (proj != null)
            {
                Projectile p = proj.GetComponent<Projectile>();
                p.isEnemyProjectile = true;
            }
        }
    }
}