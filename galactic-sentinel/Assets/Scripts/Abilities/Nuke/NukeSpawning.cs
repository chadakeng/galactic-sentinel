using UnityEngine;
using System.Collections;  
public class NukeSpawner : MonoBehaviour
{
    [Header("Nuke Setup")]
    public GameObject nukePrefab;      
    public float nukeDropDelay = 0.5f;   
    public float spawnHeight = 10f;   
    public float forwardDistance = 5f; 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(DropNuke());
        }
    }

    IEnumerator DropNuke()
    {
        yield return new WaitForSeconds(nukeDropDelay);
        Vector3 spawnPos = transform.position
                           + transform.forward * forwardDistance
                           + Vector3.up * spawnHeight;
        Instantiate(nukePrefab, spawnPos, Quaternion.identity);
    }
}
