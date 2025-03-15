using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Pool;
using System;

public class RhythmGameManager : MonoBehaviour
{
    public AudioSource musicSource;
    public float beatInterval = 1.0f;
    public GameObject[] normalEnemies;
    public GameObject[] bossEnemies;
    public Transform enemySpawnPoint;
    public KeyCode parryKey = KeyCode.Space;
    public AudioClip parrySuccessSound;
    public AudioClip parryFailSound;
    public GameObject parryEffectPrefab;
    public PlayerController player;
    public GameManager gameManager;
    public UIManager uiManager;
    public AudioManager audioManager;
    private ObjectPool<GameObject> enemyPool;
    private float nextBeatTime;
    private bool canParry;
    private AudioSource audioSource;
    private int enemiesSpawned;
    private float parryCooldown = 0.2f;
    private float lastParryTime;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<PlayerController>(); // Ensuring player is assigned

        if (gameManager == null || player == null)
        {
            Debug.LogError("GameManager or PlayerController is missing in the scene!");
            return;
        }

        nextBeatTime = Time.time + beatInterval;
        audioManager.PlayLevelMusic(gameManager.currentLevel);
        audioSource = GetComponent<AudioSource>();

        float trackBPM = audioManager.GetTrackBPM();
        beatInterval = 60f / Mathf.Clamp(trackBPM, 80f, 200f);
        enemiesSpawned = 0;

        enemyPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(normalEnemies[0], enemySpawnPoint.position, Quaternion.identity),
            actionOnGet: (enemy) => enemy.SetActive(true),
            actionOnRelease: (enemy) => enemy.SetActive(false),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    void Update()
    {
        if (Time.time >= nextBeatTime)
        {
            SpawnEnemyAttack();
            nextBeatTime += beatInterval;
            canParry = true;
            uiManager.UpdateBeatVisualizer();
        }

        if (canParry && Input.GetKeyDown(parryKey) && Time.time - lastParryTime > parryCooldown)
        {
            lastParryTime = Time.time;
            float timingOffset = Mathf.Abs(Time.time - (nextBeatTime - beatInterval));

            if (timingOffset < 0.15f)
            {
                Instantiate(parryEffectPrefab, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(parrySuccessSound);
                canParry = false;
                player.PerformParry(true);
                player.Advance();
                gameManager.EnemyDefeated();
            }
            else
            {
                audioSource.PlayOneShot(parryFailSound);
                player.PerformParry(false);
                gameManager.PlayerDamaged();
                uiManager.UpdateHealthBar(player.health);
            }
        }
    }

    void SpawnEnemyAttack()
    {
        int levelIndex = Mathf.Clamp(gameManager.currentLevel - 1, 0, normalEnemies.Length - 1);
        GameObject enemyPrefab = (enemiesSpawned < gameManager.enemiesToDefeat - 1)
            ? normalEnemies[levelIndex]
            : bossEnemies[Mathf.Clamp(levelIndex, 0, bossEnemies.Length - 1)];

        // Get the enemy from the pool or instantiate it
        GameObject enemyAttack = enemyPool.Get();
        if (enemyAttack == null)
        {
            Debug.LogError("Enemy prefab is missing or Object Pool failed to provide an object!");
            return;
        }

        enemyAttack.SetActive(true); // Ensure enemy is activated
        enemyAttack.transform.position = enemySpawnPoint.position;

        // Ensure the enemy has the required script
        Enemy enemy = enemyAttack.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Enemy script is missing from the prefab!");
            return;
        }

        // Initialize the enemy with level settings
        enemy.Initialize(gameManager.currentLevel);
        enemiesSpawned++;
    }

    public void ReleaseEnemy(GameObject enemy)
    {
        enemyPool.Release(enemy);
    }
}
