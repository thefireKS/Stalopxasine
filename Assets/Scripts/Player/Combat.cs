using System.Collections;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Combat : MonoBehaviour
    {
        public void Initialize(GameObject bullet, float attackTime)
        {
            SetBullet(bullet);
            SetAttackTime(attackTime);
            SetBulletPositionAndRotation();
        }
    
        [Space(10)]
        private Transform _bulletPositionRotation;
        private Transform _actualBulletPosition;

        private void SetBulletPositionAndRotation()
        {
            _bulletPositionRotation = GameObject.Find("Bullet Rotation").transform;
            _actualBulletPosition = GameObject.Find("Bullet Position").transform;
        }

        private GameObject _bullet;

        private void SetBullet(GameObject bullet)
        {
            _bullet = bullet;
        }

        private float _attackTime;

        private void SetAttackTime(float attackTime)
        {
            _attackTime = attackTime;
        }
    
        private Animator _anim;
        private PlayerControls _playerControls;
        private ActionState _actionState;

        [SerializeField] private float bufferTime = 0.1f;
        private float _bufferTimer;

        private bool _isLookingLeft;
        private int _high; //-2 - down, -1 - 45 deg down, 0 - middle, 1 - 45 deg up, 2 - up

        private float _angle;
    
        private bool _isAutoAttacking;
        //private float meleeDetection;
        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();

            _actionState = GetComponent<ActionState>();
            _playerControls = PlayerInputHandler.PlayerControls;
        }
    
        private void OnEnable()
        {
            _playerControls.Player.Attack.started += PerformAttack;
            _playerControls.Player.AutoAttack.started += EnableAutoAttack;
        }

        private void OnDisable()
        {
            _playerControls.Player.Attack.started -= PerformAttack;
            _playerControls.Player.AutoAttack.started -= EnableAutoAttack;
        }

        private void Update()
        {
            if (_bufferTimer >= 0) _bufferTimer -= Time.deltaTime;
        
            if (_isAutoAttacking)
                PerformAttack();

            var moveInput = _playerControls.Player.Movement.ReadValue<Vector2>();
            SetShootingPoint(moveInput);
        }

        private void EnableAutoAttack(InputAction.CallbackContext context)
        {
            _isAutoAttacking = !_isAutoAttacking;
        }
    
        private void PerformAttack()
        {
            if(_actionState.GetState() == ActionState.States.Dialogue) return;
            if(_actionState.GetState() == ActionState.States.Attacking) return;
        
            StartCoroutine(Attack());
            _anim.SetFloat("attackDir",_high);
            var bullet = Instantiate(_bullet, _actualBulletPosition.position, _bulletPositionRotation.rotation);
            if (bullet.TryGetComponent(out MeleeProjectile _))
                bullet.transform.SetParent(transform);
        }
    
        private void PerformAttack(InputAction.CallbackContext context)
        {
            if(_actionState.GetState() == ActionState.States.Dialogue) return;
            if(_actionState.GetState() == ActionState.States.Attacking) return;

            StartCoroutine(Attack());
            _anim.SetFloat("attackDir",_high);
            var bullet = Instantiate(_bullet, _actualBulletPosition.position, _bulletPositionRotation.rotation);
            if (bullet.TryGetComponent(out MeleeProjectile _))
                bullet.transform.SetParent(transform);
        }

        private IEnumerator Attack()
        {
            if (PlayerMeeting.DialogIsGoing) yield break;
        
            _actionState.ChangeActionState(ActionState.States.Attacking);
        
            PerformAttack();
            yield return new WaitForSeconds(_attackTime);
            _actionState.ChangeActionState(ActionState.States.Idle);
        }
        private void SetShootingPoint(Vector2 input)
        {
            if(_bufferTimer > 0) return;
            if (input == Vector2.zero) return;
            var inputVector = input;

            _angle = Mathf.Atan2(inputVector.y,inputVector.x) * Mathf.Rad2Deg;
            SetDirectionForAnimator(_angle);


            var bulletPositionTransform = _bulletPositionRotation.transform;
        
            bulletPositionTransform.position = new Vector3(transform.position.x,transform.position.y, 0);
            bulletPositionTransform.localEulerAngles = new Vector3(0,0,_angle);
            _bufferTimer = bufferTime;
        }

        // TODO : Move this function to animator controller 
        private void SetDirectionForAnimator(float shootingPointAngle)
        {
            _high = Mathf.RoundToInt(Mathf.Sin(shootingPointAngle * Mathf.Deg2Rad)* 2);
        }
    }
}