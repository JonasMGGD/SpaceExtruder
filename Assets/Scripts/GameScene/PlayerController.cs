using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player {
    public class PlayerController : MonoBehaviour {
        [Header("Movement")] public float moveSpeed = 20f;
        public Vector2 xLimits = new(-10f, 10f);
        public Vector2 zOffsetLimits = new(-5f, 5f);
        private Rigidbody rb;
        private Transform cam;
        private Vector3 offset = Vector3.zero;

        [Header("Shooting")] public Transform firePoint;
        public float fireRate = 0.2f;
        private float fireCooldown = 0f;

        [Header("Health")]
        private int maxHealth = 3;
        private int currentHealth;
        public Image[] heartImages;
        public float invulTime = 0.2f;
        private bool isInvulnerable;
        private Renderer[] renderers;

        [Header("References")]
        MusicManager manager;

        public void SetInvincible(bool active) { isInvulnerable = active; }

        public void SetRapidFire(bool active) { fireRate = active ? 0.05f : 0.2f; }

        public void Heal(int amount) {
            if(currentHealth == maxHealth) return;
            currentHealth += amount;
            UpdateHearts();
        }

        private void Start() {
            manager = FindFirstObjectByType<MusicManager>();
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            cam = Camera.main.transform;
            renderers = GetComponentsInChildren<Renderer>();

            currentHealth = maxHealth;
            UpdateHearts();
        }

        private void Update() {
            HandleInput();
            UpdatePosition();

            fireCooldown -= Time.deltaTime;
            if (Input.GetMouseButton(0) && fireCooldown <= 0f) {
                Shoot();
                fireCooldown = fireRate;
            }
        }

        private void HandleInput() {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 input = new Vector3(h, 0, v).normalized * (moveSpeed * Time.deltaTime);
            offset += input;

            offset.x = Mathf.Clamp(offset.x, xLimits.x, xLimits.y);
            offset.z = Mathf.Clamp(offset.z, zOffsetLimits.x, zOffsetLimits.y);
        }

        private void UpdatePosition() {
            Vector3 basePosition = cam.position + offset;
            basePosition.y = transform.position.y;
            rb.MovePosition(basePosition);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Shoot() {
            ObjectPool.Instance.SpawnFromPool("PlayerProjectile", firePoint.position, firePoint.rotation);
        }

        public void TakeDamage(int amount) {
            if (isInvulnerable) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHearts();

            if (currentHealth <= 0) {
                Die();
            }
            else {
                StartCoroutine(InvulnerabilityFlash());
            }
        }

        private void UpdateHearts() {
            for (int i = 0; i < heartImages.Length; i++) {
                heartImages[i].enabled = i < currentHealth;
            }
        }

        private void Die() {
            int finalScore = ScoreManager.Instance.GetScore();
            string name = PlayerPrefs.GetString("PlayerName", "Unknown");


            StartCoroutine(manager.MusicFade(0f, 0.7f));
            LeaderboardManager.Instance.AddScore(name, finalScore);
            PlayerPrefs.SetInt("LastScore", finalScore);
            Destroy(gameObject);

            Time.timeScale = 0f;
            SceneFader.Instance.FadeToScene("EndScene");
        }

        private IEnumerator InvulnerabilityFlash() {
            isInvulnerable = true;

            float elapsed = 0f;
            float flashInterval = 0.05f;
            bool visible = true;

            while (elapsed < invulTime) {
                visible = !visible;
                SetRendererVisibility(visible);
                yield return new WaitForSeconds(flashInterval);
                elapsed += flashInterval;
            }

            SetRendererVisibility(true);
            isInvulnerable = false;
        }

        private void SetRendererVisibility(bool visible) {
            foreach (Renderer rend in renderers) {
                rend.enabled = visible;
            }
        }
    }
}