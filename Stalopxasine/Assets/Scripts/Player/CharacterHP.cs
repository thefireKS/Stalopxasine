using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHP : MonoBehaviour
{
    private Material matDamaged;
    private Material matDefault;
    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private PlayerController plc;
    
    private const float DamageCoolDown = 1f; //кд получения урона
    private float NextHitTime = 0; //таймер для образования кулдауна между получением урона
    public int HP;
    public int FullHP;

    private WaitForSeconds Blinking = new WaitForSeconds(0.1f);
    private IEnumerator GotDamaged()
    {
        HP--;
        rb2d.velocity = new Vector2(rb2d.velocity.x, plc.Data.JumpForce);
        sr.material = matDamaged;
        yield return Blinking;
        sr.material = matDefault;
        yield return Blinking;
        sr.material = matDamaged;
        yield return Blinking;
        sr.material = matDefault;
    }

    private void Start()
    {
        plc = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        matDamaged = Resources.Load("Damaged", typeof(Material)) as Material;
        matDefault = sr.material;
    }

    private void Update()
    {
        if (HP <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (HP > FullHP)
            HP = FullHP;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            if (Time.time >= NextHitTime)
            {
                StartCoroutine(GotDamaged());
                NextHitTime = Time.time + DamageCoolDown;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            if (Time.time >= NextHitTime)
            { 
                StartCoroutine(GotDamaged());
              NextHitTime = Time.time + DamageCoolDown;
            }
        }
    }
}
