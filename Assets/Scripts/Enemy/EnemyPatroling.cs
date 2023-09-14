using UnityEngine;

public class EnemyPatroling : EnemyBase
{
    [SerializeField] protected float speed;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float rayDistance;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    private bool _isGoingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.velocity = Vector2.right * speed;

        _collider = GetComponent<Collider2D>();
    }

    private bool CheckObstacles()
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
    

    private void Patrol()
    {
        if (CheckObstacles())
        {
            Debug.Log("I found obstacle");
            _isGoingRight = !_isGoingRight;
        }
        
        var direction = _isGoingRight ? 1 : -1;
        _rigidbody.velocity = Vector2.right * (direction * speed);
    }

    private void Update()
    {
        Patrol();
    }
}
