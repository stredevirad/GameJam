using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Rhythm Game/Level Data")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class EnemyWave
    {
        public string waveName;
        public GameObject[] enemyPrefabs;
        public Transform[] spawnPoints;
        public float spawnInterval = 2f;
        public bool isBossWave = false;
    }

    public string levelName;
    public int levelNumber;
    public MusicManager.MusicTrack levelMusic;
    public float baseBPM = 120f;
    public EnemyWave[] waves;
    public GameObject bossPrefab;
    public string nextLevelName;

    [Header("Difficulty Settings")]
    public float enemyHealthMultiplier = 1f;
    public float enemyDamageMultiplier = 1f;
    public float playerHealthMultiplier = 1f;
}