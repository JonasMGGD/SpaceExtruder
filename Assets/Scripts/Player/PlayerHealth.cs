using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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

    void Die() {
        int finalScore = ScoreManager.Instance.GetScore();
        Debug.Log(finalScore);
        string name = PlayerPrefs.GetString("PlayerName", "Unknown");

        LeaderboardManager.Instance.AddScore(name, finalScore);
        PlayerPrefs.SetInt("LastScore", finalScore);

        SceneManager.LoadScene("EndScene");
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