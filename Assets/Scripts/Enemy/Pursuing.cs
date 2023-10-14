using UnityEngine;

namespace Enemy
{
    public class Pursuing : Patroling
    {
        private Transform _target;

        [SerializeField] private LayerMask targetMask;
    
        protected override void Behavior()
        {
            CheckForTarget();
            if (_target)
            {
                var direction = Mathf.Sign((transform.position - _target.position).x) ;
            
                if (direction > 0)
                {
                    SetVelocityX(-speed);
                    _isGoingRight = false;
                }
                else
                {
                    SetVelocityX(speed);;
                    _isGoingRight = true;
                }

                if (CheckObstacles())
                {
                    _target = null;
                }
            
                return;
            }

            Patrol();
        }

        private void CheckForTarget()
        {
            var bounds = _collider.bounds;
            var rayPosition = bounds.center;
            var direction = _isGoingRight ? 1 : -1;
            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.right * direction,
                rayDistanceToCheckObstacles + (bounds.max.x - bounds.center.x), targetMask);
            Debug.DrawRay(rayPosition, Vector3.right * (direction * rayDistanceToCheckObstacles), Color.yellow);
        
            if (!hit) return;
        
            if (hit.transform.CompareTag("Player"))
            {
                _target = hit.transform;
            }
        }
    }
}
