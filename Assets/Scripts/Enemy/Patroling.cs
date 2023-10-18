using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class Patroling : Base
    {
        [SerializeField] protected float speed;

        [SerializeField] private LayerMask layerMask;

        [SerializeField] protected float obstaclesCheckRayDistance;
        [SerializeField] protected float floorCheckRayDistance;

        private bool _isHitted;
        protected float _slopeThreshlold = 2f;  

        protected event Action UpdateTimer;

        protected Rigidbody2D _rigidbody;
        protected Collider2D _collider;
    
        protected bool _isGoingRight = true;

        private void OnEnable()
        {
            OnTakeDamage += () =>
            {
                _isHitted = true;
            };
        }
        
        private void OnDisable()
        {

        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            SetVelocityX(speed);

            _collider = GetComponent<Collider2D>();
        }

        protected void SetVelocityX(float speedX)
        {
            _rigidbody.velocity = new Vector2( speedX,_rigidbody.velocity.y);
            SetSpriteRotation();
        }
        
        protected void SetSpeed()
        {
            var direction = _isGoingRight ? 1 : -1;
            SetVelocityX(direction * speed);
        }

        private void SetSpriteRotation()
        {
            var myTransform = transform;
            var rotation = myTransform.rotation;
            rotation = _isGoingRight ? new Quaternion(rotation.x, 0 ,rotation.z, rotation.w) : new Quaternion(rotation.x, 180 ,rotation.z, rotation.w);
            myTransform.rotation = rotation;
        }

        protected bool CheckObstaclesInFront()
        {
            var bounds = _collider.bounds;
            var direction = _isGoingRight ? 1 : -1;
            var rayPositionFront = new Vector3
            {
                x = _isGoingRight ? bounds.max.x : bounds.min.x,
                y = bounds.center.y //right position
            };
            Debug.DrawRay(rayPositionFront, Vector3.right * (direction * obstaclesCheckRayDistance), Color.green);
            RaycastHit2D hitFront = Physics2D.Raycast(rayPositionFront, Vector2.right * direction, obstaclesCheckRayDistance, layerMask);

            return hitFront.transform != null;
        }
        
        protected bool CheckUnder()
        {
            var bounds = _collider.bounds;
            var rayPosition = new Vector3
            {
                x = _isGoingRight ? bounds.max.x : bounds.min.x,
                y = bounds.min.y //down position
            };
            Debug.DrawRay(rayPosition, Vector3.down * floorCheckRayDistance, Color.magenta);
            RaycastHit2D hitDown = Physics2D.Raycast(rayPosition, Vector2.down, floorCheckRayDistance, layerMask);
            SlopeBehavior(Vector2.Angle(hitDown.normal, Vector2.up));
            return hitDown.transform != null; //something under me = yes
        }
        protected void SlopeBehavior(float slopeAngle)
        {
            if (slopeAngle > _slopeThreshlold)
            {
                if (Mathf.Abs(_rigidbody.velocity.x) < 0.1f)
                {
                    _isGoingRight = !_isGoingRight;
                    SetSpeed();
                }
            }
            
        }

        protected override void CollisionBehavior(Collision2D other)
        {
            _isHitted = false;
        }

       
        protected void Patrol()
        {
            if (CheckObstaclesInFront() || !CheckUnder())
            {
                _isGoingRight = !_isGoingRight;
            }
            SetSpeed();
        }

        
        protected virtual void Behavior()
        {
            Patrol();
        }
        
        protected void Update()
        {
            UpdateTimer?.Invoke();
            
            if (!CheckUnder()&& Mathf.Abs(_rigidbody.velocity.y)>0.2f) return;//if flying
            if(_isHitted) return;
            
            Behavior();
        }
    }
}
