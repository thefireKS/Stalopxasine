using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [FormerlySerializedAs("data")]
    public PlayerData Data;
    private Attack atck;
    // TODO: delete animator
    private Animator animator;
    
    private Rigidbody2D rb2d;
    private BoxCollider2D playerCollider;
    private SpriteRenderer sr;
    
    [Header("Collision Checkers")]
    [SerializeField] private LayerMask layerMask;
    // TODO: ground check with BB (BB - bounding box)
    [SerializeField] private Transform groundCheckR, groundCheckL;
    private float rayDistance = 0.1f;

    private PlayerControls _playerControls;
    
    private GameObject currentOneWayPlatform;
    private float gravityScale;
    
    private bool isDropping;
    private int attacksCounter;

    private float moveX;
    private float moveY;

    private float coyoteTimer = 0f;
    private float bufferTimer = 0f;
    private WaitForSeconds DisablingCooldown;
    private enum MovementStates
    {
        Grounded,
        Jumping,
        Falling,
    }
    private MovementStates movementState = MovementStates.Falling;
    private enum ActionStates
    {
        Idle,
        Attacking
    }
    private ActionStates actionState = ActionStates.Idle;
    
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();
        
        playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        atck = GetComponent<Attack>();
        gravityScale = rb2d.gravityScale;
        DisablingCooldown = new WaitForSeconds(0.2f);
        PlayerMeeting.DialogIsGoing = false;
    }

    private void OnEnable()
    {
        _playerControls.Player.Jump.started += Jump;
        _playerControls.Player.Attack.started += Attack;
    }

    private void OnDisable()
    {
        _playerControls.Player.Jump.started -= Jump;
        _playerControls.Player.Attack.started -= Attack;
    }

    private void Update()
    {
        if (PlayerMeeting.DialogIsGoing)
            return;

        coyoteTimer -= Time.deltaTime;
        bufferTimer -= Time.deltaTime;

        ProcessInput();
        
        StateSwapper();
        
        if (actionState != ActionStates.Attacking)
            isDropping = moveY < 0 && currentOneWayPlatform != null && groundCheck();
        else
            isDropping = false;
        
        ProcessAnimation();
    }
    private void FixedUpdate()
    {
        if (PlayerMeeting.DialogIsGoing)
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        
        Controls();
    }
    private void Controls()
    {
        if (actionState == ActionStates.Attacking)
            return;
        
        moving();

        if (isDropping)
            StartCoroutine(DisableCollision());
    }
    
    private void moving()
    {
        float targetSpeed = moveX * Data.speed;
        float currentVelocity = rb2d.velocity.x;
        float acc;
        
        if (Mathf.Approximately(targetSpeed, 0f))
            acc = Data.deceleration;
        else
            acc = Data.acceleration;
        
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
    private void ProcessInput()
    {
        if (Time.timeScale < 0.2f) return;

        // Movement - From WASD separately, we now use unity's in built stuff to get 1 for right and -1 for left or up and down.
        var movement = _playerControls.Player.Move.ReadValue<Vector2>();
        
        moveX = movement.x;
        moveY = movement.y;
        
        /*if (Input.GetKeyDown("space"))
            bufferTimer = Data.jumpBufferTime;*/

        /*if (Input.GetButtonDown("Fire1") && attacksCounter > 0 && actionState == ActionStates.Idle)
            StartCoroutine(AttackOnClick());*/
    }
    
    private void ProcessAnimation()
    {
        Flip();
        
        animator.SetBool("isGrounded", movementState == MovementStates.Grounded);
        
        animator.SetBool("isGoing", moveX != 0 && movementState == MovementStates.Grounded);
        
        animator.SetBool("isAttacking", actionState == ActionStates.Attacking);
    }
    private void Flip()
    {
        if (moveX != 0)
            sr.flipX = moveX < 0;
    } 
    private bool groundCheck()
    {
        return Physics2D.Raycast(groundCheckR.position, Vector2.down, rayDistance,
            layerMask.value) || Physics2D.Raycast(groundCheckL.position, Vector2.down, rayDistance,
            layerMask.value);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 0.2f) return;
        bufferTimer = Data.jumpBufferTime;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 0.2f) return;
        if (attacksCounter > 0 && actionState == ActionStates.Idle)
        {
            StartCoroutine(AttackOnClick());
        }
    }
    
    #region States
    private void StateSwapper()
    {
        switch (movementState)
        {
            case MovementStates.Falling:
                falling();
                break;
            case MovementStates.Grounded:
                grounded();
                break;
            case MovementStates.Jumping:
                jumping();
                break;
        }
    }
    private void falling()
    {
        rb2d.gravityScale = gravityScale * Data.fallGravityMultiplier;
        if (coyoteTimer > 0 && bufferTimer>0)
        {
            movementState = MovementStates.Jumping;
            rb2d.velocity = new Vector2(rb2d.velocity.x, Data.jumpForce);
            bufferTimer = 0f;
        }

        if (groundCheck())
            movementState = MovementStates.Grounded;
    }

    private void grounded()
    {
        rb2d.gravityScale = gravityScale;
        attacksCounter = Data.possibleAttacks;

        if (groundCheck())
        {
            if (bufferTimer>0)
            {
                movementState = MovementStates.Jumping;
                rb2d.velocity = new Vector2(rb2d.velocity.x, Data.jumpForce);
                bufferTimer = 0f;
            }
        }
        else
        {
            movementState = MovementStates.Falling;
            coyoteTimer = Data.jumpCoyoteTime;
        }
           
    }
    private void jumping()
    {
        if(rb2d.velocity.y < 0)
            movementState = MovementStates.Falling;
    }
    #endregion

    #region Attack
    private IEnumerator AttackOnClick()
    {
        actionState = ActionStates.Attacking;

        atck.Attacking();
        //attackDirectionSetter();

        yield return new WaitForSeconds(Data.attackTime);
        attacksCounter-=1;
        animator.speed = 1;
        actionState = ActionStates.Idle;
    }
    
    /*private void attackDirectionSetter()
    {
        float groundCoefficient = groundCheck() ? 0.5f : 1f;
        Vector2 attackDirection = atck.AttackVector();
        Vector2 attackingVector = new Vector2(attackDirection.x * Data.speed * groundCoefficient, attackDirection.y * Data.jumpForce * groundCoefficient);
        rb2d.velocity = attackingVector; 
    }*/
    
    #endregion

    #region Dropping
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
    
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return DisablingCooldown;
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
    #endregion
}
