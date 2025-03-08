using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Assign enemy prefab in the Inspector
    public float spawnRadius = 5f;  // How far enemies can spawn from the spawner
    public float spawnRate = 5f;  // Time between spawn cycles
    public int maxEnemiesPerWave = 5;  // Max enemies per wave
    private int currentEnemyCount = 0;  // Tracks total spawned enemies

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemies), 2f, spawnRate);
    }

    void SpawnEnemies()
    {

        // Random number of enemies (between 1 and maxEnemiesPerWave)
        int enemiesToSpawn = Random.Range(1, maxEnemiesPerWave + 1);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 randomSpawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity);

            currentEnemyCount++;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;  // Random point in a circle
        return new Vector3(
            transform.position.x + randomCircle.x,  // Use spawner's own position
            transform.position.y,
            transform.position.z + randomCircle.y
        );
    }
}