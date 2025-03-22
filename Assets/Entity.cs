using UnityEngine;


public class Entity : MonoBehaviour
{

    protected Rigidbody2D rb;
    protected Animator anim;

    protected int facingDir = 1;
    protected bool facingRight = true;


    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance; // Max distance for the Raycast
    [SerializeField] protected LayerMask whatIsGround; // specifies which layers should be considered as "ground." Only colliders on these layers will be detected by the raycast

    protected bool isGrounded;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    
    }

    protected virtual void Update()
    {
        CollisionChecks();
    }

    protected virtual void CollisionChecks()
    {
        // Raycast creates an invisible line and, using colliders, returns true if it hits something, and false if it doesn't.
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    
    protected virtual void Flip()
    {
        facingDir = facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}
