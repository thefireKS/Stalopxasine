using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePC : MonoBehaviour
{
    [Header("Collision Checkers")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform groundCheckR, groundCheckL;
    private float rayDistance = 0.1f;

    //jump states
    [Header("Jump states")] 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoyoteTime; 
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float fallGravityMultiplier;
    private float lastGroundedTime;
    private float coyoteTimer;
    private float gravityScale;
    
    //move states
    [Header("Moving states")] 
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    
    //moving states
    private float moveX;
    private float moveY;
    private bool isDropping;
    private WaitForSeconds DisablingCooldown;
    
    //Components
    private Rigidbody2D rb2d;
    private Animator animator;
    private GameObject currentOneWayPlatform;
    private Collider2D playerCollider;
    
    private enum PlayerStates
    {
        Grounded,
        Jumping
    }
    
    private PlayerStates state;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gravityScale = rb2d.gravityScale;
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        state = PlayerStates.Grounded;
        DisablingCooldown = new WaitForSeconds(0.2f);
    }

    private void Update()
    {
        ProcessInput();
        
        if (state == PlayerStates.Grounded) grounded();
        if (state == PlayerStates.Jumping) jumping();
        
        ProcessAnimation();
        
        isDropping = moveY < 0 && currentOneWayPlatform != null && groundCheck();
    }

    private void FixedUpdate()
    {
        #region Movement
        float targetSpeed = moveX * speed;
        float currentVelocity = rb2d.velocity.x;
        float acc;
        if (targetSpeed == 0)
        { 
            acc = deceleration;
        } else {
            acc = acceleration;
        }
        if (currentVelocity < targetSpeed)
        {
            currentVelocity += acc * Time.fixedDeltaTime;
            if (currentVelocity > targetSpeed)
                currentVelocity = targetSpeed;
        }
        if (currentVelocity > targetSpeed)
        { 
            currentVelocity -= acc * Time.fixedDeltaTime; 
            if (currentVelocity < targetSpeed) 
                currentVelocity = targetSpeed;
        }
        rb2d.velocity = new Vector2(currentVelocity, rb2d.velocity.y);
        #endregion
        
        if (isDropping)
            StartCoroutine(DisableCollision());
    }

    private void grounded()
    {
        if (Input.GetKey("space"))
        {
            state = PlayerStates.Jumping;
            if (coyoteTimer>0 && lastGroundedTime>0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
        }
        if (Input.GetKeyUp("space"))
        {
            coyoteTimer = 0;
        }
    }
    private void jumping()
    {
        if (groundCheck()) {
            state = PlayerStates.Grounded;
            lastGroundedTime = jumpBufferTime;
            coyoteTimer = jumpCoyoteTime;
        } else {
            coyoteTimer -= Time.deltaTime;
            lastGroundedTime -= Time.deltaTime;
        }
        
        if (rb2d.velocity.y < 0) {
            rb2d.gravityScale = gravityScale * fallGravityMultiplier;
        } else {
            rb2d.gravityScale = gravityScale;
        }
    }
    private bool groundCheck()
    {
        return Physics2D.Raycast(groundCheckR.position, Vector2.down, rayDistance,
            layerMask.value) || Physics2D.Raycast(groundCheckL.position, Vector2.down, rayDistance,
            layerMask.value);
    }
    private void Flip()
    {
        if (moveX != 0)
        {
            transform.localScale = new Vector2(moveX, 1f);
        }
    }
    private void ProcessInput()
    {
        // Movement - From WASD separately, we now use unity's in built stuff to get 1 for right and -1 for left or up and down.
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
    }
    
    /// <summary>
    /// Handling Animations in one place
    /// Call it in Update
    /// </summary>
    private void ProcessAnimation()
    {
        animator.SetBool("isGrounded", groundCheck());
        
        Flip();
        
        animator.SetBool("isGoing", moveX != 0 && groundCheck());
    }
    
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return DisablingCooldown;
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundPlatforms"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundPlatforms"))
        {
            currentOneWayPlatform = null;
        }
    }
}
