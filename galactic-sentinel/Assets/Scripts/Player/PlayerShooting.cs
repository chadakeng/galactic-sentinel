using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera; 
    public float shootRange = 50f;
    public float damage = 20f;
    public LayerMask enemyLayer;
    public LayerMask turretPlatformLayer;
        
    public float fireRate = 0.2f; // Delay between shots
    private float nextTimeToShoot = 0f; // Tracks when player can shoot next


    public int turretCost = 50;

    void Update()
    {
        if (Input.GetMouseButton(0)) // Hold Left Click to continuously shoot
        {
            if (Time.time >= nextTimeToShoot)
            {
                nextTimeToShoot = Time.time + fireRate;
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) // Press 'F' to buy and place a turret
        {
            TrySpawnTurret();
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootRange, enemyLayer))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    void TrySpawnTurret()
    {
        if (GameManager.Instance.gold <= turretCost) return; // Not enough gold

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootRange, turretPlatformLayer))
        {
            TurretPlatform platform = hit.collider.GetComponent<TurretPlatform>();
            if (platform != null && platform.isActive)
            {
                Debug.Log("Turret placed in playershooting");
                platform.SpawnTurret();
            }
            else
            {
                Debug.Log("Turret already placed here!");
            }
        }
    }
}