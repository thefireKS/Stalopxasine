using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private Enemy enemy; 
    private EnemyAI enemyAI;
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }
    private void Update()
    {
        if (enemyAI.isFacingLeft)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
            enemy.canSeePlayer = true;
    }
    private void OnTriggerExit2D(Collider2D col)
    { 
        enemy.canSeePlayer = false;
    }
}
