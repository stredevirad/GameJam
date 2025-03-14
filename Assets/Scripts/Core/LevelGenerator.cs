using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public LevelData levelData;
    public GameObject enemyContainer;
    public BeatDetector beatDetector;
    public MusicManager musicManager;

    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("No level data assigned to LevelGenerator");
            return;
        }

        // Set up music
        if (musicManager != null)
        {
            musicManager.LoadTrackForLevel(levelData.levelNumber - 1);
        }

        // Set up beat detector
        if (beatDetector != null)
        {
            beatDetector.bpm = levelData.baseBPM;
        }

        // Set up wave manager
        EnemyWaveManager waveManager = GetComponent<EnemyWaveManager>();
        if (waveManager != null)
        {
            List<EnemyWaveManager.Wave> waves = new List<EnemyWaveManager.Wave>();

            // Convert level data waves to wave manager waves
            foreach (LevelData.EnemyWave dataWave in levelData.waves)
            {
                EnemyWaveManager.Wave wave = new EnemyWaveManager.Wave();
                wave.waveName = dataWave.waveName;
                wave.spawnInterval = dataWave.spawnInterval;
                wave.isBossWave = dataWave.isBossWave;

                List<EnemyWaveManager.EnemySpawnInfo> spawnInfos = new List<EnemyWaveManager.EnemySpawnInfo>();

                for (int i = 0; i < dataWave.enemyPrefabs.Length; i++)
                {
                    EnemyWaveManager.EnemySpawnInfo spawnInfo = new EnemyWaveManager.EnemySpawnInfo();
                    spawnInfo.enemyPrefab = dataWave.enemyPrefabs[i];
                    spawnInfo.spawnPoint = dataWave.spawnPoints[i % dataWave.spawnPoints.Length];
                    spawnInfos.Add(spawnInfo);
                }

                wave.enemies = spawnInfos;
                waves.Add(wave);
            }

            // Add boss wave if there's a boss
            if (levelData.bossPrefab != null)
            {
                EnemyWaveManager.Wave bossWave = new EnemyWaveManager.Wave();
                bossWave.waveName = "Boss Wave";
                bossWave.spawnInterval = 2f;
                bossWave.isBossWave = true;

                EnemyWaveManager.EnemySpawnInfo bossSpawnInfo = new EnemyWaveManager.EnemySpawnInfo();
                bossSpawnInfo.enemyPrefab = levelData.bossPrefab;
                bossSpawnInfo.spawnPoint = transform; // Use level generator as spawn point

                bossWave.enemies = new List<EnemyWaveManager.EnemySpawnInfo> { bossSpawnInfo };
                waves.Add(bossWave);
            }

            waveManager.waves = waves;
        }
    }
}