using UnityEngine;

public class EnemyPatroling : EnemyBase
{
    [SerializeField] protected float speed;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] protected float rayDistance;

    protected Rigidbody2D _rigidbody;
    protected Collider2D _collider;

    protected bool _isGoingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.velocity = new Vector2( speed,_rigidbody.velocity.y);

        _collider = GetComponent<Collider2D>();
    }

    protected bool CheckObstacles()
    {
        var bounds = _collider.bounds;
        var rayPosition = new Vector3
        {
            x = _isGoingRight ? bounds.max.x : bounds.min.x,
            y = bounds.min.y //down position
        };
        Debug.DrawRay(rayPosition, Vector3.down * rayDistance, Color.red);
        var direction = _isGoingRight ? 1 : -1;
        RaycastHit2D hitDown = Physics2D.Raycast(rayPosition, Vector2.down, rayDistance, layerMask);
        bool isAnythingUnder = hitDown.transform != null;

        rayPosition = _isGoingRight ? bounds.max : bounds.min;
        rayPosition.y = bounds.center.y; //right position
        Debug.DrawRay(rayPosition, Vector3.right * (direction * rayDistance), Color.green);
        RaycastHit2D hitRight = Physics2D.Raycast(rayPosition, Vector2.right * direction, rayDistance, layerMask);
        bool isAnythingForward = hitRight.transform != null;
        
        return !isAnythingUnder || isAnythingForward;
    }
    

    protected void Patrol()
    {
        if (CheckObstacles())
        {
            _isGoingRight = !_isGoingRight;
        }
        
        var direction = _isGoingRight ? 1 : -1;
        _rigidbody.velocity = new Vector2(direction * speed, _rigidbody.velocity.y);
    }

    protected virtual void Behavior()
    {
        Patrol();
    }

    protected void Update()
    {
        Behavior();
    }
}
