using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern

    public int gold = 10000; // Starting gold
    public GameObject turretPrefab;

    public int score = 0;
    public GameObject bonusTextPrefab;
public Transform playerTransform; // Assign this in the Inspector (your Player's transform)

    private int playerBonusDamage = 0;
    private int turretHealingBonus = 0;
    private int playerMaxHealthBonus = 0;

    private float scoreTimer = 0f;
    private PlayerHealth player; // Optional: if you have a player health system
    

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
        player = FindFirstObjectByType<PlayerHealth>(); // Only works if PlayerHealth is in the scene
    }

    void Update()
    {
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= 1f)
        {
            score += 1;
            scoreTimer = 0f;
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

        if (player != null)
        {
            player.IncreaseMaxHealth(5); // Make sure this method exists in PlayerHealth
        }
    }

    public int GetPlayerBonusDamage() => playerBonusDamage;
    public int GetTurretHealingBonus() => turretHealingBonus;
    public int GetPlayerMaxHealthBonus() => playerMaxHealthBonus;
}