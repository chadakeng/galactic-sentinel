using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public int goldReward = 20; // Gold earned when killed

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            GameManager.Instance.AddGold(goldReward);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject); 
    }
}