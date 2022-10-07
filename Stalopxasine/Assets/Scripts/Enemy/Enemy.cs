using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField]
    private Transform CastPos;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float AgroTime;
    
    

    private Rigidbody2D rb2d;

    private Collider2D agroVision;

    private EnemyAI enemyai;
    private Transform player;

    private WaitForSeconds agressionPeriod;

    [HideInInspector] public bool canSeePlayer;
    private bool isSearching;
    private bool isAgro;
    private float rotateCoolDown = 0;
    private void Awake()
    {
        enemyai = GetComponent<EnemyAI>();
        agressionPeriod = new WaitForSeconds(AgroTime);
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (canSeePlayer)
            isAgro = true;

        else if (isAgro&&!isSearching)
        {
            isSearching = true;
            StartCoroutine(StopChasingPlayer());
        }

        if (isAgro)
            ChasePlayer();

        rotateCoolDown += Time.deltaTime;
    }
    void ChasePlayer()
    {
        if (!canSeePlayer && rotateCoolDown > 0.5f)
        {
            rotateCoolDown = 0f;
            enemyai.ChangeFacingDirection();
            int dir = enemyai.isFacingLeft ? 1 : -1;
            rb2d.velocity = new Vector2(moveSpeed * dir, rb2d.velocity.y);
        }
    }

    private IEnumerator StopChasingPlayer()
    {
        yield return agressionPeriod;
        isSearching = false;
        isAgro = false;
    }
}