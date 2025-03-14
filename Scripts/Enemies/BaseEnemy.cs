using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth;
    public int currentHealth;
    public int attackDamage;

    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    public GameObject attackEffectPrefab;

    [Header("References")]
    public Animator animator;
    protected BeatDetector beatDetector;
    protected PlayerController player;

    protected bool canAttack = true;
    protected bool isDead = false;

    public delegate void EnemyEvent(BaseEnemy enemy);
    public static event EnemyEvent OnEnemyDefeated;

    protected virtual void Start()
    {
        beatDetector = FindObjectOfType<BeatDetector>();
        player = FindObjectOfType<PlayerController>();
        currentHealth = maxHealth;

        // Subscribe to the beat event
        BeatDetector.OnBeat += OnBeat;
    }

    protected virtual void OnDestroy()
    {
        // Unsubscribe from beat event
        BeatDetector.OnBeat -= OnBeat;
    }

    // Called on each music beat
    protected virtual void OnBeat(int beatNumber)
    {
        // Base implementation: attack on every 4th beat
        if (beatNumber % 4 == 0 && canAttack && !isDead)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (!canAttack || isDead) return;

        if (animator != null)
            animator.SetTrigger("Attack");

        // Start attack cooldown
        canAttack = false;
        Invoke("ResetAttack", attackCooldown);

        // Check if player is parrying
        bool playerParried = player.IsParrying() && beatDetector.IsOnBeat();

        if (playerParried)
        {
            // Player successfully parried
            TakeDamage(10); // Base damage from successful parry
        }
        else
        {
            // Player failed to parry, deal damage to player
            player.TakeDamage(attackDamage);

            // Instantiate attack effect
            if (attackEffectPrefab != null)
            {
                Instantiate(attackEffectPrefab, player.transform.position, Quaternion.identity);
            }
        }
    }

    protected void ResetAttack()
    {
        canAttack = true;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (animator != null)
            animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        if (animator != null)
            animator.SetBool("IsDead", true);

        // Disable collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Notify game manager
        if (OnEnemyDefeated != null)
            OnEnemyDefeated(this);

        // Destroy after animation
        Destroy(gameObject, 1.5f);
    }
}