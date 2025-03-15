using UnityEngine;
using UnityEngine.UI;

public class TurretScrap : MonoBehaviour
{
    public int repairCost;
    public int previousUpgradeCost;

    public GameObject repairBarPrefab;
    private GameObject repairBarInstance;
    private Image repairBarFill;
    private float repairProgress = 0f;
    private float repairTime = 3f;

    void Start()
    {
        if (repairBarPrefab != null)
        {
            repairBarInstance = Instantiate(repairBarPrefab, transform.position + Vector3.up * 3f, Quaternion.identity);
            repairBarFill = repairBarInstance.transform.Find("RepairBarFill").GetComponent<Image>();
            repairBarInstance.SetActive(false);
        }
    }

    public void RepairScrap()
    {
        repairProgress += Time.deltaTime / repairTime;
        repairBarFill.fillAmount = repairProgress;

        if (repairProgress >= 1f)
        {
            Debug.Log($"Turret Repaired for {repairCost} Gold!");
            if (GameManager.Instance.gold >= repairCost)
            {
                GameManager.Instance.SpendGold(repairCost);
                
                GameObject newTurret = Instantiate(GameManager.Instance.turretPrefab, transform.position, Quaternion.identity);
                TurretHealth turretHealth = newTurret.GetComponent<TurretHealth>();
                
                if (turretHealth != null)
                {
                    turretHealth.InitializeHealthUI();
                }

                Destroy(repairBarInstance);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Not enough gold to repair the turret!");
            }
        }
    }
}