using System;
using System.Collections;
using Score;
using Sounds;
using StartScene;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene {
    public class PlayerController : MonoBehaviour {
        [Header("Movement")] public float moveSpeed = 20f;
        public Vector2 xLimits = new(-10f, 10f);
        public Vector2 zOffsetLimits = new(-5f, 5f);
        private Rigidbody _rb;
        private Transform _cam;
        private Vector3 _offset = Vector3.zero;

        [Header("Shooting")] public Transform firePoint;
        public float fireRate = 0.2f;
        private float _fireCooldown;

        [Header("Health")] public int maxHealth;
        private int _currentHealth;
        public Slider healthSlider;
        private const float InvulTime = 0.2f;
        private bool _isInvulnerable;
        private Renderer[] _renderers;

        [Header("Audio")] private MusicManager _manager;
        public AudioClip shootSound;
        public AudioClip deathSound;
        public AudioClip hitSound;
        private float _shootSoundCooldownDefault = 0.2f;
        private float _shootSoundCooldown = 0.2f;

        public void SetInvincible(bool active) {
            _isInvulnerable = active;
        }

        public void SetRapidFire(bool active) {
            fireRate = active ? 0.05f : 0.2f;
            _shootSoundCooldownDefault = active ? 0.1f : 0.2f;
        }

        private void Start() {
            maxHealth = PlayerPrefs.GetInt("Health", 10);
            Debug.Log("Max Health: " + maxHealth);
            _manager = FindFirstObjectByType<MusicManager>();
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;

            _cam = Camera.main.transform;
            _renderers = GetComponentsInChildren<Renderer>();

            _currentHealth = maxHealth;
            healthSlider.maxValue = PlayerPrefs.GetInt("Health", maxHealth);
            healthSlider.value = _currentHealth;
            UpdateHealthBar();
        }

        public void Heal(int amount) {
            if (_currentHealth == maxHealth) return;
            _currentHealth += amount;
            UpdateHealthBar();
        }

        private void Update() {
            HandleInput();
            UpdatePosition();

            _fireCooldown -= Time.deltaTime;
            _shootSoundCooldown -= Time.deltaTime;
            if (!Input.GetMouseButton(0) || !(_fireCooldown <= 0f)) return;
            Shoot();
            if (_shootSoundCooldown <= 0f) {
                SoundEffectsManager.instance.PlaySoundFX(shootSound, transform, "SFXVolume");
                _shootSoundCooldown = _shootSoundCooldownDefault;
            }

            _fireCooldown = fireRate;
        }

        private void HandleInput() {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            var input = new Vector3(h, 0, v).normalized * (moveSpeed * Time.deltaTime);
            _offset += input;

            _offset.x = Mathf.Clamp(_offset.x, xLimits.x, xLimits.y);
            _offset.z = Mathf.Clamp(_offset.z, zOffsetLimits.x, zOffsetLimits.y);
        }

        private void UpdatePosition() {
            var basePosition = _cam.position + _offset;
            basePosition.y = transform.position.y;
            _rb.MovePosition(basePosition);
        }

        private void UpdateHealthBar() {
            healthSlider.value = _currentHealth;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Shoot() {
            ObjectPool.instance.SpawnFromPool("PlayerProjectile", firePoint.position, firePoint.rotation);
        }

        public void TakeDamage(int amount) {
            if (_isInvulnerable) return;
            _currentHealth -= amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            if (_currentHealth <= 0) Die();
            else {
                StartCoroutine(InvulnerabilityFlash());
                SoundEffectsManager.instance.PlaySoundFX(hitSound, transform, "SFXVolume");
            }
            UpdateHealthBar();
        }

        private void Die() {
            var finalScore = ScoreManager.instance.GetScore();
            var name = PlayerPrefs.GetString("PlayerName", "Unknown");

            ObjectPool.instance.SpawnFromPool("ExplosionEffect", transform.position, Quaternion.identity);
            SoundEffectsManager.instance.PlaySoundFX(deathSound, transform, "SFXVolume");
            StartCoroutine(_manager.MusicFade(0f, 0.7f));
            LeaderboardManager.instance.AddScore(name, finalScore);
            PlayerPrefs.SetInt("LastScore", finalScore);
            Destroy(gameObject);

            Time.timeScale = 0f;
            SceneFader.instance.FadeToScene("EndScene");
        }

        private IEnumerator InvulnerabilityFlash() {
            _isInvulnerable = true;

            var elapsed = 0f;
            var flashInterval = 0.05f;
            var visible = true;

            while (elapsed < InvulTime) {
                visible = !visible;
                SetRendererVisibility(visible);
                yield return new WaitForSeconds(flashInterval);
                elapsed += flashInterval;
            }

            SetRendererVisibility(true);
            _isInvulnerable = false;
        }

        private void SetRendererVisibility(bool visible) {
            foreach (Renderer rend in _renderers) {
                rend.enabled = visible;
            }
        }
    }
}