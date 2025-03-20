using UnityEngine;
using UnityEngine.AI; 

public class FirewallPush : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushForce = 50f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PushEnemy(other);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PushEnemy(other);
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = true; 
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = false; 
            }
        }
    }

    void PushEnemy(Collider enemy)
    {
        Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
        if (enemyRb != null)
        {
            Vector3 pushDirection = (enemy.transform.position - transform.position).normalized;
            enemyRb.AddForce(pushDirection * pushForce);
        }
    }
}
