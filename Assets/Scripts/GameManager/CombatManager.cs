using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public EnemySpawner[] enemySpawners; 
    public float timer = 0; // Timer to track wave intervals
    [SerializeField] private float waveInterval = 5f; // Time between waves
    public int waveNumber = 0; // Current wave number
    public int totalEnemies = 0; // Total enemies to spawn in current wave
    public int totalPoints = 0;

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
        if (AllSpawnersFinished() && totalEnemies <= 0)
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

        foreach (EnemySpawner spawner in enemySpawners)
        {
            if (spawner != null)
            {
                totalEnemies += spawner.spawnCount; // Add spawn count of each spawner
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
        totalEnemies--; // Decrease the count of remaining enemies
        totalPoints += enemyLevel;  // Add points based on the enemy level
        Debug.Log($"Enemies left: {totalEnemies}");
    }
}
