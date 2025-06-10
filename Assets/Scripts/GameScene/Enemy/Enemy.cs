using System.Collections;
using Score;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float moveSpeed = 10f;
    public Transform[] firePoints;

    private float fireCooldown;
    private Transform player;
    private Transform cam;
    private CameraScript _cameraScript;

    public float xMin = -10f;
    public float xMax = 10f;
    private float xTarget;
    private float xChangeTimer;
    private float xVelocity;
    public float smoothTime = 0.3f;

    private void Start() {
        cam = Camera.main.transform;
        _cameraScript = cam.GetComponent<CameraScript>();
        PickNewXTarget();
    }

    private void OnEnable() {
        fireCooldown = 0.1f;
        xChangeTimer = Random.Range(0.4f, 1.3f);
        PickNewXTarget();
    }

    private void Update() {
        if (!cam || !_cameraScript) return;

        Vector3 scroll = Vector3.left * _cameraScript.scrollSpeed;
        Vector3 forwardMove = -transform.forward * moveSpeed;
        Vector3 combinedMove = (forwardMove + scroll) * Time.deltaTime;

        Vector3 pos = transform.position;
        float targetX = Mathf.Clamp(xTarget, xMin, xMax);

        pos.x = Mathf.SmoothDamp(pos.x, targetX, ref xVelocity, smoothTime);
        transform.position = new Vector3(pos.x, pos.y, pos.z + combinedMove.z);

        if (pos.x <= xMin + 0.1f || pos.x >= xMax - 0.1f) {
            PickNewXTarget(avoidBorder: true);
            xChangeTimer = Random.Range(0.5f, 1.5f);
        }

        xChangeTimer -= Time.deltaTime;
        if (xChangeTimer <= 0f) {
            PickNewXTarget();
            xChangeTimer = Random.Range(1f, 3f);
        }

        if (Mathf.Approximately(transform.position.x, xTarget)) {
            PickNewXTarget(avoidBorder: false);
        }

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f) {
            Shoot();
            string enemyTag = gameObject.tag;
            if (enemyTag == "EnemyHeinz") {
                fireCooldown = 0.3f;
            }
            else {
                fireCooldown = Random.Range(1.3f, 2.1f);
            }
        }

        float playerZLimitMin = cam.position.z + 45f;

        if (transform.position.z < playerZLimitMin) {
            string enemyTag = gameObject.tag;
            if( enemyTag == "EnemyHeinz") {
                ScoreManager.Instance.AddScore(-100);
                FloatingScoreManager.Instance.SpawnScore(transform.position, -100);
            }
            else {
                ScoreManager.Instance.AddScore(-10);
                FloatingScoreManager.Instance.SpawnScore(transform.position, -10);
            }
            ObjectPool.Instance.ReturnToPool(enemyTag, gameObject);
        }
    }

    void PickNewXTarget(bool avoidBorder = false) {
        float currentX = transform.position.x;

        float newX;
        do {
            newX = Random.Range(xMin, xMax);
        } while (Mathf.Abs(newX - currentX) < 0.5f);

        if (avoidBorder) {
            if (currentX <= xMin + 0.1f)
                newX = currentX + Random.Range(2f, 5f);
            else if (currentX >= xMax - 0.1f)
                newX = currentX - Random.Range(2f, 5f);
        }

        xTarget = Mathf.Clamp(newX, xMin, xMax);
    }

    void Shoot() {
        int mode = Random.Range(0, 3);
        switch (mode) {
            case 0:
                int index = Random.Range(0, firePoints.Length);
                SpawnProjectile(firePoints[index]);
                break;
            case 1:
                foreach (var firePoint in firePoints) SpawnProjectile(firePoint);
                break;
            case 2:
                StartCoroutine(DelayedFire(firePoints[0], firePoints[1]));
                break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void SpawnProjectile(Transform firePoint) {
        var flippedRotation = Quaternion.Euler(0f, 180f, 0f);
        string enemyTag = gameObject.tag;
        var tagString = "EnemyProjectile";
        if (enemyTag == "EnemyHeinz") tagString = "HeinzProjectile";
        ObjectPool.Instance.SpawnFromPool(tagString, firePoint.position, flippedRotation);
    }

    IEnumerator DelayedFire(Transform fp1, Transform fp2) {
        SpawnProjectile(fp1);
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        SpawnProjectile(fp2);
    }
}