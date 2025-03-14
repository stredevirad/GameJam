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
        int levelMultiplier = GameManager.Instance.currentLevel;
        maxHealth = Mathf.RoundToInt(maxHealth * (1f + (levelMultiplier - 1) * 0.3f));
        attackDamage = Mathf.RoundToInt(attackDamage * (1f + (levelMultiplier - 1) * 0.2f));
        currentHealth = maxHealth;
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
            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                // Move to next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    protected override void OnBeat(int beatNumber)
    {
        // Standard enemies attack on every 4th beat
        if (beatNumber % 4 == 0 && canAttack && !isDead)
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        base.Attack();

        // Add enemy-specific attack behavior
        // Play attack animation
        animator.SetTrigger("Attack");

        // Create attack effect
        if (attackEffectPrefab != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            Instantiate(attackEffectPrefab, transform.position + (Vector3)direction, Quaternion.identity);
        }
    }
}