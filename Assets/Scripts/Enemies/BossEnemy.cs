using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BaseEnemy
{
    [Header("Boss Specific Settings")]
    public float specialAttackChance = 0.3f;
    public GameObject specialAttackEffectPrefab;
    public int specialAttackDamage;
    public float specialAttackScale = 1.5f;

    [Header("Pattern Settings")]
    public int[] attackPattern = new int[] { 4, 2, 4, 2, 8 }; // Beats to attack on
    private int patternIndex = 0;
    private int beatCounter = 0;

    protected override void Start()
    {
        base.Start();

        // Scale boss stats based on level
        int levelMultiplier = GameManager.Instance.currentLevel;
        maxHealth = Mathf.RoundToInt(maxHealth * (1f + levelMultiplier * 0.5f));
        attackDamage = Mathf.RoundToInt(attackDamage * (1f + levelMultiplier * 0.3f));
        specialAttackDamage = Mathf.RoundToInt(specialAttackDamage * (1f + levelMultiplier * 0.4f));
        currentHealth = maxHealth;

        // Boss specific initialization
        animator.SetBool("IsBoss", true);
    }

    protected override void OnBeat(int beatNumber)
    {
        beatCounter++;

        // Check if it's time to attack according to the pattern
        if (beatCounter >= attackPattern[patternIndex] && canAttack && !isDead)
        {
            // Reset beat counter
            beatCounter = 0;

            // Move to next position in the pattern
            patternIndex = (patternIndex + 1) % attackPattern.Length;

            // Determine if this is a special attack
            if (Random.value < specialAttackChance)
            {
                StartCoroutine(SpecialAttack());
            }
            else
            {
                Attack();
            }
        }
    }

    protected override void Attack()
    {
        base.Attack();

        // Boss specific attack behavior
        animator.SetTrigger("Attack");

        // Visual effect for boss attack
        if (attackEffectPrefab != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            GameObject effect = Instantiate(attackEffectPrefab, transform.position + (Vector3)direction, Quaternion.identity);
            effect.transform.localScale *= 1.2f; // Bigger effect for boss
        }
    }

    IEnumerator SpecialAttack()
    {
        if (!canAttack || isDead) yield break;

        // Disable normal attacks during special attack
        canAttack = false;

        // Telegraph the special attack
        animator.SetTrigger("SpecialAttackCharge");

        // Wait for telegraph animation
        yield return new WaitForSeconds(1.0f);

        // Execute special attack
        animator.SetTrigger("SpecialAttack");

        // Visual effect for special attack
        if (specialAttackEffectPrefab != null)
        {
            GameObject effect = Instantiate(specialAttackEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.localScale *= specialAttackScale;
        }

        // Check if player is parrying
        bool playerParried = player.IsParrying() && beatDetector.IsOnBeat();

        if (playerParried)
        {
            // Player successfully parried special attack
            TakeDamage(20); // Higher damage for successfully parrying special attack
        }
        else
        {
            // Player failed to parry, deal special attack damage
            player.TakeDamage(specialAttackDamage);
        }

        // Cooldown after special attack
        yield return new WaitForSeconds(attackCooldown * 1.5f);

        // Re-enable attacks
        canAttack = true;
    }

    protected override void Die()
    {
        // Play boss death animation
        animator.SetTrigger("BossDeath");

        // Boss specific death effects
        // Add particles, screen shake, etc.

        // Call base implementation
        base.Die();
    }
}