using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public int maxHealth = 3;
    private int currentHealth;

    public Image[] heartImages;

    public float invulTime = 0.2f;
    private bool isInvulnerable = false;

    private Renderer[] renderers;

    private void Start() {
        currentHealth = maxHealth;
        renderers = GetComponentsInChildren<Renderer>();
        UpdateHearts();
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
        Debug.Log("Player dead");
        gameObject.SetActive(false);
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