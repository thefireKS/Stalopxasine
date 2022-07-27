using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Mathematics;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [FormerlySerializedAs("data")]
    public PlayerData Data;
    private Attack atck;
    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D playerCollider;

    [Header("Collision Checkers")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform groundCheckR, groundCheckL;
    private float rayDistance = 0.1f;

    // Physics States
    private bool isFacingLeft;
    private GameObject currentOneWayPlatform;
    private float gravityScale;

    // Input State
    private float moveX;
    private float moveY;

    // Player State
    private bool isDropping;
    public bool canJump;
    private bool jumpPressed;
    private bool attacking;

    public enum PlayerStates
    {
        Grounded,
        Jumping,
        Falling,
    }
    
    public PlayerStates state;

    // Timer Caches
    private float lastGroundedTime = -10;
    private WaitForSeconds DisablingCooldown;
    private WaitForSeconds AttackingCooldown;

    /// <summary>
    /// Use this to GetComponent cache stuff and most the expensive things.
    /// </summary>
    private void Awake()
    {
        playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        atck = GetComponent<Attack>();
        gravityScale = rb2d.gravityScale;
        DisablingCooldown = new WaitForSeconds(0.2f);
        AttackingCooldown = new WaitForSeconds(0.1f);
    }

    private void Update() 
    {
        Debug.Log(canJump +"  "+jumpPressed + " " + state);
        if (!PlayerMeeting.DialogIsGoing)
        {
            // Input Method
            ProcessInput();
            
            ProcessAttack();
            
            ProcessAnimation();

            isDropping = moveY < 0 && currentOneWayPlatform != null && !attacking && groundCheck();
        }
    }

    private void FixedUpdate()
    {
        Controls();
    }

    private void Controls()
    {
        if (!attacking &&!PlayerMeeting.DialogIsGoing)
        {
            moving();
            
            if (state == PlayerStates.Grounded) grounded();
            if (state == PlayerStates.Jumping) jumping();
            if (state == PlayerStates.Falling) falling();

            if ( !PlayerMeeting.DialogIsGoing)
            {
                if (Time.time > lastGroundedTime + Data.jumpCoyoteTime)
                {
                    canJump = false;
                    jumpPressed = false;
                }
                else
                    rb2d.gravityScale = gravityScale;


                if (jumpPressed && canJump && state != PlayerStates.Grounded)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, Data.jumpForce);
                    canJump = false;
                    jumpPressed = false;
                }

                if (isDropping)
                    StartCoroutine(DisableCollision());
            }
            else
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                animator.SetBool("isGrounded", true);
                animator.SetBool("isGoing", false);
            }
        }
    }
    
    private void moving()
    {
        float targetSpeed = moveX * Data.speed;
        float currentVelocity = rb2d.velocity.x;
        float acc;
        if (targetSpeed == 0)
        { 
            acc = Data.deceleration;
        } else {
            acc = Data.acceleration;
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
    }
    
    private void grounded()
    {
        rb2d.gravityScale = gravityScale;
        if (groundCheck()) {
            lastGroundedTime = Time.time;
            canJump = true;
            jumpPressed = false;
        } else {
            state = PlayerStates.Falling;
        }
    }
    private void jumping()
    {
        if(rb2d.velocity.y < 0) {
            state = PlayerStates.Falling; 
        }
    }
    private void falling()
    {
        rb2d.gravityScale = gravityScale * Data.fallGravityMultiplier;
        if (groundCheck())
            state = PlayerStates.Grounded;
    }

    private void attackDirectionSetter()
    {
        Vector2 attackDirection = atck.AttackVector();
        Vector2 attackingVector = new Vector2(attackDirection.x * Data.speed,attackDirection.y * Data.jumpForce);
        if (state == PlayerStates.Grounded && attackingVector.y < 0)
            attackingVector.y = 0;
        rb2d.velocity = attackingVector;
    }
    private bool groundCheck()
    {
        return Physics2D.Raycast(groundCheckR.position, Vector2.down, rayDistance,
            layerMask.value) || Physics2D.Raycast(groundCheckL.position, Vector2.down, rayDistance,
            layerMask.value);
    }
    
    /// <summary>
    /// All the input manipulation happens here
    /// Call it in Update()
    /// </summary>
    private void ProcessInput()
    {
        // Movement - From WASD separately, we now use unity's in built stuff to get 1 for right and -1 for left or up and down.
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown("space"))
        {
            jumpPressed = true;
            state = PlayerStates.Jumping;
        }
    }

    /// <summary>
    /// Flipping of Character based on which direction button is pressed.
    /// Called in ProcessAnimation()
    /// </summary>
    private void Flip()
    {
        if (moveX != 0)
        {
            float facingLeft = moveX > 0 ? 0 : -1;
            if (moveX != 0)
                transform.rotation = quaternion.RotateY(Mathf.PI*facingLeft);
        }
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
        
        animator.SetBool("isAttacking", attacking);
    }
    
    private void ProcessAttack()
    {
        if (Input.GetButton("Fire1")&&!attacking)
            StartCoroutine(AttackOnClick());
    }
    
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return DisablingCooldown;
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
    private IEnumerator AttackOnClick()
    {
        attacking = true;
        atck.Attacking();
        attackDirectionSetter();
        yield return AttackingCooldown;
        attacking = false;
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