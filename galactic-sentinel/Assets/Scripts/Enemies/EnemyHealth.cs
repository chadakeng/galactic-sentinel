using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public int goldReward = 20;
    private Renderer enemyRenderer;
    private Color originalColor;
    public float hitFlashDuration = 0.1f;

    public GameObject damageTextPrefab; // Assign FloatingDamage prefab in Inspector

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }
public void TakeDamage(float amount)
{
    health -= amount;
    Debug.Log($"{gameObject.name} took {amount} damage! Current HP: {health}"); 

    StartCoroutine(HitFlash());

    // Show floating damage text
    if (damageTextPrefab != null)
    {
        SpawnFloatingText(amount);
    }

    if (health <= 0)  // Ensure it triggers at exactly 0
    {
        health = 0;
        Debug.Log($"{gameObject.name} should now DIE! Calling Die()..."); 
        GameManager.Instance.AddGold(goldReward);
        Die(); 
    }
}

    IEnumerator HitFlash()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = Color.red;
            yield return new WaitForSeconds(hitFlashDuration);
            enemyRenderer.material.color = originalColor;
        }
    }

void SpawnFloatingText(float amount)
{
    Debug.Log("Spawning Floating Text: " + amount);

    if (damageTextPrefab == null)
    {
        Debug.LogError("DamageTextPrefab is NOT assigned in Inspector!");
        return;
    }

    GameObject worldCanvas = GameObject.Find("WorldUI");
    if (worldCanvas == null)
    {
        Debug.LogError("WorldUI Canvas NOT found in scene!");
        return;
    }

    Vector3 spawnOffset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(1f, 1.5f), 0);

    GameObject damageTextObj = Instantiate(damageTextPrefab, transform.position + spawnOffset, Quaternion.identity, worldCanvas.transform);
    
    if (damageTextObj == null)
    {
        Debug.LogError("Failed to instantiate DamageTextPrefab!");
        return;
    }

    FloatingDamage floatingText = damageTextObj.GetComponent<FloatingDamage>();

    if (floatingText == null)
    {
        Debug.LogError("FloatingDamage script is missing on Floating Damage Text prefab!");
        return;
    }

    floatingText.SetDamage(amount, transform, false);
}

    void Die()
    {
        Destroy(gameObject);
    }
}