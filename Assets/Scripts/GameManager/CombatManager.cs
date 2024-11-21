using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public EnemySpawner[] enemySpawners; // Array of spawners
    public float timer = 0; // Timer to track wave intervals
    [SerializeField] private float waveInterval = 5f; // Time between waves
    public int waveNumber = 1; // Current wave number
    public int totalEnemies = 0; // Total enemies spawned

    private void Start()
    {
        StartWave();
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
                spawner.defaultSpawnCount = waveNumber; // Increase spawn count per wave
                spawner.spawnCountMultiplier = 1; // Reset multiplier
                spawner.isSpawning = true;
            }
        }
    }

    private void StartNextWave()
    {
        waveNumber++;
        totalEnemies = 0;

        foreach (var spawner in enemySpawners)
        {
            if (spawner != null)
            {
                spawner.defaultSpawnCount = waveNumber; // Adjust spawn count
                spawner.multiplierIncreaseCount = waveNumber; // Increase difficulty
                spawner.isSpawning = true;
            }
        }
    }

    private bool AllSpawnersFinished()
    {
        foreach (var spawner in enemySpawners)
        {
            if (spawner != null && spawner.isSpawning)
            {
                return false; // If any spawner is still active, waves aren't finished
            }
        }
        return true; // All spawners are inactive, wave is finished
    }

    public void RegisterKill()
    {
        totalEnemies++;
    }
}

