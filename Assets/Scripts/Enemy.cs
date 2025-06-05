using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float moveSpeed = 10f;
    public Transform[] firePoints;

    private float fireCooldown;
    private Transform player;
    private Transform cam;
    private CameraScript _cameraScript;

    private void Start() {
        _cameraScript = cam.GetComponent<CameraScript>();
    }

    private void OnEnable() {
        fireCooldown = 0.1f;
        if (Camera.main) cam = Camera.main.transform;
    }

    private void Update() {
        var scroll = Vector3.left * _cameraScript.scrollSpeed;
        var combinedMove = (-transform.forward * moveSpeed + scroll) * Time.deltaTime;
        transform.position += combinedMove;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f) {
            Shoot();
            fireCooldown = Random.Range(1.0f, 2.5f);
        }

        if (transform.position.z < cam.position.z - 20f) {
            gameObject.SetActive(false);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Shoot() {
        int mode = Random.Range(0, 3);

        switch (mode) {
            case 0:
                int index = Random.Range(0, firePoints.Length);
                SpawnProjectile(firePoints[index]);
                break;
            case 1:
                foreach (var firePoint in firePoints) {
                    SpawnProjectile(firePoint);
                }
                break;
            case 2:
                StartCoroutine(DelayedFire(firePoints[0], firePoints[1]));
                break;
        }
    }

    void SpawnProjectile(Transform firePoint) {
        var flippedRotation = Quaternion.Euler(0f, 180f, 0f);
        ObjectPool.Instance.SpawnFromPool("EnemyProjectile", firePoint.position, flippedRotation);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator DelayedFire(Transform fp1, Transform fp2) {
        SpawnProjectile(fp1);
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        SpawnProjectile(fp2);
    }
}