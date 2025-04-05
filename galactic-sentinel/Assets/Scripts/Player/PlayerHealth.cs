using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 200f;
    private float currentHealth;

    public GameObject gameOverUI;
    public Image healthBarFill;
    public TextMeshProUGUI healthText;
void Start()
{
    currentHealth = maxHealth;

    if (healthBarFill != null)
    {
        healthBarFill.fillAmount = 1f;
    }

    UpdateHealthText();
}
void UpdateHealthText()
{
    if (healthText != null)
    {
        healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
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
        UpdateHealthText();
    }

    void Die()
    {
        Debug.Log("Player Died!");

        if (gameOverUI != null) gameOverUI.SetActive(true);

        gameObject.SetActive(false); // Disable the player
    }
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        Debug.Log($"Player max health increased to {maxHealth}");
        UpdateHealthText();
    }
    public void Heal(float amount)
{
    currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    Debug.Log($"Player healed: {amount}. Current health: {currentHealth}");

    if (healthBarFill != null)
        healthBarFill.fillAmount = currentHealth / maxHealth;

    UpdateHealthText();
}
    
}