using UnityEngine;

public class DifficultyBalancer : MonoBehaviour
{
    [Header("Difficulty Scaling")]
    public AnimationCurve healthScaling;
    public AnimationCurve damageScaling;
    public AnimationCurve bpmScaling;

    [Header("Level Settings")]
    public int maxLevel = 10;

    private GameManager gameManager;
    private BeatDetector beatDetector;

    void Start()
    {
        gameManager = GameManager.Instance;
        beatDetector = FindObjectOfType<BeatDetector>();

        // Apply difficulty settings for current level
        ApplyDifficultyForLevel(gameManager.currentLevel);
    }

    public void ApplyDifficultyForLevel(int level)
    {
        // Normalize level to 0-1 range for curve evaluation
        float normalizedLevel = Mathf.Clamp01((float)(level - 1) / (maxLevel - 1));

        // Calculate scaling factors
        float healthScale = healthScaling.Evaluate(normalizedLevel);
        float damageScale = damageScaling.Evaluate(normalizedLevel);
        float bpmScale = bpmScaling.Evaluate(normalizedLevel);

        Debug.Log("Level " + level + " Difficulty Scaling - Health: " + healthScale + ", Damage: " + damageScale + ", BPM: " + bpmScale);

        // Apply BPM scaling
        if (beatDetector != null)
        {
            float baseBPM = 120f;
            beatDetector.bpm = baseBPM * bpmScale;
        }

        // Find all enemies and modify their stats
        BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();
        foreach (BaseEnemy enemy in enemies)
        {
            enemy.maxHealth = Mathf.RoundToInt(enemy.maxHealth * healthScale);
            enemy.attackDamage = Mathf.RoundToInt(enemy.attackDamage * damageScale);
            enemy.currentHealth = enemy.maxHealth;
        }

        //