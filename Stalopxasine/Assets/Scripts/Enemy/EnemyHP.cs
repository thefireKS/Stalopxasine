using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public bool HasKnockback;
    
    private Knockback knockback;
    private Enemy enemy;
    private EnemyAI enemyai;
    private UltimateEnergy ue;

    public int HP;
    int fullHP;

    public HealthBar HealthBar;
    public GameObject Bar;

    private WaitForSeconds knockbackCD = new WaitForSeconds(0.33f);
    private IEnumerator Wait()
    {
        yield return knockbackCD;
        if(enemyai!=null)
            enemyai.enabled = true;
        if(enemy!=null)
            enemy.enabled = true;
    }
    
    private void Start()
    {
        ue = Globals.CreatedCharacter.GetComponentInChildren<UltimateEnergy>();
        fullHP = HP;
        HealthBar.SetMaxHealth(fullHP);
        knockback = GetComponent<Knockback>();
        enemy = GetComponent<Enemy>();
        enemyai = GetComponent<EnemyAI>();
    }

    private void Update()
    {
        HealthBar.SetHealth(HP);
        if(HP<fullHP)
            Bar.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UltimateHitBox"))
        {
            HP -= 4;
            if (HP <= 0)
            {
                ue.Energy++;
                LevelLock.killedEnemies++;
                Destroy(gameObject);
            }
        }
        
        if (collision.CompareTag("RangedHitBox") || collision.CompareTag("MeleeHitBox"))
        {
            if (collision.CompareTag("RangedHitBox"))
                Destroy(collision.gameObject);

            HP--;
            if (HasKnockback)
            {
                if(enemy!=null)
                    enemy.enabled = false;
                if(enemyai!=null)
                    enemyai.enabled = false;
                knockback.Knocking(collision.transform.position);
                StartCoroutine(Wait());
            }

            if (HP <= 0)
            {
                ue.Energy++;
                LevelLock.killedEnemies++;
                Destroy(gameObject);
            }
        }
    }
}
