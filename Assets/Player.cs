using System;
using UnityEngine;

public class Player : Entity
{

    [Header("Move info")]
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

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

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

    public void SetAttackOver()
    {
        isAttacking = false;

        comboCounter++;
        if (comboCounter > 2)
            comboCounter = 0;
    }
}