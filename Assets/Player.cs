using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration = 1;
    private float dashTimer;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer = 0;

    [Header("Attack info")]
    [SerializeField] private float comboTime = 0.3f;
    private int comboCounter;
    private float comboTimeWindow;
    private bool isAttacking;



    private float xInput;
    private int facingDir = 1;
    private bool facingRight = true;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance; // Max distance for the Raycast
    [SerializeField] private LayerMask whatIsGround; // specifies which layers should be considered as "ground." Only colliders on these layers will be detected by the raycast
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();
        CheckInput();
        CollisionChecks();


        dashTimer -= Time.deltaTime; // countdown time regardless of FPS
        comboTimeWindow -= Time.deltaTime;
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        FlipController();
        AnimatorControllers();
    }

    private void CollisionChecks()
    {
        // Raycast creates an invisible line and, using colliders, returns true if it hits something, and false if it doesn't.
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();
    }

    private void StartAttackEvent()
    {
        if (!isGrounded) 
            return;
        
        if (comboTimeWindow < 0) // reset combo if time window has passed
            comboCounter = 0;

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void Dash()
    {
        if (dashCooldownTimer <= 0 && !isAttacking)
        {
            dashTimer = dashDuration; // start dash duration countdown
            dashCooldownTimer = dashCooldown;
        }
    }

    private void Movement()
    {
        if (isAttacking) {
            rb.linearVelocity = new Vector2(0,0); // stop movement while attacking
        }
        else if (dashTimer > 0)
        {
            // keep velY as 0 allows player to not fall as fast (floaty) when dashing while in the air
            // using facingDir instead of xInput allows dash to move player even dashing from idle
            rb.linearVelocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.linearVelocityX != 0;

        // Reference the data from the animator attached to this player game object
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTimer > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

    private void FlipController()
    {
        if (rb.linearVelocityX > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.linearVelocityX < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDir = facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

    public void SetAttackOver()
    {
        isAttacking = false;

        comboCounter++;
        if (comboCounter > 2)
            comboCounter = 0;
    }
}