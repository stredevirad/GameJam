using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI levelText;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;
    public GameObject pausePanel;

    [Header("Beat Visualization")]
    public BeatVisualizer beatVisualizer;

    private PlayerController player;
    private BaseEnemy currentEnemy;
    private int score = 0;
    private int combo = 0;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentEnemy = FindObjectOfType<BaseEnemy>();

        // Initialize UI elements
        UpdateHealthBars();
        UpdateScoreUI();
        UpdateLevelUI();

        // Subscribe to events
        PlayerController.OnHealthChanged += OnPlayerHealthChanged;
        BaseEnemy.OnEnemyDefeated += OnEnemyDefeated;

        // Hide panels
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        PlayerController.OnHealthChanged -= OnPlayerHealthChanged;
        BaseEnemy.OnEnemyDefeated -= OnEnemyDefeated;
    }

    void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Update enemy health bar if we have an enemy
        if (currentEnemy != null && enemyHealthBar != null)
        {
            enemyHealthBar.value = (float)currentEnemy.currentHealth / currentEnemy.maxHealth;
        }
    }

    void OnPlayerHealthChanged(int currentHealth, int maxHealth)
    {
        UpdateHealthBars();
    }

    void OnEnemyDefeated(BaseEnemy enemy)
    {
        // Update score
        score += 100 * combo;
        combo++;

        UpdateScoreUI();

        // Find the next enemy
        currentEnemy = FindObjectOfType<BaseEnemy>();
    }

    void UpdateHealthBars()
    {
        if (player != null && playerHealthBar != null)
        {
            playerHealthBar.value = (float)player.currentHealth / player.maxHealth;
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        if (comboText != null)
        {
            comboText.text = "Combo: x" + combo.ToString();
        }
    }

    void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + GameManager.Instance.currentLevel.ToString();
        }
    }

    void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            // Unpause
            Time.timeScale = 1;
            if (pausePanel != null) pausePanel.SetActive(false);
        }
        else
        {
            // Pause
            Time.timeScale = 0;
            if (pausePanel != null) pausePanel.SetActive(true);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void ShowLevelComplete()
    {
        if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
    }

    // UI Button callbacks
    public void OnRestartButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.RestartLevel();
    }

    public void OnNextLevelButton()
    {
        Time.timeScale = 1;
        GameManager.Instance.NextLevel();
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}