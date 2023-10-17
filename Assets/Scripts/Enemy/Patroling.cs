using System;
using UnityEngine;

namespace Enemy
{
    public class Patroling : Base
    {
        [SerializeField] protected float speed;

        [SerializeField] private LayerMask layerMask;

        [SerializeField] protected float rayDistanceToCheckObstacles;

        private bool _isHitted;

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

        private void SetSpriteRotation()
        {
            var myTransform = transform;
            var rotation = myTransform.rotation;
            rotation = _isGoingRight ? new Quaternion(rotation.x, 0 ,rotation.z, rotation.w) : new Quaternion(rotation.x, 180 ,rotation.z, rotation.w);
            myTransform.rotation = rotation;
        }

        protected bool CheckObstacles()
        {
            var bounds = _collider.bounds;
            var direction = _isGoingRight ? 1 : -1;
            var rayPosition = new Vector3
            {
                x = _isGoingRight ? bounds.max.x : bounds.min.x,
                y = bounds.min.y //down position
            };
            Debug.DrawRay(rayPosition, Vector3.down * rayDistanceToCheckObstacles, Color.red);
            RaycastHit2D hitDown = Physics2D.Raycast(rayPosition, Vector2.down, rayDistanceToCheckObstacles, layerMask);
            bool isAnythingUnder = hitDown.transform != null;

            rayPosition = _isGoingRight ? bounds.max : bounds.min;
            rayPosition.y = bounds.center.y; //right position
            Debug.DrawRay(rayPosition, Vector3.right * (direction * rayDistanceToCheckObstacles), Color.green);
            RaycastHit2D hitRight = Physics2D.Raycast(rayPosition, Vector2.right * direction, rayDistanceToCheckObstacles, layerMask);
            bool isAnythingForward = hitRight.transform != null;
            return !isAnythingUnder || isAnythingForward;
        }

        protected bool CheckUnder()
        {
            var bounds = _collider.bounds;
            var rayPosition = new Vector3
            {
                x = bounds.center.x,
                y = bounds.min.y //down position
            };
            Debug.DrawRay(rayPosition, Vector3.down * 0.05f, Color.magenta);
            RaycastHit2D hitDown = Physics2D.Raycast(rayPosition, Vector2.down, 0.05f, layerMask);
            return hitDown.transform != null; //something under me = yes
        }

        protected override void CollisionBehavior(Collision2D other)
        {
            _isHitted = false;
        }

        protected void Patrol()
        {
            if (CheckObstacles())
            {
                _isGoingRight = !_isGoingRight;
            }
        
            var direction = _isGoingRight ? 1 : -1;
            SetVelocityX(direction * speed);
        }

        protected virtual void Behavior()
        {
            Patrol();
        }

        

        protected void Update()
        {
            UpdateTimer?.Invoke();
            
            if (!CheckUnder()) return;
            if(_isHitted) return;
            
            Behavior();
        }
    }
}
