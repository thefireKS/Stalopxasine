using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer sprite;

    Transform PlayerPos;
    [SerializeField]
    Transform CastPos;
    [SerializeField]
    float AgroRange;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float AgroTime;

    Rigidbody2D rb2d;
    Vector3 Scale;
    Vector2 endPos;

    private EnemyAI enemyai;
    bool isAgro;
    bool isSearching;
    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        enemyai = GetComponent<EnemyAI>();
        Scale = transform.localScale;
    }
    private void Start()
    {
        PlayerPos = Globals.CreatedCharacter.transform;
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        //Agression on Player 
        float distToPlayer = Vector2.Distance(transform.position, PlayerPos.position);
 

        if (CanSeePlayer(AgroRange))
        {
             isAgro = true;

        }
        else
        {
            if (isAgro)
            {

                if (!isSearching)
                {
                    isSearching = true;
                    Invoke("StopChasingPlayer", AgroTime);
                }
            }
        }
        if (isAgro)
            ChasePlayer();
    }
    void ChasePlayer()
    {
        enemyai.enabled = false;


        if (transform.position.x < PlayerPos.position.x)
        {
            sprite.flipX = false;
            
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            enemyai.Direction = "right";
            
        }
        else
        {
            sprite.flipX = true;
            
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            enemyai.Direction = "left";
        }

        if (enemyai.isNearEdge())
            rb2d.velocity = new Vector2(rb2d.velocity.x,rb2d.velocity.y);

    }

    void StopChasingPlayer()
    {
        isSearching = false;
        isAgro = false;
        rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
        enemyai.enabled = true;
    }

    bool CanSeePlayer(float distance)
    {
        bool value = false;
        float CastDistance = distance;

        
        if (enemyai.Direction == "right")
            endPos = CastPos.position + Vector3.right * CastDistance;
        else if (enemyai.Direction == "left")
            endPos = CastPos.position + Vector3.left * CastDistance;

        RaycastHit2D hit = Physics2D.Linecast(CastPos.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                value = true;
            }
            else
            {
                value = false;
            }
            Debug.DrawLine(CastPos.position, hit.point, Color.green);
        }
        else
            Debug.DrawLine(CastPos.position, endPos, Color.red);

        return value;
    }

}