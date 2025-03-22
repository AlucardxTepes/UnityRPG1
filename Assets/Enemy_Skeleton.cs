using UnityEngine;

public class Enemy_Skeleton : Entity
{
    bool isAttacking;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                rb.linearVelocity = new Vector2(moveSpeed * 1.5f * facingDir, rb.linearVelocityY);
                isAttacking = false;
                Debug.Log("I see the player");
            }
            else
            {
                Debug.Log("ATTACK! " + isPlayerDetected.collider.gameObject.name);
                isAttacking = true;
            }
        }

        if (!isGrounded || isWallDetected)
            Flip();
        
        Movement();
    }

    private void Movement()
    {
        if (!isAttacking) 
            rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
}
