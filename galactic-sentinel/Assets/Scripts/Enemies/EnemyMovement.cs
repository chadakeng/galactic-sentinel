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
            agent.SetDestination(playerCore.position);
        }
    }

    void Update()
    {
        if (enemyType == EnemyType.Regular)
        {
            FindClosestTarget();

            if (currentTarget != null)
            {
                float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (distance <= attackRange)
                {
                    Attack(currentTarget.gameObject);
                    agent.isStopped = true;
                }
                else
                {
                    agent.SetDestination(currentTarget.transform.position);
                    agent.isStopped = false;
                }
            }
        }
    }

    void FindClosestTarget()
    {
        Turret[] turrets = FindObjectsByType<Turret>(FindObjectsSortMode.None);
        Transform closestTarget = playerCore;
        float closestDistance = Vector3.Distance(transform.position, playerCore.position);

        foreach (Turret turret in turrets)
        {
            float distance = Vector3.Distance(transform.position, turret.transform.position);
            if (distance < closestDistance && turret.health > 0)
            {
                closestDistance = distance;
                closestTarget = turret.transform;
            }
        }

        if (closestTarget == playerCore)
        {
            currentTarget = null;
        }
        else
        {
            currentTarget = closestTarget.GetComponent<Turret>();
        }

        agent.SetDestination(closestTarget.position);
    }

    void Attack(GameObject target)
    {
        if (Time.time >= nextAttackTime)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Attacking Player!");
            }
            else if (target.GetComponent<Turret>() != null)
            {
                target.GetComponent<Turret>().TakeDamage(damage);
            }
            nextAttackTime = Time.time + attackRate;
        }
    }
}