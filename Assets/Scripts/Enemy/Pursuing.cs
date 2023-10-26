using UnityEngine;

namespace Enemy
{
    public class Pursuing : Patroling
    {
        private Transform _target;

        [SerializeField] private LayerMask targetMask;

        [SerializeField] private float pursuitTime;
        private float _pursuitTimer;

        [SerializeField] private float maxDistanceToTarget;

        [SerializeField] private float targetCheckRayDistance;
    
        protected override void Behavior()
        {
            var isTarget = CheckForTarget();
            if (_target)
            {
                if (isTarget)
                {
                    _pursuitTimer = pursuitTime;
                }
                else
                {
                    _pursuitTimer -= Time.deltaTime;
                    if (_pursuitTimer <= 0)
                    {
                        _target = null;
                        Debug.Log("Pursuing: out of time");
                        return;
                    }
                }

                var vectorToTarget = transform.position - _target.position;

                var distanceToTarget = vectorToTarget.magnitude;
                if (distanceToTarget > maxDistanceToTarget)
                {
                    _target = null;
                    Debug.Log("Pursuing: out of range");
                }
                
                var direction = Mathf.Sign((vectorToTarget).x);
            
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

                if (CheckObstaclesInFront())
                {
                    _target = null;
                }
            
                return;
            }

            Patrol();
        }

        private bool CheckForTarget()
        {
            var bounds = _collider.bounds;
            var rayPosition = bounds.center;
            var direction = _isGoingRight ? 1 : -1;
            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.right * direction,
                targetCheckRayDistance + (bounds.max.x - bounds.center.x), targetMask);
            Debug.DrawRay(rayPosition, Vector3.right * (direction * targetCheckRayDistance), Color.yellow);
        
            if (!hit) return false;
        
            if (hit.transform.CompareTag("Player"))
            {
                _target = hit.transform;
                return true;
            }

            return false;
        }
    }
}
