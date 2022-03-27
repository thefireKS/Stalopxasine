using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [FormerlySerializedAs("data")]
    public PlayerData Data;
    public Transform BulletPosition;
    private Animator animator;
    private Rigidbody2D rb2d;
    private BoxCollider2D PlayerCollider;

    [Header("Collision Checkers")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform groundCheckR, groundCheckL, groundChekEndR, groundChekEndL;

    // Physics States
    private bool isFacingLeft;
    private bool isGrounded;
    private GameObject CurrentOneWayPlatform;
    private float gravityScale;

    // Input State
    private bool jumpPressed;
    private float moveX;
    private float moveY;

    // Player State
    private bool isAttacking;
    private bool isJumping;
    private bool isDropping;
    private bool isJumpingOnce;
    
    // Timer Caches
    private float lastGroundedTime;
    private float lastJumpTime;
    private WaitForSeconds DisablingCooldown;
    private WaitForSeconds AttackTimer;

    
    /// <summary>
    /// Use this to GetComponent cache stuff and most the expensive things.
    /// </summary>
    private void Awake()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        gravityScale = rb2d.gravityScale;

        DisablingCooldown = new WaitForSeconds(0.4f);
        AttackTimer = new WaitForSeconds(Data.AttackTime);
    }

    private void Update() 
    {
        if (!PlayerMeeting.DialogIsGoing)
        {
            // Input Method
            ProcessInput();

            #region NewJumpingControls
            // Jump
            if(Input.GetKey("space"))
                lastJumpTime = Data.JumpBufferTime;


            if (Input.GetKeyUp("space"))
                isJumpingOnce = true;

            #endregion
            

            // Dropping Down
            isDropping = moveY < 0 && CurrentOneWayPlatform != null && !isAttacking && isGrounded;

            // Jump State
            isJumping = isGrounded && jumpPressed;

            #region NewJumpingthings
            //Coyote time time!
            if (isGrounded)
                lastGroundedTime = Data.JumpCoyoteTime;
            if (rb2d.velocity.y <= 0 && jumpPressed)
                isJumping = false;
            
            lastGroundedTime -= Time.deltaTime;
            lastJumpTime -= Time.deltaTime;
            #endregion
            
            // Animator
            ProcessAnimation();
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerMeeting.DialogIsGoing)
        {

            isGrounded = Physics2D.Linecast(groundCheckR.position, groundChekEndR.position,
                layerMask.value) || Physics2D.Linecast(groundCheckL.position, groundChekEndL.position,
                layerMask.value);

            // Jump State after Jump Key is pressed. 
            if (isJumping && lastGroundedTime>0 && lastJumpTime > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, Data.JumpForce);
                lastGroundedTime = 0;
                lastJumpTime = 0;
                isJumping = false;
                jumpPressed = false;
            }

            //Separating Jumps so now you need to hold for higher jumping
            if (isJumpingOnce && rb2d.velocity.y > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * (1 - Data.JumpCutMultiplier));
                isJumping = false;
                isJumpingOnce = false;
                lastJumpTime = 0;
            }

            #region NewMovementThings
            //calculate the direction we want to move in and our desired velocity
            float targetSpeed = moveX * Data.Speed;
            //calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - rb2d.velocity.x;
            //change acceleration rate depending on situation
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.Acceleration : Data.Deceleration;
            //applies acceleration to speed difference, the raises to a set power so acceleration increases with higher speeds
            //finally multiplies by sign to reapply direction
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, Data.VelocityPower) * Mathf.Sign(speedDif);
            #endregion

            // Move Physics
            rb2d.velocity = new Vector2(movement, rb2d.velocity.y);
            
            #region NewFrictionThing
            if (lastGroundedTime > 0 && Mathf.Abs(moveX) < 0.01f) 
            {
                //then we use either the friction amount (~ 0.2) or our velocity
                float amount = Mathf.Min(Mathf.Abs(rb2d.velocity.x), Mathf.Abs(Data.FrictionAmount));
                //sets to movement direction
                amount *= Mathf.Sign(rb2d.velocity.x);
                //applies force against movement direction
                rb2d.AddForce(Vector2.right * -amount, ForceMode2D.Impulse); 		
            }
            #endregion

            #region NewJumpingGravity

            if (rb2d.velocity.y < 0 && lastGroundedTime <= 0)
            {
                rb2d.gravityScale = gravityScale * Data.FallGravityMultiplier;
            }
            else
            {
                rb2d.gravityScale = gravityScale;
            }

            #endregion

            if (isDropping)
            {
                StartCoroutine(DisableCollision());
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0,rb2d.velocity.y);
            animator.SetBool("isGrounded",true);
            animator.SetBool("isGoing",false);
        }
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


        // Attack
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(WaitForAttackToFinish());
        }
    }

    /// <summary>
    /// Attack Trigger and cooldown
    /// Called in Update()
    /// </summary>
    private IEnumerator WaitForAttackToFinish()
    {
        isAttacking = true;
        if (!Data.MeleeAttack)
        {
            BulletFly bulletObj = Instantiate(Data.Bullet);
            bulletObj.Shooting(isFacingLeft);
            bulletObj.transform.position = BulletPosition.transform.position;
        }
        yield return AttackTimer;
        isAttacking = false;
    }

    /// <summary>
    /// Disable Collision for One Way Platforms
    /// </summary>
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = CurrentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(PlayerCollider, platformCollider);
        yield return DisablingCooldown;
        Physics2D.IgnoreCollision(PlayerCollider, platformCollider, false);
    }

    /// <summary>
    /// Flipping of Character based on which direction button is pressed.
    /// Called in ProcessAnimation()
    /// </summary>
    private void Flip()
    {
        if (moveX != 0)
        {
            transform.localScale = new Vector2(moveX, 1f);
            isFacingLeft = moveX < 0;
        }
    }


    /// <summary>
    /// Handling Animations in one place
    /// Call it in Update
    /// </summary>
    private void ProcessAnimation()
    {
        // Set animation parameter to isGrounded, if its false it will go to jump animation first
        animator.SetBool("isGrounded", isGrounded);

        // Flip
        Flip();

        // Move State to Idle State, it will be true if moveX is above or lower than 0 
        // and on ground
        animator.SetBool("isGoing", moveX != 0 && isGrounded);

        // Attack State
        animator.SetBool("isAttacking", isAttacking);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundPlatforms"))
        {
            CurrentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundPlatforms"))
        {
            CurrentOneWayPlatform = null;
        }
    }
}