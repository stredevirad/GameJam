using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Parry Settings")]
    public float parryWindow = 0.15f; // Time window for successful parry
    public GameObject parryEffectPrefab;

    [Header("References")]
    public Animator animator;
    public BeatDetector beatDetector;

    private bool canParry = true;

    public delegate void PlayerEvent(int currentHealth, int maxHealth);
    public static event PlayerEvent OnHealthChanged;
    public static event PlayerEvent OnPlayerDeath;

    void Start()
    {
        ResetHealth();
    }

    void Update()
    {
        // Check for parry input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            TryParry();
        }
    }

    void TryParry()
    {
        if (!canParry) return;

        if (animator != null)
            animator.SetTrigger("Parry");

        // Instantiate visual effect
        if (parryEffectPrefab != null)
        {
            Instantiate(parryEffectPrefab, transform.position, Quaternion.identity);
        }

        // Check if parry is on beat
        bool successfulParry = beatDetector.IsOnBeat();

        if (successfulParry)
        {
            // The actual damage is handled by the enemy when they detect a successful parry
            Debug.Log("Successful parry!");
        }
        else
        {
            Debug.Log("Missed the beat!");
        }

        // Prevent spamming parry
        canParry = false;
        Invoke("ResetParry", 0.3f);
    }

    void ResetParry()
    {
        canParry = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Trigger health UI update
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth, maxHealth);

        if (animator != null)
            animator.SetTrigger("Hit");

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger death animation
        if (animator != null)
            animator.SetBool("IsDead", true);

        // Disable controls
        this.enabled = false;

        // Notify game manager
        if (OnPlayerDeath != null)
            OnPlayerDeath(currentHealth, maxHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        if (animator != null)
            animator.SetBool("IsDead", false);

        // Trigger health UI update
        if (OnHealthChanged != null)
            OnHealthChanged(currentHealth, maxHealth);
    }

    public bool IsParrying()
    {
        // Check animator state to see if we're in parry animation
        if (animator != null)
            return animator.GetCurrentAnimatorStateInfo(0).IsName("Parry");
        return false;
    }
}