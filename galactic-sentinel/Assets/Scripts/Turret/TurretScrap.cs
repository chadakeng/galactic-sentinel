using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurretScrap : MonoBehaviour
{
    public GameObject turretPrefab;
    public int upgradeLevel;
    public int turretBaseCost;
    public int upgradeCost;
    public GameObject repairBarPrefab;
    public int repairTime;

    
    private GameObject repairBarInstance;
    private Image repairBarFill;
    private Coroutine repairCoroutine;
    private bool isRepairing = false;

    void Start()
    {
        if (repairBarPrefab != null)
        {
            GameObject canvas = GameObject.Find("WorldUI");
            if (canvas == null)
            {
                Debug.LogError("WorldUI Canvas NOT found in scene!");
                return;
            }

            repairBarInstance = Instantiate(repairBarPrefab, canvas.transform);
            repairBarInstance.transform.localScale = Vector3.one * 2f;
            repairBarInstance.transform.position = transform.position + Vector3.up * 3f;
            repairBarInstance.transform.rotation = Quaternion.LookRotation(repairBarInstance.transform.position - Camera.main.transform.position);

            repairBarFill = repairBarInstance.transform.Find("UpgradeBarBackground/UpgradeBarFill")?.GetComponent<Image>();

            if (repairBarFill == null)
            {
                Debug.LogError("RepairBarFill image not found in prefab!");
            }

            if (repairBarFill != null) repairBarFill.fillAmount = 0f;
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
    }

    public void StartRepair()
    {
        if (isRepairing) return;
        isRepairing = true;
        if (repairCoroutine != null) StopCoroutine(repairCoroutine);
        repairCoroutine = StartCoroutine(RepairProcess());
    }

    public void CancelRepair()
    {
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            repairCoroutine = null;
        }

        isRepairing = false;
        if (repairBarFill != null) repairBarFill.fillAmount = 0f;
        if (repairBarInstance != null) repairBarInstance.SetActive(false);
        Debug.Log("Repair cancelled.");
    }

    IEnumerator RepairProcess()
    {
        float progress = 0f;
        if (repairBarInstance != null) repairBarInstance.SetActive(true);

        while (progress < 1f)
        {
            if (!Input.GetKey(KeyCode.F))
            {
                CancelRepair();
                yield break;
            }

            progress += Time.deltaTime / repairTime;
            if (repairBarFill != null)
                repairBarFill.fillAmount = progress;

            yield return null;
        }

        CompleteRepair();
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
            cost += upgradeCost * i * 2;
        }
        Debug.Log($"Repair cost: {cost}");
        return cost;
    }
}
