using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform enemyTarget;  // Assign the enemy in the Inspector
    public float moveSpeed = 5f;   // Speed of movement
    public float stopDistance = 2f; // Distance at which to stop before the enemy

    private bool isMoving = true;

    void Update()
    {
        if (enemyTarget == null) return;

        float distanceToEnemy = Vector2.Distance(transform.position, enemyTarget.position);

        // Move if the player is farther than stopDistance
        if (isMoving && distanceToEnemy > stopDistance)
        {
            Vector2 moveDirection = (enemyTarget.position - transform.position).normalized;
            transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            isMoving = false; // Stop moving
        }
    }
}
