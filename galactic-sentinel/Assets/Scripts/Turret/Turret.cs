using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Turret : MonoBehaviour
{
    public float health = 100f;
    public float range = 10f;
    public float fireRate = 0.2f;
    public float damagePerShot = 5f;
    public ParticleSystem muzzleFlash;

    private float nextFireTime = 0f;
    public int turretBaseCost = 50;
    public int upgradeCost = 20;
    public int upgradeLevel = 0;
    public int maxUpgradeLevel = 5;

    public GameObject upgradeBarPrefab;
    private GameObject upgradeBarInstance;
    private Image upgradeBarFill;
    private Coroutine upgradeCoroutine = null;
    private bool isUpgrading = false;

    void Start()
    {
        InitializeUpgradeUI();
    }

    void Update()
    {
        EnemyMovement targetEnemy = FindClosestEnemy();
        if (targetEnemy != null && Time.time >= nextFireTime)
        {
            Shoot(targetEnemy);
            nextFireTime = Time.time + fireRate;
        }

        if (upgradeBarInstance != null)
        {
            upgradeBarInstance.transform.position = transform.position + Vector3.up * 3.5f; // Adjust height here
            upgradeBarInstance.transform.LookAt(Camera.main.transform);
            upgradeBarInstance.transform.Rotate(0, 180, 0);
        }
    }

    public void InitializeUpgradeUI()
    {
        if (upgradeBarPrefab != null)
        {
            GameObject canvas = GameObject.Find("WorldUI");
            if (canvas == null)
            {
                Debug.LogError("WorldUI Canvas NOT found in scene!");
                return;
            }

            upgradeBarInstance = Instantiate(upgradeBarPrefab, canvas.transform);
            upgradeBarInstance.transform.localScale = Vector3.one * 2f;
            upgradeBarInstance.transform.position = transform.position + Vector3.up * 10f;

            upgradeBarInstance.transform.rotation = Quaternion.LookRotation(upgradeBarInstance.transform.position - Camera.main.transform.position);

            upgradeBarFill = upgradeBarInstance.transform.Find("UpgradeBarBackground/UpgradeBarFill").GetComponent<Image>();

            if (upgradeBarFill == null)
            {
                Debug.LogError("UpgradeBarFill image not found in prefab!");
            }

            upgradeBarInstance.SetActive(false);
        }
    }

    public void StartUpgrade()
    {
        int currentUpgradeCost = GetUpgradeCost();
        if (isUpgrading || upgradeLevel >= maxUpgradeLevel || GameManager.Instance.gold < upgradeCost) return;

        isUpgrading = true;
        upgradeCoroutine = StartCoroutine(UpgradeProcess());
    }

    public void CancelUpgrade()
    {
        if (upgradeCoroutine != null)
        {
            StopCoroutine(upgradeCoroutine);
            upgradeCoroutine = null;
        }

        isUpgrading = false;

        if (upgradeBarFill != null)
        {
            upgradeBarFill.fillAmount = 0;
        }

        if (upgradeBarInstance != null)
        {
            upgradeBarInstance.SetActive(false);
        }
    }

    IEnumerator UpgradeProcess()
    {
        isUpgrading = true;
        float upgradeTime = Mathf.Min(1f + upgradeLevel, 5f);

        if (upgradeBarInstance != null)
        {
            upgradeBarInstance.SetActive(true);
        }

        float progress = 0f;
        while (progress < 1f)
        {
            if (!Input.GetKey(KeyCode.F))
            {
                CancelUpgrade();
                yield break;
            }

            progress += Time.deltaTime / upgradeTime;
            if (upgradeBarFill != null)
                upgradeBarFill.fillAmount = progress;
            yield return null;
        }

        CompleteUpgrade();
    }

    void CompleteUpgrade()
    {
        int currentUpgradeCost = GetUpgradeCost();
        if (GameManager.Instance.gold >= upgradeCost)
        {
            GameManager.Instance.gold -= currentUpgradeCost; ;
            upgradeLevel++;
            fireRate *= 1.05f; // 5% increase in fire rate per upgrade
            damagePerShot = 5f * Mathf.Pow(1.1f, upgradeLevel); // 10% increase in damage per upgrade
            Debug.Log($"Turret upgraded to Level {upgradeLevel}!");
        }
        else
        {
            Debug.Log("Not enough gold to complete the upgrade!");
        }

        isUpgrading = false;
        upgradeCoroutine = null;

        if (upgradeBarInstance != null)
        {
            upgradeBarInstance.SetActive(false);
        }
    }

    EnemyMovement FindClosestEnemy()
    {
        EnemyMovement[] enemies = FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);
        return enemies.Length > 0 ? enemies[0] : null;
    }

    void Shoot(EnemyMovement target)
    {
        if (muzzleFlash != null) muzzleFlash.Play();
        target.GetComponent<EnemyHealth>().TakeDamage(damagePerShot);
    }
    int GetUpgradeCost()
    {
        return turretBaseCost + (upgradeCost * upgradeLevel * 2); // Doubles cost each level
    }
}