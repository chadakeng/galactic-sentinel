using UnityEngine;
using System.Collections;

public class FirewallSpawn : MonoBehaviour
{
    [Header("Firewall Settings")]
    public GameObject firewallPrefab;      
    public float firewallForwardDistance = 5f; 
    public float firewallSpawnHeight = 1f;   
    public float firewallLifeTime = 5f;     

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            FireFirewall();
        }
    }

    void FireFirewall()
    {
        Vector3 spawnPos = transform.position
                           + transform.forward * firewallForwardDistance
                           + Vector3.up * firewallSpawnHeight;
        GameObject firewall = Instantiate(firewallPrefab, spawnPos, Quaternion.LookRotation(transform.forward));
        Destroy(firewall, firewallLifeTime);
    }
}
