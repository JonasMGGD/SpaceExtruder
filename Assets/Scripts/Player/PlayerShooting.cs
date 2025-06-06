using UnityEngine;

public class PlayerShooting : MonoBehaviour {
    public Transform firePoint;
    public float fireRate = 0.2f;

    private float fireCooldown = 0f;

    private void Update() {
        fireCooldown -= Time.deltaTime;

        if (Input.GetMouseButton(0) && fireCooldown <= 0f) {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Shoot() {
        GameObject projectile = ObjectPool.Instance.SpawnFromPool("PlayerProjectile", firePoint.position, firePoint.rotation);
    }
}