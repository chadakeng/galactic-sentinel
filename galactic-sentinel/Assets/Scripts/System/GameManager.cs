using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern

    public int gold = 10000; // Starting gold
    public GameObject turretPrefab;

    public int score = 0;
    public GameObject bonusTextPrefab;
    public TextMeshProUGUI healthGainPopupText;
    public TextMeshProUGUI gunDamagePopupText;
    public TextMeshProUGUI gunDamageLabelText;

    private int playerBonusDamage = 0;
    private int turretHealingBonus = 0;
    private int playerMaxHealthBonus = 0;

    private float scoreTimer = 0f;
    private PlayerHealth playerHealth; // Optional: if you have a player health system
    private PlayerShooting playerShooting; // Optional: if you have a player shooting system
public TextMeshProUGUI playerHealingLabelText;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>(); // Only works if PlayerHealth is in the scene
        playerShooting = FindFirstObjectByType<PlayerShooting>(); // Only works if PlayerShooting is in the scene
    }

    void Update()
    {
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            score += 1;
            scoreTimer = 0f;
        }
        if (gunDamageLabelText != null && playerShooting != null)
        {
            float totalDamage = playerShooting.GetBaseDamage() + playerBonusDamage;
            gunDamageLabelText.text = $"Gun Damage: {totalDamage}";

            float totalHealing = playerShooting.GetBaseHealing() + turretHealingBonus;
            playerHealingLabelText.text = $"Turret Healing: {totalHealing}";
            
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold: " + gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log("Gold spent! Remaining: " + gold);
            return true;
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false;
        }
    }

    public void OnEnemyKilled()
    {
        playerBonusDamage += 1;
        turretHealingBonus += 1;
        playerMaxHealthBonus += 1;
        score += 5;

        Debug.Log($"Enemy killed! ➕DMG+{playerBonusDamage}, ➕Heal+{turretHealingBonus}, ➕HP+{playerMaxHealthBonus}");
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(5);

            // Trigger popups
            if (healthGainPopupText != null)
            {
                healthGainPopupText.GetComponent<FloatingStats>().Show("+5 HP");
            }

            if (gunDamagePopupText != null)
            {
                gunDamagePopupText.GetComponent<FloatingStats>().Show("+1");
            }

            if (gunDamageLabelText != null)
            {
                float totalDamage = playerShooting.GetBaseDamage() + playerBonusDamage;
                gunDamageLabelText.text = $"Gun Damage: {totalDamage}";
            }
        }
    }

    public int GetPlayerBonusDamage() => playerBonusDamage;
    public int GetTurretHealingBonus() => turretHealingBonus;
    public int GetPlayerMaxHealthBonus() => playerMaxHealthBonus;
}