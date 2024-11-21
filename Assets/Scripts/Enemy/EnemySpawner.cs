using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public Enemy spawnedEnemy;

    [SerializeField] private int minimumKillsToIncreaseSpawnCount = 3;
    public int totalKill = 0;
    private int totalKillWave = 0;

    [SerializeField] private float spawnInterval = 3f;

    [Header("Spawned Enemies Counter")]
    public int spawnCount = 0;
    public int defaultSpawnCount = 1;
    public int spawnCountMultiplier = 1;
    public int multiplierIncreaseCount = 1;

    public CombatManager combatManager;

    public bool isSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (isSpawning)
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    SpawnEnemy();
                }

                totalKillWave = 0;
                while (totalKillWave < spawnCount)
                {
                    yield return null; // Wait for the current wave's kills
                }

                // Check if it's time to increase the spawn count
                if (totalKill >= minimumKillsToIncreaseSpawnCount * multiplierIncreaseCount)
                {
                    spawnCountMultiplier++;
                    spawnCount = defaultSpawnCount * spawnCountMultiplier;
                    multiplierIncreaseCount++;
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnedEnemy != null)
        {
            Enemy newEnemy = Instantiate(spawnedEnemy, transform.position, Quaternion.identity);

            // Activate the enemy after a delay using the method provided in Enemy
            StartCoroutine(newEnemy.ActivateAfterDelay(0f)); // Adjust the delay as needed
        }
        else
        {
            Debug.LogError("No enemy prefab assigned!");
        }
    }

    // Method to start spawning
    public void StartSpawning()
    {
        isSpawning = true;
    }

    // Method to stop spawning
    public void StopSpawning()
    {
        isSpawning = false;
    }
}