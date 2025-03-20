using UnityEngine;

public class FirewallEffect : MonoBehaviour
{
    [Header("Firewall Effect Settings")]
    public float stopDuration = 2f;
    public float damagePerSecond = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement enemy = other.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.StopMovement(stopDuration);
            }
        }
    }
}
