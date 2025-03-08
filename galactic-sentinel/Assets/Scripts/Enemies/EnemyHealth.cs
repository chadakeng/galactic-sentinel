using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public int goldReward = 20;
    private Renderer enemyRenderer;
    private Color originalColor;
    public float hitFlashDuration = 0.1f;

    public GameObject damageTextPrefab; // Assign DamageText prefab

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
        StartCoroutine(HitFlash()); // Flash red effect

        // Show damage text
        if (damageTextPrefab != null)
        {
            ShowDamageText(amount);
        }

        if (health <= 0)
        {
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

void ShowDamageText(float amount)
{
    if (damageTextPrefab != null)
    {
        GameObject canvas = GameObject.Find("DamageCanvas"); // Find world space canvas
        GameObject dmgText = Instantiate(damageTextPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity, canvas.transform);
        dmgText.GetComponent<FloatingDamage>().SetDamage(amount, transform); // Pass enemy position
    }
}

    void Die()
    {
        Destroy(gameObject);
    }
}