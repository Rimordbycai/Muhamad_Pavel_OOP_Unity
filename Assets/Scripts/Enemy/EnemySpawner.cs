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

    public bool isSpawning = false;

    private void Start()
    {
        spawnCount = defaultSpawnCount;// Initialize enemies left in the UI
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

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    public void OnDeath()
    {
        Debug.Log("Enemy Killed");
        totalKill++;
        ++totalKillWave;

        if (totalKillWave == minimumKillsToIncreaseSpawnCount)
        {
            Debug.Log("Increasing spawn count");
            totalKillWave = 0;
            defaultSpawnCount *= spawnCountMultiplier;
            
            if (spawnCountMultiplier < 3)
            {
                spawnCountMultiplier += multiplierIncreaseCount;
            }
            spawnCount = defaultSpawnCount;
        }
    }
}