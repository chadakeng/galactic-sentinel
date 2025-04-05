using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float baseSpawnRate = 30f;
    public float spawnRadius = 5f;
    public int maxEnemiesPerWave = 5;

    private float spawnTimer = 0f;
    private float currentSpawnRate;
    private int baseEnemyHealth = 100;
    private int baseEnemyDamage = 10;

    void Start()
    {
        currentSpawnRate = baseSpawnRate;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        int difficultyMultiplier = GameManager.Instance.score / 200;

        // Update spawn rate (decreases by 0.2s every 100 score, min 1s)
        currentSpawnRate = Mathf.Max(1f, baseSpawnRate - difficultyMultiplier * 0.1f);

        if (spawnTimer >= currentSpawnRate)
        {
            SpawnEnemies(difficultyMultiplier);
            spawnTimer = 0f;
        }
    }

    void SpawnEnemies(int difficultyMultiplier)
    {
        int enemiesToSpawn = Random.Range(1, maxEnemiesPerWave + 1);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.health = baseEnemyHealth + (10 * difficultyMultiplier);
                enemyHealth.damage = baseEnemyDamage + (10 * difficultyMultiplier);
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 circle = Random.insideUnitCircle * spawnRadius;
        return new Vector3(transform.position.x + circle.x, transform.position.y, transform.position.z + circle.y);
    }
}