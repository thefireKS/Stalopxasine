using System;
using System.Collections;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        public void Initialize(float speed, float jumpBufferTime, float jumpForce, float fallGravityMultiplier,
            float jumpCoyoteTime, LayerMask layerMask)
        {
            _speed = speed;
            _jumpBufferTime = jumpBufferTime;
            _jumpForce = jumpForce;
            _fallGravityMultiplier = fallGravityMultiplier;
            _jumpCoyoteTime = jumpCoyoteTime;
            _layerMask = layerMask;
        }

        [Header("References")] private Combat _combat;

        // TODO: delete animator
        private Animator _animator;

        private float _speed;
        private float _jumpBufferTime;
        private float _jumpForce;
        private float _fallGravityMultiplier;
        private float _jumpCoyoteTime;

        private Rigidbody2D _rb2d;
        private BoxCollider2D _playerCollider;
        private SpriteRenderer _spriteRenderer;

        [Header("Collision Checkers")] private LayerMask _layerMask;
        private readonly float _rayDistance = 0.1f;
        private readonly float _maxSlopeAngle = 70f;

        private PlayerControls _playerControls;

        private GameObject _currentOneWayPlatform;
        private float _gravityScale;

        private bool _isDropping;

        private float _moveX;
        //private float _moveY;

        private bool _autoFire;

        private bool _isJumpPressed;

        // After groundCheck = false
        private float _coyoteTimer;

        // Time to jump after press
        private float _bufferTimer;
        private WaitForSeconds _disablingCooldown;

        private enum MovementStates
        {
            Grounded,
            Jumping,
            Falling,
        }

        private MovementStates _movementState = MovementStates.Falling;

        private ActionState _actionState;

        private void Awake()
        {
            _playerControls = PlayerInputHandler.PlayerControls;

            _actionState = GetComponent<ActionState>();

            _playerCollider = GetComponent<BoxCollider2D>();
            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rb2d = GetComponent<Rigidbody2D>();
            _combat = GetComponent<Combat>();
            _gravityScale = _rb2d.gravityScale;
            _disablingCooldown = new WaitForSeconds(0.5f);
            PlayerMeeting.DialogIsGoing = false;
        }

        private void OnEnable()
        {
            _playerControls.Player.VerticalMovementUp.started += JumpStart;
            _playerControls.Player.VerticalMovementUp.performed += JumpEnd;
            _playerControls.Player.VerticalMovementUp.canceled += JumpEnd;

            _playerControls.Player.VerticalMovementDown.started += OneWayPlatformMovement;

            ActionState.OnActionStateChanged += ProcessStateChange;
        }

        private void OnDisable()
        {
            _playerControls.Player.VerticalMovementUp.started -= JumpStart;
            _playerControls.Player.VerticalMovementUp.performed -= JumpEnd;
            _playerControls.Player.VerticalMovementUp.canceled -= JumpEnd;

            _playerControls.Player.VerticalMovementDown.started -= OneWayPlatformMovement;

            ActionState.OnActionStateChanged -= ProcessStateChange;
        }

        private void ProcessStateChange(ActionState.States state)
        {
            switch(state)
            {
                case ActionState.States.Dialogue:
                    
                    _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
                    Debug.Log("Set velocity 0");
                    break;
            }
        }

        private void Update()
        {
            ProcessAnimation();
            
            if (PlayerMeeting.DialogIsGoing)
                return;

            ProcessInput();
        }

        private void ProcessInput()
        {
            if (Time.timeScale < 0.2f) return;

            if (_actionState.GetState() != ActionState.States.Dialogue)
            {
                var movement = _playerControls.Player.Movement.ReadValue<Vector2>();

                _moveX = movement.x;
                
                if (_isJumpPressed)
                    AddJumpHeight();
            }
            
            Flip();
        }

        private void Flip()
        {
            if (_moveX != 0)
                _spriteRenderer.flipX = _moveX < 0;
        }

        private void ProcessAnimation()
        {
            _animator.SetBool("isGrounded", _movementState == MovementStates.Grounded);

            _animator.SetBool("isGoing", MathF.Abs(_rb2d.velocity.x) >= 0.0001 && _movementState == MovementStates.Grounded);

            _animator.SetBool("isAttacking", _actionState.GetState() == ActionState.States.Attacking);

            _animator.SetFloat("ySpeed", _rb2d.velocity.y);
        }

        private void FixedUpdate()
        {
            if (PlayerMeeting.DialogIsGoing)
                _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);

            Moving();
            MovementStateSwapper();
        }

        private void Moving()
        {
            if (_actionState.GetState() == ActionState.States.Dialogue)
            {
                _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
                return;
            }
            float targetSpeed = _moveX * _speed;
            _rb2d.velocity = new Vector2(targetSpeed, _rb2d.velocity.y);
        }

        private bool GroundCheck()
        {
            if (_currentOneWayPlatform != null && MathF.Abs(_rb2d.velocity.y) > 0.2f) return false;
        
            var bounds = _playerCollider.bounds;
        
            /*RaycastHit2D hitDown = Physics2D.BoxCast(transform.position, bounds.extents,
                transform.rotation.z, Vector2.down, _rayDistance * 2, _layerMask.value);
            SlopeStand(Vector2.Angle(hitDown.normal, Vector2.up), hitDown.normal);*/

            //is 1 boxCast cheaper than 2 RayCast?

            Vector2 leftCorner = bounds.min;
            Vector2 rightCorner = bounds.max;
            rightCorner.y -= bounds.size.y;
        
            var leftRay = Physics2D.Raycast(rightCorner, Vector2.down, _rayDistance,
                _layerMask.value);
            var rightRay = Physics2D.Raycast(leftCorner, Vector2.down, _rayDistance,
                _layerMask.value);
        
            SlopeStand(Vector2.Angle((leftRay.transform != null? leftRay : rightRay).normal, Vector2.up), 
                (leftRay.transform != null? leftRay : rightRay).normal);
        
        

            /* return Physics2D.Raycast(rightCorner, Vector2.down, _rayDistance,
                 _layerMask.value) || Physics2D.Raycast(leftCorner, Vector2.down, _rayDistance,
                 _layerMask.value);*/
        
            return leftRay || rightRay;
            //return hitDown;
        }

        private void SlopeStand(float slopeAngle, Vector2 normal)
        {
            if (slopeAngle >= _maxSlopeAngle || slopeAngle < 1f) return;
        
            var gravityVector = new Vector2(0f, -20f * _gravityScale * _rb2d.mass);
            var frictionVector = -1 * (gravityVector +  normal.normalized * (gravityVector.magnitude * Mathf.Cos(slopeAngle* Mathf.Deg2Rad )));
        
            Debug.DrawRay(transform.position, frictionVector.normalized, Color.green);
            _rb2d.AddForce(frictionVector);
        }

        /*private Vector2 ProjectVector(Vector2 vector, Vector2 normal)
    {
        var projectedVector = Vector2.Dot(vector, normal) / (normal.magnitude * normal.magnitude) * normal;
        return projectedVector;
    }*/

        private void JumpStart(InputAction.CallbackContext context)
        {
            if (_isDropping) return;

            _bufferTimer = _jumpBufferTime;

            if (!GroundCheck() && _coyoteTimer < 0) return;
            _coyoteTimer = 0;
            if (_bufferTimer < 0) return;

            _isJumpPressed = true;
            _movementState = MovementStates.Jumping;
        }

        private void AddJumpHeight()
        {
            if (_isDropping) return;
            _rb2d.velocity = new Vector2(_rb2d.velocity.x, _jumpForce);
        }

        private void JumpEnd(InputAction.CallbackContext context)
        {
            _isJumpPressed = false;
            _movementState = MovementStates.Falling;
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
            switch (_movementState)
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
            _bufferTimer -= Time.deltaTime;
            _coyoteTimer -= Time.deltaTime;

            if (GroundCheck())
                _movementState = MovementStates.Grounded;
        }

        private void grounded()
        {
            _rb2d.gravityScale = _gravityScale;
            //attacksCounter = Data.possibleAttacks;
            _coyoteTimer = _jumpCoyoteTime;

            if (GroundCheck())
            {
                if (_bufferTimer > 0)
                {
                    _bufferTimer = 0f;
                    _movementState = MovementStates.Jumping;
                }
            }
            else
            {
                _movementState = MovementStates.Falling;
            }
        }

        private void jumping()
        {
            if (GroundCheck())
                _movementState = MovementStates.Grounded;
            else if (_rb2d.velocity.y < 0)
                _movementState = MovementStates.Falling;
        }

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
            yield return _disablingCooldown;
            Physics2D.IgnoreCollision(_playerCollider, platformCollider, false);
            _isDropping = false;
        }

        #endregion
    }
}