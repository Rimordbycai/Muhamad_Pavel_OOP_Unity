using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public EnemySpawner[] enemySpawners; // Array of spawners
    public float timer = 0; // Timer to track wave intervals
    [SerializeField] private float waveInterval = 5f; // Time between waves
    public int waveNumber = 1; // Current wave number
    public int totalEnemiesDefeated = 0; // Total enemies defeated across all waves

    private void Start()
    {
        waveNumber = 0;
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.combatManager = this;
        }
    }

    private void Update()
    {
        if (AllSpawnersFinished())
        {
            timer += Time.deltaTime;

            // Check if it's time for the next wave
            if (timer >= waveInterval)
            {
                StartNextWave();
                timer = 0;
            }
        }
    }

    private void StartWave()
    {
        foreach (var spawner in enemySpawners)
        {
            if (spawner != null)
            {
                spawner.defaultSpawnCount = waveNumber; // Set spawn count based on wave number
                spawner.spawnCountMultiplier = 1; // Reset multiplier for new wave
                spawner.isSpawning = true; // Activate spawner
            }
        }
    }

    private void StartNextWave()
    {
        timer = 0;
        waveNumber++;
        totalEnemiesDefeated = 0; // Reset enemy defeat count for the new wave

        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            if (enemySpawner != null)
            {
                enemySpawner.defaultSpawnCount = waveNumber; // Adjust spawn count for the wave
                enemySpawner.spawnCountMultiplier = waveNumber; // Increase spawn rate or difficulty
                enemySpawner.isSpawning = true; // Restart spawners
            }
        }
    }

    private bool AllSpawnersFinished()
    {
        foreach (var spawner in enemySpawners)
        {
            if (spawner != null && spawner.isSpawning)
            {
                return false; // If any spawner is still active, wave isn't finished
            }
        }
        return true; // All spawners have finished spawning
    }

    public void RegisterKill()
    {
        totalEnemiesDefeated++;
        // Optionally, handle logic here for when a kill impacts gameplay globally.
        Debug.Log($"Total enemies defeated: {totalEnemiesDefeated}");
    }
}
