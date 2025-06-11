using System.Collections;
using StartScene;
using UnityEngine;
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
        public int maxHealth;
        private int currentHealth;
        public Slider healthSlider;
        public float invulTime = 0.2f;
        private bool isInvulnerable;
        private Renderer[] renderers;

        [Header("Audio")]
        private MusicManager manager;
        public AudioClip shootSound;
        public AudioClip deathSound;
        private float shootSoundCooldownDefault = 0.2f;
        private float shootSoundCooldown = 0.2f;

        public void SetInvincible(bool active) { isInvulnerable = active; }

        public void SetRapidFire(bool active) {
            fireRate = active ? 0.05f : 0.2f;
            shootSoundCooldownDefault = active ? 0.1f : 0.2f;
        }

        private void Start() {
            maxHealth = PlayerPrefs.GetInt("Health", 10);
            Debug.Log("Max Health: " + maxHealth);
            manager = FindFirstObjectByType<MusicManager>();
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            cam = Camera.main.transform;
            renderers = GetComponentsInChildren<Renderer>();

            currentHealth = maxHealth;
            healthSlider.maxValue = PlayerPrefs.GetInt("Health", maxHealth);
            healthSlider.value = currentHealth;
            UpdateHealthBar();
        }

        public void Heal(int amount) {
            if(currentHealth == maxHealth) return;
            currentHealth += amount;
            UpdateHealthBar();
        }

        private void Update() {
            HandleInput();
            UpdatePosition();

            fireCooldown -= Time.deltaTime;
            shootSoundCooldown -= Time.deltaTime;
            if (Input.GetMouseButton(0) && fireCooldown <= 0f) {
                Shoot();
                if (shootSoundCooldown <= 0f) {
                    SoundEffectsManager.instance.PlaySoundFX(shootSound, transform, "SFXVolume");
                    shootSoundCooldown = shootSoundCooldownDefault;
                }
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

        private void UpdateHealthBar() {
            healthSlider.value = currentHealth;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Shoot() {
            ObjectPool.Instance.SpawnFromPool("PlayerProjectile", firePoint.position, firePoint.rotation);
        }

        public void TakeDamage(int amount) {
            if (isInvulnerable) return;

            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthBar();

            if (currentHealth <= 0) Die();
            else StartCoroutine(InvulnerabilityFlash());
        }

        private void Die() {
            int finalScore = ScoreManager.Instance.GetScore();
            string name = PlayerPrefs.GetString("PlayerName", "Unknown");

            SoundEffectsManager.instance.PlaySoundFX(deathSound, transform, "SFXVolume");
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