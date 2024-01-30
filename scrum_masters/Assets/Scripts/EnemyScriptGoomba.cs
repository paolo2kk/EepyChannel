using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;
    public float raycastDistance = 0.5f;
    public LayerMask groundLayer;
    public float flipDelay = 0.5f; // Adjust this value as needed

    private bool movingRight = true;
    private float timeSinceLastFlip;

    void Update()
    {
        // Move the enemy
        Move();

        // Check for ground in front of the enemy
        CheckGround();
    }

    void Move()
    {
        // Calculate movement vector
        Vector2 movement = movingRight ? Vector2.right : Vector2.left;

        // Move the enemy
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void CheckGround()
    {
        // Only check for ground after a certain delay
        if (Time.time - timeSinceLastFlip > flipDelay)
        {
            // Cast a ray downward to check for ground
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);

            // If no ground is detected, turn back
            if (!hit.collider)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        // Change the direction of movement
        movingRight = !movingRight;

        // Flip the enemy sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Update the time of the last flip
        timeSinceLastFlip = Time.time;
    }
}