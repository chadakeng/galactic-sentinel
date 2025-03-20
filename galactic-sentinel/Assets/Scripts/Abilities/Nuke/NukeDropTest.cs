using UnityEngine;
using System.Collections;

public class NukeDropTest : MonoBehaviour
{
    [Header("Nuke Settings")]
    public GameObject nukePrefab;    
    public float nukeDropDelay = 2f; 
    public float spawnHeight = 10f;
    public float forwardDistance = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 dropPosition = transform.position;
            StartCoroutine(DropNuke(dropPosition));
        }
    }

    IEnumerator DropNuke(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(nukeDropDelay);
        Vector3 spawnPos = targetPosition + Vector3.up * spawnHeight;
        Instantiate(nukePrefab, spawnPos, Quaternion.identity);
    }
}
