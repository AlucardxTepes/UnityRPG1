using UnityEngine;

public class Enemy_Skeleton : Entity
{
    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!isGrounded || isWallDetected)
            Flip();

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }
}
