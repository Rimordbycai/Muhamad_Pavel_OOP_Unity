using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy spawnedEnemy;
    [SerializeField] private int minimumKillsToIncreaseSpawnCount = 3;
    public int totalKill = 0;
    private int totalKillWave = 0;

    [SerializeField] private float spawnInterval = 3f;
    public int spawnCount = 0;
    public int defaultSpawnCount = 1;
    public int spawnCountMultiplier = 1;
    public int multiplierIncreaseCount = 1;

    public CombatManager combatManager;
    public GameStats gameStats; // Reference to GameStats to update UI

    public bool isSpawning = false;

    private void Start()
    {
        spawnCount = defaultSpawnCount;
        gameStats.UpdateEnemiesLeft(spawnCount); // Initialize enemies left in the UI
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void StartSpawning()
    {
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
            if (spawnCount == 0)
            {
                spawnCount = defaultSpawnCount;
            }

            int enemiesToSpawn = spawnCount;

            while (enemiesToSpawn > 0)
            {
                Enemy enemy = Instantiate(spawnedEnemy);
                enemy.GetComponent<Enemy>().enemySpawner = this;
                enemy.GetComponent<Enemy>().combatManager = combatManager;

                enemiesToSpawn--;
                spawnCount = enemiesToSpawn;

                // Register the spawned enemy in the CombatManager
                //combatManager.RegisterKill(enemy.Level);

                yield return new WaitForSeconds(spawnInterval);
            }

            StopSpawning();
        }
    }

    public void OnDeath()
    {
        Debug.Log("Enemy Killed");
        totalKill++;
        totalKillWave++;

        if (totalKillWave >= minimumKillsToIncreaseSpawnCount)
        {
            Debug.Log("Increasing spawn count");
            totalKillWave = 0;
            defaultSpawnCount *= spawnCountMultiplier;
            spawnCount = defaultSpawnCount;

            if (spawnCountMultiplier < 3)
            {
                spawnCountMultiplier += multiplierIncreaseCount;
            }
        }

        combatManager.RegisterKill(spawnedEnemy.Level);
        gameStats.UpdatePoints(totalKill); // Update points in the UI
    }
}