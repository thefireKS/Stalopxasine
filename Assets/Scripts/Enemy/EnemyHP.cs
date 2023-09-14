using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField]
    private bool HasKnockback;
    [SerializeField]
    private float knockbackScale;
    
    private Enemy enemy;
    private EnemyAI enemyai;
    private Rigidbody2D rb2d;

    [Header("Health")]
    [SerializeField]
    private int HP;
    private int fullHP;

    [Header("GUI")]
    [SerializeField]
    private HealthBar HealthBar;
    [SerializeField]
    private GameObject Bar;
    
    public static Action GiveEnergy;
    private WaitForSeconds knockbackCD = new WaitForSeconds(0.33f);
    private IEnumerator Wait()
    {
        yield return knockbackCD;
        if(enemyai!=null)
            enemyai.enabled = true;
        if(enemy!=null)
            enemy.enabled = true;
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyai = GetComponent<EnemyAI>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        fullHP = HP;
        HealthBar.SetMaxHealth(fullHP);
    }

    private void Update()
    {
        if (HP<fullHP)
            Bar.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UltimateHitBox"))
        {
            HP -= 4;
            if (HP <= 0)
            {
                GiveEnergy?.Invoke();
                LevelLock.killedEnemies++;
                Destroy(gameObject);
            }
        }
        
        if (collision.CompareTag("RangedHitBox") || collision.CompareTag("MeleeHitBox"))
        {
            Destroy(collision.gameObject, 0.2f);
            HP--;
            if (knockbackScale > 0)
            {
                if(enemy!=null)
                    enemy.enabled = false;
                if(enemyai!=null)
                    enemyai.enabled = false;
                //Knockback(collision.transform);
                StartCoroutine(Wait());
            }

            if (HP > 0) return;
            GiveEnergy?.Invoke();
            LevelLock.killedEnemies++;
            Destroy(gameObject);
        }

        HealthBar.SetHealth(HP);
    }
}
