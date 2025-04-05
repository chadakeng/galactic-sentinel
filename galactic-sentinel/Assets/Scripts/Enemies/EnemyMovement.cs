using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Fast, Regular }
    public EnemyType enemyType;

    public float attackRange = 2f;
    public float attackRate = 1f;
    public int damage = 10;

    private NavMeshAgent agent;
    private GameObject currentTarget;
    private float nextAttackTime = 0f;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (agent == null) Debug.LogError($"{gameObject.name} is missing a NavMeshAgent!");
        if (animator == null) Debug.LogError($"{gameObject.name} is missing an Animator!");
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
                    if (!agent.isStopped)
                    {
                        agent.isStopped = true;
                        agent.ResetPath();
                    }

                    Attack(currentTarget);
                }
                else
                {
                    if (agent.isStopped) agent.isStopped = false;

                    agent.SetDestination(currentTarget.transform.position);
                }

                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }
    }

    void FindClosestTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Turret[] turrets = FindObjectsByType<Turret>(FindObjectsSortMode.None);
        // if turret still present target turret else target player
        if (turrets.Length > 0)
        {
            currentTarget = turrets[0].gameObject;
            float closestDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
            foreach (Turret turret in turrets)
            {
                float distance = Vector3.Distance(transform.position, turret.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = turret.gameObject;
                }
            }
            agent.SetDestination(currentTarget.transform.position);
        }
        else
        {
            currentTarget = player;
            if (player != null)
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }

    void Attack(GameObject target)
    {
        if (Time.time >= nextAttackTime)
        {
            if (target.CompareTag("Player"))
            {
                target.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
            else if (target.GetComponent<TurretHealth>() != null)
            {
                target.GetComponent<TurretHealth>().TakeDamage(damage);
            }

            nextAttackTime = Time.time + attackRate;
        }
    }
}