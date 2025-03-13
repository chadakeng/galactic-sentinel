using UnityEngine;

public class TurretHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public ParticleSystem destroyEffect; // Assign in Inspector
    private TurretPlatform platform; // The platform this turret belongs to
    public GameObject damageTextPrefab; // Assign the floating text prefab in Inspector
void Start()
{
    currentHealth = maxHealth;
    
    // Find the turret platform below this turret
    if (platform == null)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            platform = hit.collider.GetComponent<TurretPlatform>();
        }
    }
}

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        SpawnFloatingText(amount, false); // False = damage (red text)

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        SpawnFloatingText(amount, true); // True = healing (green text)
    }

    void SpawnFloatingText(float amount, bool isHealing)
    {
        if (damageTextPrefab == null) return; // Prevent errors

        GameObject damageTextObj = Instantiate(damageTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        FloatingDamage floatingText = damageTextObj.GetComponent<FloatingDamage>();
        floatingText.SetDamage(amount, transform, isHealing);
    }

    void Die()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        // Reactivate platform so player can build again
        if (platform != null)
        {
            platform.isActive = true;
        }

        Destroy(gameObject);
    }
}