using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera;
    public float shootRange = 50f;
    public float damage = 20f;
    public float baseHealing = 10f;
    public LayerMask enemyLayer;
    public LayerMask turretPlatformLayer;
    public AudioSource gunSound;
    public TextMeshProUGUI interactionPromptText;
    private PlayerHealth playerHealth;

    public float fireRate = 0.2f;
    private float nextTimeToShoot = 0f;

    public int turretCost = 50;
    public GameObject upgradePromptUI; // <-- Add this for UI
    private RaycastHit hit;
    private bool isLookingAtTurret = false;
    private bool isLookingAtScrap = false;
    private bool isLookingAtPlatform = false;

    void Start()
    {
            playerHealth = FindObjectOfType<PlayerHealth>();

    }
    void Update()
    {
        PerformRaycast();

        // Show [F] Upgrade when looking at turret
        upgradePromptUI.SetActive(isLookingAtTurret);

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToShoot)
        {
            nextTimeToShoot = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetMouseButton(1))
        {
            TryRepairTurret();
        }

        if (isLookingAtTurret)
        {
            Turret turret = hit.collider.GetComponent<Turret>();

            if (Input.GetKeyDown(KeyCode.F))
            {
                turret.StartUpgrade();
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                turret.CancelUpgrade();
            }
        }

        if (isLookingAtScrap)
        {
            TurretScrap scrap = hit.collider.GetComponent<TurretScrap>();
            if (Input.GetKey(KeyCode.F))
            {
                scrap?.StartRepair();
            }
            else
            {
                scrap?.CancelRepair();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Replacing scrap with new turret");
                ReplaceScrapWithNewTurret();
            }
        }

        if (isLookingAtPlatform && Input.GetKeyDown(KeyCode.F))
        {
            TrySpawnTurret();
        }
        TryHealPlayer();
    }

void PerformRaycast()
{
    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    isLookingAtTurret = false;
    isLookingAtScrap = false;
    isLookingAtPlatform = false;

    interactionPromptText.text = ""; // Reset prompt every frame

    if (Physics.Raycast(ray, out hit, shootRange))
    {
        if (hit.collider.TryGetComponent(out Turret turret))
        {
            isLookingAtTurret = true;
            int cost = turret.GetUpgradeCost(); // Add this method to Turret.cs if you haven't already
            interactionPromptText.text = $"[F] - Upgrade {cost}g\n[MB2] - Heal";
        }
        else if (hit.collider.TryGetComponent(out TurretScrap scrap))
        {
            isLookingAtScrap = true;
            float repairCost = scrap.GetRepairCost(); // You’ll need to expose this
            interactionPromptText.text = $"[F] - Repair {repairCost}g\n[R] - New Turret {turretCost}g";
        }
        else if (hit.collider.TryGetComponent<TurretPlatform>(out var platform) && platform.isActive)
        {
            isLookingAtPlatform = true;
            interactionPromptText.text = $"[F] - Buy Turret {turretCost}g";
        }
    }
}

    void Shoot()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out hit, shootRange, enemyLayer))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                int totalDamage = (int)damage + GameManager.Instance.GetPlayerBonusDamage();
                enemy.TakeDamage(totalDamage);
            }
        }
        if (gunSound != null) gunSound.Play();
    }

    void TrySpawnTurret()
    {
        if (GameManager.Instance.gold < turretCost) return;

        if (hit.collider.GetComponent<TurretPlatform>() != null)
        {
            hit.collider.GetComponent<TurretPlatform>().SpawnTurret();
            GameManager.Instance.gold -= turretCost;
        }
    }
    void TryHealPlayer()
{
    if (playerHealth != null && Input.GetKey(KeyCode.LeftShift))
    {
        float healing = baseHealing + GameManager.Instance.GetTurretHealingBonus();
        playerHealth.Heal(healing * Time.deltaTime); // Heal over time while holding Shift
    }
}

    void TryRepairTurret()
    {
        if (hit.collider.GetComponent<TurretHealth>() != null)
        {
            float healing = baseHealing + GameManager.Instance.GetTurretHealingBonus();
            hit.collider.GetComponent<TurretHealth>()?.Heal(healing);
        }
    }

    void ReplaceScrapWithNewTurret()
    {
        if (GameManager.Instance.gold < turretCost) return;

        TurretScrap scrap = hit.collider.GetComponent<TurretScrap>();
        if (scrap != null)
        {
            GameManager.Instance.SpendGold(turretCost);
            Vector3 spawnPosition = scrap.transform.position + Vector3.up * 0.5f;
            Destroy(scrap.gameObject);
            Instantiate(GameManager.Instance.turretPrefab, spawnPosition, Quaternion.identity);
        }
    }
    public float GetBaseDamage()
    {
        return damage;
    }
    public float GetBaseHealing(){
        return baseHealing;
    }
}