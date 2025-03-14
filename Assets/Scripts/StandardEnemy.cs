using UnityEngine;

public class StandardEnemy : BaseEnemy
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;

    protected override void Start()
    {
        base.Start();

        // Scale health and damage based on current level
        if (GameManager.Instance != null)
        {
            int levelMultiplier = GameManager.Instance.currentLevel;
            maxHealth = Mathf.RoundToInt(maxHealth * (1f + (levelMultiplier - 1) * 0.3f));
            attackDamage = Mathf.RoundToInt(attackDamage * (1f + (levelMultiplier - 1) * 0.2f));
            currentHealth = maxHealth;
        }
    }

    void Update()
    {
        if (isDead) return;

        // Simple waypoint movement
        if (waypoints != null && waypoints.Length > 0)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

            // Check if reached waypoint
            if (Vector2.Distance(transform.position, targetWaypoint