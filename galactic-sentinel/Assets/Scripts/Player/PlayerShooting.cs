using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera; 
    public float shootRange = 50f;
    public float damage = 20f;
    public LayerMask enemyLayer;
    public LayerMask turretPlatformLayer;

    public int turretCost = 50;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click to Shoot
        {
            Shoot();
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
        if (!GameManager.Instance.SpendGold(turretCost)) return; // Not enough gold

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootRange, turretPlatformLayer))
        {
            TurretPlatform platform = hit.collider.GetComponent<TurretPlatform>();
            if (platform != null && platform.isActive)
            {
                platform.SpawnTurret();
            }
            else
            {
                Debug.Log("Turret already placed here!");
            }
        }
    }
}