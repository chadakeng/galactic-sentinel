using UnityEngine;
using UnityEngine.UI;

public class TurretHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public ParticleSystem destroyEffect;
    private TurretPlatform platform;

    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Image healthBarFill;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);

            healthBarInstance.transform.localScale = Vector3.one * 0.5f; // Adjust size here

            healthBarFill = healthBarInstance.transform.Find("HealthBarBackground/HealthBarFill").GetComponent<Image>();

            if (healthBarFill == null)
            {
                Debug.LogError("❌ HealthBarFill Image not found inside prefab!");
            }
        }
    }

    void Update()
    {
        if (healthBarInstance != null)
        {
            // Move health bar above the turret
            healthBarInstance.transform.position = transform.position + Vector3.up * 3.5f; // Adjust this value

            // Ensure health bar faces the player
            healthBarInstance.transform.LookAt(Camera.main.transform);
            healthBarInstance.transform.Rotate(0, 180, 0);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth); // Prevent health from going negative
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }


void UpdateHealthBar()
{
    if (healthBarFill != null)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        Debug.Log($"🔴 Health Updated! Current Health: {currentHealth}, Fill Amount: {healthBarFill.fillAmount}");
    }
}

    void Die()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        if (platform != null)
        {
            platform.isActive = true;
        }

        Destroy(healthBarInstance);
        Destroy(gameObject);
    }
}