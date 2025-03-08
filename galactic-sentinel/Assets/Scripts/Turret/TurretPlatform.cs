using UnityEngine;

public class TurretPlatform : MonoBehaviour
{
    public GameObject turretPrefab;
    public bool isActive = true;

    public void SpawnTurret()
    {
        if (!isActive) return;

        GameObject newTurret = Instantiate(turretPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Debug.Log("Turret spawned in TurretPlatform");
        isActive = false;
    }
}