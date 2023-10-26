using System;
using System.Collections;
using Player.States;
using UnityEngine;

namespace Enemy
{
    public class Rush : Patroling
    {
        [SerializeField] private float rayDistanceToCheckPlayer;
        [SerializeField] private LayerMask playerLayer;

        [Space(5)]
        [SerializeField] private float rayDistanceToCheckAnything;
        [SerializeField] private LayerMask excludeEnemy;
        
        [Space(5)]
        [SerializeField] private float timeToWarmup;
        
        [SerializeField] private float timeInStun;
        
        [Space(5)]
        [SerializeField] private float speedInRush;

        private float _normalSpeed;
        private Animator _animator;

        public static event Action OnSpotPlayer;
        
        private void Start()
        {
            _normalSpeed = speed;
            _animator = GetComponent<Animator>();
        }

        private bool _inRush;
        private bool _inStun;

        private bool CheckPlayer()
        {
            var bounds = _collider.bounds;
            var direction = _isGoingRight ? 1 : -1;
            var rayPosition = _isGoingRight ? bounds.max : bounds.min;
            rayPosition.y = bounds.center.y; //right position
            Debug.DrawRay(rayPosition, Vector2.right * (direction * rayDistanceToCheckPlayer), Color.yellow);
            RaycastHit2D hit2D = Physics2D.Raycast(rayPosition, Vector2.right * direction, rayDistanceToCheckPlayer, playerLayer);
            if (hit2D)
            {
                OnSpotPlayer?.Invoke();
            }
            return hit2D.transform != null;
        }
        
        private bool CheckAnything()
        {
            var bounds = _collider.bounds;
            var direction = _isGoingRight ? 1 : -1;
            var rayPosition = _isGoingRight ? bounds.max : bounds.min;
            rayPosition.y = bounds.center.y; //right position
            Debug.DrawRay(rayPosition, Vector2.right * (direction * rayDistanceToCheckAnything), Color.blue);
            RaycastHit2D hit2D = Physics2D.Raycast(rayPosition, Vector2.right * direction, rayDistanceToCheckAnything, excludeEnemy);
            
            return hit2D.transform != null;
        }

        protected override void Behavior()
        {
            if (!_inRush)
            {
                Patrol();
                if (CheckPlayer())
                {
                    _inRush = true;
                    StartCoroutine(Warmup());
                }
            }
            else
            {
                if (CheckAnything())
                {
                    if (!_inStun)
                    {
                        StartCoroutine(Stun());
                    }
                }
            }
        }

        private IEnumerator Warmup()
        {
            Debug.Log("Start WarmUp");
            SetVelocityX(0);
            _animator.SetTrigger("IsInWarmup");
            yield return new WaitForSeconds(timeToWarmup);
            var direction = _isGoingRight ? 1 : -1;
            SetVelocityX(speedInRush*direction);
            Debug.Log("RUSH!");
            _animator.SetBool("IsInRush",true);
        }
        
        private IEnumerator Stun()
        {
            _animator.SetBool("IsInRush",false);
            _animator.SetBool("IsInStun", true);
            _inStun = true;
            SetVelocityX(0);
            yield return new WaitForSeconds(timeInStun);
            _animator.SetBool("IsInStun", false);
            var direction = _isGoingRight ? 1 : -1;
            SetVelocityX(_normalSpeed*direction);
            _inStun = false;
            _inRush = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
                DealDamage(damage, damageable);
            
            if (!_inStun)
            {
                StartCoroutine(Stun());
            }
        }
    }
}
