using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float extraSpeed;
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
    private void Awake()
    {
        enemyai = GetComponent<EnemyAI>();
        agressionPeriod = new WaitForSeconds(AgroTime);
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
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
    }

    private void ChasePlayer()
    {
        enemyai.enabled = false;
        int dir = enemyai.isFacingLeft ? -1 : 1;
        rb2d.velocity = new Vector2(extraSpeed * dir, rb2d.velocity.y);
    }

    private IEnumerator StopChasingPlayer()
    {
        yield return agressionPeriod;
        enemyai.enabled = true;
        isSearching = false;
        isAgro = false;
    }
}