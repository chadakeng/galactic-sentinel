using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 200f;
    private float currentHealth;

    public GameObject gameOverUI; // Optional: drag your Game Over UI here
    public Image healthBarFill;   // Drag "HealthbarFill" here in the Inspector

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player took {amount} damage. Current health: {currentHealth}");

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");

        if (gameOverUI != null) gameOverUI.SetActive(true);

        gameObject.SetActive(false); // Disable the player
    }
}