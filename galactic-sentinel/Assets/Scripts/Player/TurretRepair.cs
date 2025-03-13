using UnityEngine;
using System.Collections;

public class TurretRepair : MonoBehaviour
{
    public float repairAmountPerSecond = 10f;
    public float repairRange = 15f;
    public ParticleSystem repairEffect; // Yellow particles with + signs
    public LayerMask turretLayer;
    
    private TurretHealth targetTurret;

    void Update()
    {
        if (Input.GetMouseButton(1)) // Right click
        {
            FindTurretToRepair();
        }
    }

    void FindTurretToRepair()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, repairRange, turretLayer))
        {
            TurretHealth turret = hit.collider.GetComponent<TurretHealth>();
            if (turret != null)
            {
                StartCoroutine(RepairTurret(turret));
            }
        }
    }

    IEnumerator RepairTurret(TurretHealth turret)
    {
        targetTurret = turret;
        ParticleSystem effect = Instantiate(repairEffect, turret.transform.position, Quaternion.identity);
        
        while (Input.GetMouseButton(1) && turret != null && turret.gameObject.activeSelf)
        {
            turret.Heal(repairAmountPerSecond * Time.deltaTime);
            yield return null;
        }

        Destroy(effect.gameObject);
    }
}