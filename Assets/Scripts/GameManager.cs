using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    public int currentLevel = 1;
    public int maxLevels = 5;
    public float difficultyMultiplier = 1.25f; // How much harder each level gets

    [Header("References")]
    public PlayerController player;
    public BeatDetector beatDetector;
    public GameObject levelCompletePanel;
    public GameObject gameOverPanel;

    private bool isGameOver = false;
    private bool isLevelComplete = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }

    void Start()
    {
        StartLevel(currentLevel);
    }

    public void StartLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        isGameOver = false;
        isLevelComplete = false;

        // Reset player health
        if (player != null)
            player.ResetHealth();

        // Hide UI panels
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Load level scene
        SceneManager.LoadScene("Level" + levelNumber);

        // Adjust difficulty based on level
        AdjustDifficulty();

        Debug.Log("Starting Level " + levelNumber);
    }

    void AdjustDifficulty()
    {
        // Calculate difficulty modifier for current level
        float difficulty = Mathf.Pow(difficultyMultiplier, currentLevel - 1);

        // Increase BPM for higher levels
        if (beatDetector != null)
        {
            beatDetector.bpm = 120f + (currentLevel - 1) * 10f;
        }

        Debug.Log("Level " + currentLevel + " Difficulty: " + difficulty + ", BPM: " + beatDetector.bpm);
    }

    public void CompleteLevel()
    {
        if (isGameOver || isLevelComplete) return;

        isLevelComplete = true;

        Debug.Log("Level Completed!");

        // Show level complete UI
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        // Check if we've completed all levels
        if (currentLevel >= maxLevels)
        {
            Debug.Log("Game Completed!");
            // Handle game completion
        }
    }

    void HandlePlayerDeath(int currentHealth, int maxHealth)
    {
        if (isGameOver || isLevelComplete) return;

        isGameOver = true;

        Debug.Log("Game Over!");

        // Show game over UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        StartLevel(currentLevel);
    }

    public void NextLevel()
    {
        if (currentLevel < maxLevels)
        {
            StartLevel(currentLevel + 1);
        }
        else
        {
            // Return to main menu or show end game screen
            SceneManager.LoadScene("MainMenu");
        }
    }
}