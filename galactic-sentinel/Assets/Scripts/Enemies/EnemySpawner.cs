using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // Assign multiple spawn points
    public float spawnRate = 5f;
    public int maxEnemies = 10;

    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnRate);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        currentEnemyCount++;
    }
}