using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public void Initialize(float speed, float jumpBufferTime, float jumpForce, float fallGravityMultiplier, float jumpCoyoteTime, LayerMask layerMask)
    {
        this._speed = speed;
        _jumpBufferTime = jumpBufferTime;
        _jumpForce = jumpForce;
        _fallGravityMultiplier = fallGravityMultiplier;
        _jumpCoyoteTime = jumpCoyoteTime;
        this._layerMask = layerMask;
    }

    [Header("References")]
    private PlayerAttack atck;
    // TODO: delete animator
    private Animator animator;

    private float _speed;
    private float _jumpBufferTime;
    private float _jumpForce;
    private float _fallGravityMultiplier;
    private float _jumpCoyoteTime;
    
    private Rigidbody2D _rb2d;
    private BoxCollider2D _playerCollider;
    private SpriteRenderer _spriteRenderer;
    
    [Header("Collision Checkers")]
    private LayerMask _layerMask;
    private readonly float _rayDistance = 0.1f;
    private readonly float _maxSlopeAngle = 70f;

    private PlayerControls _playerControls;
        
    private GameObject _currentOneWayPlatform;
    private float _gravityScale;
    
    private bool _isDropping;
    //private int attacksCounter;

    private float moveX;
    private float moveY;

    /*
    [SerializeField] private float longJumpDelay = 0.1f;
    [SerializeField] private float jumpTimer = 0.3f;
    [SerializeField] private float jumpModif = 0.33f;
    [SerializeField] private float jumpModifDec = 1f;
    private float _currentJumpModif;
    private float _currentJumpTimer;
    */

    private bool _autoFire;

    private bool isJumpPressed;
    
    // After groundCheck = false
    private float coyoteTimer = 0f;
    // Time to jump after press
    private float bufferTimer = 0f;
    private WaitForSeconds DisablingCooldown;
    private enum MovementStates
    {
        Grounded,
        Jumping,
        Falling,
    }
    private MovementStates movementState = MovementStates.Falling;
    public enum ActionStates
    {
        Idle,
        Attacking
    }
    [HideInInspector] public ActionStates actionState = ActionStates.Idle;
    
    private void Awake()
    {
        _playerControls = PlayerInputHandler.PlayerControls;

        _playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rb2d = GetComponent<Rigidbody2D>();
        atck = GetComponent<PlayerAttack>();
        _gravityScale = _rb2d.gravityScale;
        DisablingCooldown = new WaitForSeconds(0.5f);
        PlayerMeeting.DialogIsGoing = false;
    }

    private void OnEnable()
    {
        _playerControls.Player.VerticalMovementUp.started += JumpStart;
        _playerControls.Player.VerticalMovementUp.performed += JumpEnd;
        _playerControls.Player.VerticalMovementUp.canceled += JumpEnd;

        _playerControls.Player.VerticalMovementDown.started += OneWayPlatformMovement;

        //_playerControls.Player.Attack.started += Attack;
        //_playerControls.Player.AutoAttack.started += SwitchAuto;
    }

    private void OnDisable()
    {
        _playerControls.Player.VerticalMovementUp.started -= JumpStart;
        _playerControls.Player.VerticalMovementUp.performed -= JumpEnd;
        _playerControls.Player.VerticalMovementUp.canceled -= JumpEnd;
        
        _playerControls.Player.VerticalMovementDown.started -= OneWayPlatformMovement;
        
        //_playerControls.Player.Attack.started -= Attack;
        //_playerControls.Player.AutoAttack.started -= SwitchAuto;
    }

    private void Update()
    {
        if (PlayerMeeting.DialogIsGoing)
            return;

        ProcessInput();
        
        MovementStateSwapper();

        ProcessAnimation();
    }

    private void ProcessInput()
    {
        if (Time.timeScale < 0.2f) return;

        var movement = _playerControls.Player.Movement.ReadValue<Vector2>();

        moveX = movement.x;
        moveY = movement.y;

        Flip();

        if (isJumpPressed)
            AddJumpHeight();
    }

    private void Flip()
    {
        if (moveX != 0)
            _spriteRenderer.flipX = moveX < 0;
    }

    private void ProcessAnimation()
    {
        animator.SetBool("isGrounded", movementState == MovementStates.Grounded);

        animator.SetBool("isGoing", moveX != 0 && movementState == MovementStates.Grounded);

        animator.SetBool("isAttacking", actionState == ActionStates.Attacking);
        
        animator.SetFloat("ySpeed", _rb2d.velocity.y);
    }

    private void FixedUpdate()
    {
        if (PlayerMeeting.DialogIsGoing)
            _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
        
        Controls();
    }

    private void Controls()
    {
        Moving();
    }
    
    private void Moving()
    {
        float targetSpeed = moveX * _speed;
        _rb2d.velocity = new Vector2(targetSpeed, _rb2d.velocity.y);
    }
    
    private bool GroundCheck()
    {
        var bounds = _playerCollider.bounds;
        Vector2 leftCorner = bounds.min;
        Vector2 rightCorner = bounds.max;
        rightCorner.y -= bounds.size.y;

        
        RaycastHit2D downHit = Physics2D.CircleCast(transform.position,  0.5f*bounds.extents.magnitude, 
            Vector2.down, bounds.extents.magnitude, _layerMask.value);
        SlopeStand(Vector2.Angle(downHit.normal, Vector2.up));
        
        return Physics2D.Raycast(rightCorner, Vector2.down, _rayDistance,
            _layerMask.value) || Physics2D.Raycast(leftCorner, Vector2.down, _rayDistance,
            _layerMask.value) || downHit;
    }

    private void SlopeStand(float slopeAngle)
    {
        if (slopeAngle >= _maxSlopeAngle)
        {
            
            //_rb2d.velocity = new Vector2(0, 0);
        }
        _rb2d.gravityScale = slopeAngle <= _maxSlopeAngle && slopeAngle!=0 ?  0f :_gravityScale ;
        //_rb2d.drag = (slopeAngle >= _maxSlopeAngle && _playerControls.Player.Movement.ReadValue<Vector2>() == Vector2.zero) ? 100f : 0f;
    }
    private void JumpStart(InputAction.CallbackContext context)
    {
        if(_isDropping) return;
        
        bufferTimer = _jumpBufferTime;

        if (!GroundCheck() && coyoteTimer < 0) return;
        coyoteTimer = 0;
        if (bufferTimer < 0) return;
        
        isJumpPressed = true;
        movementState = MovementStates.Jumping;
    }
    
    private void AddJumpHeight()
    {
        if(_isDropping) return;
        _rb2d.velocity = new Vector2(_rb2d.velocity.x, _jumpForce);
    }

    private void JumpEnd(InputAction.CallbackContext context)
    {
        isJumpPressed = false;
        movementState = MovementStates.Falling;
    }

    private void OneWayPlatformMovement(InputAction.CallbackContext context)
    {
        if (_currentOneWayPlatform != null && GroundCheck())
        {
            _isDropping = true;
            StartCoroutine(DisableCollision());
        }
    }

    /*private void Attack(InputAction.CallbackContext context)
    {
        if (Time.timeScale < 0.2f) return;
        if (CanAttack() && !_autoFire)
        {
            StartCoroutine(AttackOnClick());
        }
    }
    
    private void Attack()
    {
        if (Time.timeScale < 0.2f) return;
        if (CanAttack())
        {
            StartCoroutine(AttackOnClick());
        }
    }*/

    #region States
    private void MovementStateSwapper()
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
        _rb2d.gravityScale = _gravityScale * _fallGravityMultiplier;
        bufferTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;

        if (GroundCheck())
            movementState = MovementStates.Grounded;
    }

    private void grounded()
    {
        _rb2d.gravityScale = _gravityScale;
        //attacksCounter = Data.possibleAttacks;
        coyoteTimer = _jumpCoyoteTime;
        
        if (GroundCheck())
        {
            if (bufferTimer > 0)
            {
                bufferTimer = 0f;
                movementState = MovementStates.Jumping;
            }
        }
        else
        {
            movementState = MovementStates.Falling;
        }
           
    }
    private void jumping()
    {
        if(GroundCheck())
            movementState = MovementStates.Grounded;
        else if (_rb2d.velocity.y < 0)
            movementState = MovementStates.Falling;
    }
    #endregion

    #region Attack
    /*private IEnumerator AttackOnClick()
    {
        actionState = ActionStates.Attacking;

        //atck.PerformAttack();
        //attackDirectionSetter();
        yield return new WaitForSeconds(Data.attackTime);
        
        actionState = ActionStates.Idle;
    }*/
    
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
            _currentOneWayPlatform = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundPlatforms"))
        {
            _currentOneWayPlatform = null;
        }
    }
    
    private IEnumerator DisableCollision()
    {
        CompositeCollider2D platformCollider = _currentOneWayPlatform.GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(_playerCollider, platformCollider);
        yield return DisablingCooldown;
        Physics2D.IgnoreCollision(_playerCollider, platformCollider, false);
        _isDropping = false;
    }
    #endregion
}
