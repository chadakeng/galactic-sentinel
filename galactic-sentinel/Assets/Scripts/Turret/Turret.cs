using UnityEngine;

public class Turret : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject); // Destroy turret when health reaches 0
        }
    }
}