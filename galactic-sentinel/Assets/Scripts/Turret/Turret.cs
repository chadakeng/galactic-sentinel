using UnityEngine;
using System.Collections;
using System.Linq;
public class Turret : MonoBehaviour
{
    public float health = 100f;
    public float range = 10f; // Shooting range
    public float fireRate = 0.2f; // Machine gun speed
    public float damagePerShot = 5f; // Instant hitscan damage
    public ParticleSystem muzzleFlash; // Assign in Inspector
    private float nextFireTime = 0f;


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject); // Destroy turret when health reaches 0
        }
    }


    void Update()
    {
        EnemyMovement targetEnemy = FindClosestEnemy();

        if (targetEnemy != null)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot(targetEnemy);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    EnemyMovement FindClosestEnemy()
    {
        EnemyMovement[] enemies = FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);
        return enemies.OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
                      .FirstOrDefault(e => Vector3.Distance(transform.position, e.transform.position) <= range);
    }

    void Shoot(EnemyMovement target)
    {
        // Play muzzle flash
        if (muzzleFlash != null) muzzleFlash.Play();
        // Apply instant damage (hitscan)
        target.GetComponent<EnemyHealth>().TakeDamage(damagePerShot);
    }
}
