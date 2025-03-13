using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Fast, Regular }
    public EnemyType enemyType;

    public float attackRange = 2f; // Distance to attack
    public float attackRate = 1f; // Time between attacks
    public int damage = 10; // Damage per attack

    private NavMeshAgent agent;
    private Transform playerCore; // The Player (Core)
    private Turret currentTarget;
    private float nextAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError(gameObject.name + " is missing a NavMeshAgent!");
        }

        playerCore = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerCore == null)
        {
            Debug.LogError("Player (Core) not found! Make sure the Player has the 'Player' tag.");
        }

        if (enemyType == EnemyType.Fast)
        {
            agent.SetDestination(playerCore.position); // Fast enemies go directly to core
        }
    }

    void Update()
    {
        if (enemyType == EnemyType.Regular)
        {
            FindClosestTarget(); // Always check for the closest target

            if (currentTarget != null)
            {
                float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (distance <= attackRange)
                {
                    Attack(currentTarget.gameObject);
                    agent.isStopped = true;  // Stop moving when attacking
                }
                else
                {
                    agent.SetDestination(currentTarget.transform.position);
                    agent.isStopped = false;
                }
            }
            else
            {
                agent.SetDestination(playerCore.position);
            }
        }
    }

    void FindClosestTarget()
    {
        Turret[] turrets = FindObjectsByType<Turret>(FindObjectsSortMode.None);
        float closestDistance = float.MaxValue;
        Turret closestTurret = null;

        foreach (Turret turret in turrets)
        {
            if (turret == null || turret.health <= 0) continue; // Ignore destroyed turrets

            float distance = Vector3.Distance(transform.position, turret.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTurret = turret;
            }
        }

        if (closestTurret != null)
        {
            currentTarget = closestTurret;
            agent.SetDestination(currentTarget.transform.position); // Move to the closest turret
        }
        else
        {
            currentTarget = null;
            agent.SetDestination(playerCore.position); // No turrets left, move to core
        }
    }

void Attack(GameObject target)
{
    if (Time.time >= nextAttackTime)
    {
        if (target.CompareTag("Player"))
        {
            Debug.Log("Attacking Player!");
        }
        else if (target.GetComponent<TurretHealth>() != null)
        {
            Debug.Log("Attacking turret: " + target.name);
            target.GetComponent<TurretHealth>().TakeDamage(damage);
        }
        nextAttackTime = Time.time + attackRate;
    }
}
}