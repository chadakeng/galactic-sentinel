using UnityEngine;
using UnityEngine.UI;

public class TurretHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public ParticleSystem destroyEffect;
    
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;
    private Image healthBarFill;

    public GameObject scrapPrefab; // Assign TurretScrap prefab in Inspector
    private Turret turretComponent; // Reference to get upgrade level

    void Start()
    {
        currentHealth = maxHealth;
        turretComponent = GetComponent<Turret>();
        InitializeHealthUI();
    }

    public void InitializeHealthUI()
    {
        if (healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 3f, Quaternion.identity);
            healthBarFill = healthBarInstance.transform.Find("HealthBarBackground/HealthBarFill").GetComponent<Image>();
        }
    }

    void Update()
    {
        if (healthBarInstance != null)
        {
            healthBarInstance.transform.position = transform.position + Vector3.up * 3.5f;
            healthBarInstance.transform.LookAt(Camera.main.transform);
            healthBarInstance.transform.Rotate(0, 180, 0);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
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
        }
    }

    void Die()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        SpawnScrap();
        Destroy(healthBarInstance);
        Destroy(gameObject);
    }

    void SpawnScrap()
    {
        if (scrapPrefab == null) return;

        GameObject scrap = Instantiate(scrapPrefab, transform.position, Quaternion.identity);
        TurretScrap scrapData = scrap.GetComponent<TurretScrap>();

        if (scrapData != null && turretComponent != null)
        {
            scrapData.repairCost = turretComponent.turretBaseCost + (turretComponent.upgradeCost * turretComponent.upgradeLevel);
            scrapData.previousUpgradeCost = turretComponent.upgradeCost * turretComponent.upgradeLevel;
        }
    }
}