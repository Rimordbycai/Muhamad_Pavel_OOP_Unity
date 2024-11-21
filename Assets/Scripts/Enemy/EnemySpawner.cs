using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public Enemy spawnedEnemy;

    [SerializeField] private int minimumKillsToIncreaseSpawnCount = 3; // Minimum kills to increase spawn count
    public int totalKill = 0; // Total kills by the player
    private int totalKillWave = 0; // Kills within the current wave

    [SerializeField] private float spawnInterval = 3f; // Time between spawns

    [Header("Spawned Enemies Counter")]
    public int spawnCount = 0; // Remaining enemies to spawn in the current wave
    public int defaultSpawnCount = 1; // Base spawn count
    public int spawnCountMultiplier = 1; // Spawn scaling multiplier
    public int multiplierIncreaseCount = 1; // Amount to increase the multiplier by

    public CombatManager combatManager; // Reference to the CombatManager

    public bool isSpawning = false; // Whether spawning is active

    private void Start()
    {
        spawnCount = defaultSpawnCount; // Initialize spawn count
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
        // Only start spawning if the enemy's level is less than or equal to the current wave number
        if (spawnedEnemy.Level <= combatManager.waveNumber)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }

    public IEnumerator SpawnEnemies()
    {
        if (isSpawning)
        {
            // If spawn count is 0, reset it to the default spawn count
            if (spawnCount == 0)
            {
                spawnCount = defaultSpawnCount;
            }

            int enemiesToSpawn = spawnCount;
            while (enemiesToSpawn > 0)
            {
                // Instantiate the enemy
                Enemy enemy = Instantiate(spawnedEnemy);
                enemy.GetComponent<Enemy>().enemySpawner = this;
                enemy.GetComponent<Enemy>().combatManager = combatManager; // Set the combat manager reference
                enemiesToSpawn--;
                spawnCount = enemiesToSpawn;

                // Increment the total enemies count in the CombatManager
                if (combatManager != null)
                {
                    combatManager.totalEnemiesDefeated++;
                }

                yield return new WaitForSeconds(spawnInterval); // Wait before spawning the next enemy
            }
        }
    }

    public void OnDeath()
    {
        Debug.Log("Enemy Killed");
        totalKill++; // Increment total kills
        totalKillWave++; // Increment kills in the current wave
        // Check if total kills in the wave have reached the threshold to increase spawn count
        if (totalKillWave >= minimumKillsToIncreaseSpawnCount)
        {
            Debug.Log("Increasing spawn count");
            totalKillWave = 0; // Reset the wave kill counter
            defaultSpawnCount *= spawnCountMultiplier; // Increase default spawn count
            spawnCount = defaultSpawnCount; // Update spawn count for the spawner

            // Increment spawn count multiplier for scaling, with a limit
            if (spawnCountMultiplier < 3)
            {
                spawnCountMultiplier += multiplierIncreaseCount;
            }
        }
    }
}