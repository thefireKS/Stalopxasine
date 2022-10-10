using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Object = System.Object;

public class EnemyVision : MonoBehaviour
{
    private Enemy enemy; 
    private EnemyAI enemyai;

    private Vector3 leftSide = new Vector3(-1, 1, 1);
    private Vector3 rightSide = new Vector3(1, 1, 1);

    private Transform player;
    //private Timer timer = new Timer(500);
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyai = GetComponentInParent<EnemyAI>();
        //timer.Elapsed += Rotate;
        //timer.AutoReset = false;
    }
    private void Update()
    {
        transform.localScale = enemyai.isFacingLeft ? leftSide : rightSide;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        enemy.canSeePlayer = true;
        player = col.transform;
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        enemy.canSeePlayer = false;
        //timer.Enabled = true;
        Invoke("Rotate", 0.5f);
    }

    private void Rotate() //Object source, ElapsedEventArgs e
    {
        if((enemyai.isFacingLeft && player.transform.position.x > enemyai.transform.position.x) || (!enemyai.isFacingLeft && player.transform.position.x < enemyai.transform.position.x))
            enemyai.ChangeFacingDirection();
        //timer.Enabled = false;
    }   
}
