using UnityEngine;
using UnityEngine.UI;

public class TurretScrap : MonoBehaviour
{
    public GameObject turretPrefab;
    public int upgradeLevel;
    public int turretBaseCost;
    public int upgradeCost;

    public GameObject repairBarPrefab;
    private GameObject repairBarInstance;
    private Image repairBarFill;
    private float repairProgress = 0f;
    private float repairTime = 5f;
    private bool isRepairing = false;

    void Start()
    {
        if (repairBarPrefab != null)
        {
            repairBarInstance = Instantiate(repairBarPrefab);
            repairBarInstance.transform.SetParent(GameObject.Find("WorldUI").transform, false);
            repairBarFill = repairBarInstance.transform.Find("RepairBarFill")?.GetComponent<Image>();
            if (repairBarFill == null)
                Debug.LogError("RepairBarFill not found!");
            else
                repairBarFill.fillAmount = 0f;

            repairBarInstance.SetActive(false);
        }
    }

void Update()
{
    if (repairBarInstance != null)
    {
        repairBarInstance.transform.position = transform.position + Vector3.up * 3f;
        repairBarInstance.transform.LookAt(Camera.main.transform);
        repairBarInstance.transform.Rotate(0, 180, 0);
    }

    if (isRepairing)
    {
        repairProgress += Time.deltaTime / repairTime;
        Debug.Log($"Repairing... {repairProgress * 100f:F1}%"); // Show percentage

        if (repairBarFill != null)
            repairBarFill.fillAmount = repairProgress;

        if (repairProgress >= 1f)
        {
            CompleteRepair();
        }
    }
}

public void StartRepair()
{
    if (isRepairing) return; // prevent re-triggering every frame

    isRepairing = true;
    repairProgress = 0f;
    if (repairBarFill != null) repairBarFill.fillAmount = 0f;
    if (repairBarInstance != null) repairBarInstance.SetActive(true);
    
    Debug.Log("Started repairing turret...");
}

public void CancelRepair()
{
    isRepairing = false;
    repairProgress = 0f;
    if (repairBarFill != null) repairBarFill.fillAmount = 0f;
    if (repairBarInstance != null) repairBarInstance.SetActive(false);
    Debug.Log("Repair cancelled.");
}

void CompleteRepair()
{
    int repairCost = Mathf.RoundToInt(CalculateRepairCost() * 0.8f);
    if (GameManager.Instance.gold >= repairCost)
    {
        GameManager.Instance.SpendGold(repairCost);

        GameObject turret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        Turret turretScript = turret.GetComponent<Turret>();
        if (turretScript != null)
        {
            turretScript.upgradeLevel = upgradeLevel;
        }

        Debug.Log($"✅ Repair completed! Restored turret at level {upgradeLevel}.");

        Destroy(repairBarInstance);
        Destroy(gameObject);
    }
    else
    {
        Debug.Log("❌ Not enough gold to repair turret.");
        CancelRepair();
    }
}

    int CalculateRepairCost()
    {
        int cost = turretBaseCost;
        for (int i = 1; i <= upgradeLevel; i++)
        {
            cost += upgradeCost * i * 2; // same formula as upgrade
        }
                Debug.Log($"Repair cost: {cost}");

        return cost;
    }
}
