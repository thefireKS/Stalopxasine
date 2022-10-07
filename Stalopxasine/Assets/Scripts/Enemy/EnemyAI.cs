using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    [HideInInspector] public bool isFacingLeft = false;

    [SerializeField]
    public float speed;
    [SerializeField]
    private Transform CastPos;

    private const float baseCastDistance = 0.15f;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float velocityX = speed;

        if (isFacingLeft)
            velocityX = -speed;

        if(isNearEdge()|| isHittingWall())
            ChangeFacingDirection();

        rb2d.velocity = new Vector2(velocityX, rb2d.velocity.y);
    }

    public void ChangeFacingDirection ()
    {
        if (isFacingLeft)
            sprite.flipX = false;
        else
            sprite.flipX = true;

        isFacingLeft = !isFacingLeft;
    }

    bool isHittingWall()
    {
        bool value = false;
        float castDistance = baseCastDistance;
        var castPosition = transform.TransformPoint(CastPos.localPosition.x, CastPos.localPosition.y, CastPos.localPosition.z);
        if (isFacingLeft)
        {
            castDistance = -baseCastDistance;
            castPosition = transform.TransformPoint(-CastPos.localPosition.x, CastPos.localPosition.y, CastPos.localPosition.z);
        }
        
        Vector3 targetPos = castPosition;
        targetPos.x += castDistance;

        Debug.DrawLine(castPosition, targetPos, Color.green);

        if(Physics2D.Linecast(castPosition, targetPos, 1 << LayerMask.NameToLayer("Ground")))
            value = true;

        return value;
    }


    public bool isNearEdge()
    {
        bool value = true;
        float castDistance = baseCastDistance;
        var castPosition = transform.TransformPoint(CastPos.localPosition.x, CastPos.localPosition.y, CastPos.localPosition.z);
        if (isFacingLeft)
            castPosition = transform.TransformPoint(-CastPos.localPosition.x, CastPos.localPosition.y, CastPos.localPosition.z);

        Vector3 targetPos = castPosition;
        targetPos.y -= castDistance;

        Debug.DrawLine(castPosition, targetPos, Color.yellow);

        if(Physics2D.Linecast(castPosition, targetPos, 1 << LayerMask.NameToLayer("Ground")))
            value = false;
        

        return value;
    }
}