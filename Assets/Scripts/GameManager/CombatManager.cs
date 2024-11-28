using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public EnemySpawner[] enemySpawners;
    public GameStats gameStats;  // Array of spawners
    public float timer = 0; // Timer to track wave intervals
    [SerializeField] private float waveInterval = 5f; // Time between waves
    public int waveNumber = 1; // Current wave number
    public int totalEnemiesDefeated = 0;
    private int totalEnemiesInCurrentWave = 0; // Total enemies to spawn in current wave
    private int totalPoints = 0; // Total enemies defeated across all waves

    private void Start()
    {
        waveNumber = 0;
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.combatManager = this;
        }

        gameStats.UpdateWave(waveNumber);
        gameStats.UpdateEnemiesLeft(totalEnemiesDefeated);
        gameStats.UpdatePoints(totalPoints);
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
    private void StartNextWave()
    {
        timer = 0;
        waveNumber++;
        totalEnemiesInCurrentWave = 0;
        totalEnemiesLeftInWave = 0; // Reset total enemies for new wave

        foreach (EnemySpawner spawner in enemySpawners)
        {
            if (spawner != null)
            {
                totalEnemiesInCurrentWave += spawner.spawnCount; // Add spawn count of each spawner
            }
        }
        
        gameStats.UpdateWave(waveNumber);
        gameStats.UpdateEnemiesLeft(totalEnemiesInCurrentWave);

        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            if (enemySpawner != null)
            {
                enemySpawner.StartSpawning(); // Restart spawners
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

    public void RegisterKill(int enemyLevel)
    {
        totalEnemiesDefeated++;
        totalPoints += enemyLevel;  // Add points based on the enemy level
        // Optionally, handle logic here for when a kill impacts gameplay globally.
        Debug.Log($"Total enemies defeated: {totalEnemiesDefeated}");
        gameStats.UpdatePoints(totalPoints);
    }
}
