using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();
        public float spawnInterval = 5f;
        public bool isBossWave = false;
    }

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public Transform spawnPoint;
    }

    public List<Wave> waves = new List<Wave>();
    public int currentWaveIndex = 0;

    private int enemiesRemainingInWave = 0;
    private bool isSpawning = false;

    void OnEnable()
    {
        BaseEnemy.OnEnemyDefeated += OnEnemyDefeated;
    }

    void OnDisable()
    {
        BaseEnemy.OnEnemyDefeated -= OnEnemyDefeated;
    }

    void Start()
    {
        StartCoroutine(StartWave(currentWaveIndex));
    }

    IEnumerator StartWave(int waveIndex)
    {
        Wave currentWave = waves[waveIndex];
        Debug.Log("Starting Wave: " + currentWave.waveName);

        isSpawning = true;
        enemiesRemainingInWave = currentWave.enemies.Count;

        // Spawn enemies in the wave
        foreach (EnemySpawnInfo spawnInfo in currentWave.enemies)
        {
            // Spawn enemy
            GameObject enemyObj = Instantiate(spawnInfo.enemyPrefab, spawnInfo.spawnPoint.position, Quaternion.identity);

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }

        isSpawning = false;

        // If it's a boss wave, wait for the boss to be defeated
        if (currentWave.isBossWave)
        {
            Debug.Log("Boss Wave Started");
        }
    }

    void OnEnemyDefeated(BaseEnemy enemy)
    {
        enemiesRemainingInWave--;

        // Check if wave is complete
        if (enemiesRemainingInWave <= 0 && !isSpawning)
        {
            WaveCompleted();
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");

        // Check if there are more waves
        if (currentWaveIndex < waves.Count - 1)
        {
            // Start the next wave
            currentWaveIndex++;
            StartCoroutine(StartWave(currentWaveIndex));
        }
        else
        {
            // All waves completed, level finished
            Debug.Log("All Waves Completed!");
            GameManager.Instance.CompleteLevel();
        }
    }
}