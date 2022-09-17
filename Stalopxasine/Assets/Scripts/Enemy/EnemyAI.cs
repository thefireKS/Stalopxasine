using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    SpriteRenderer sprite;
    bool isFacingLeft;
    Rigidbody2D rb2d;
    public string Direction = "right";

    [SerializeField]
    public float speed;
    [SerializeField]
    Transform CastPos;

    public float baseCastDistance;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float velocityX = speed;

        if (Direction == "left")
            velocityX = -speed;
        rb2d.velocity = new Vector2(velocityX, rb2d.velocity.y);

        if (isHittingWall() || isNearEdge())
        {
            if (Direction == "left")
                ChangeFacingDirection("right");
            else if(Direction == "right")
                ChangeFacingDirection("left");
        }
    }

    void ChangeFacingDirection (string newDirection)
    {
        if (newDirection == "left")
            sprite.flipX = true;
        else if(newDirection == "right")
            sprite.flipX = false;

        Direction = newDirection;
    }

    bool isHittingWall()
    {
        Vector3 castPosition;

        bool value = false;
        float castDistance = baseCastDistance;
        castPosition = transform.TransformPoint(CastPos.localPosition.x, CastPos.localPosition.y, CastPos.localPosition.z);
        if (Direction == "left")
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
        Vector3 castPosition;
        bool value = true;
        float castDistance = baseCastDistance;
        castPosition = transform.TransformPoint(CastPos.localPosition.x, CastPos.localPosition.y, CastPos.localPosition.z);
        if (Direction == "left")
        {
            castPosition = transform.TransformPoint(-CastPos.localPosition.x, CastPos.localPosition.y, CastPos.localPosition.z);
        }

        Vector3 targetPos = castPosition;
        targetPos.y -= castDistance;

        Debug.DrawLine(castPosition, targetPos, Color.yellow);

        if(Physics2D.Linecast(castPosition, targetPos, 1 << LayerMask.NameToLayer("Ground")))
            value = false;
        

        return value;
    }
}