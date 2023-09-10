using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Material matDamaged;
    private Material matDefault;
    private Rigidbody2D rb2d;
    private PlayerController plc;
    private Animator anim;
    
    private const float DamageCoolDown = 1f; //damage getting cd
    private float NextHitTime = 0; //timer to cd of damage
    private int _currentHealth;
    private int _maxHealth;
    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public static event Action<int> OnHealthChanged;

    private WaitForSeconds Blinking = new WaitForSeconds(0.5f);
    
    private void Start()
    {
        GetMaxHealth();
        OnHealthChanged?.Invoke(_maxHealth);
        plc = GetComponent<PlayerController>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        matDamaged = Resources.Load("Damaged", typeof(Material)) as Material;
    }

    private void OnEnable()
    {
        LevelFinisher.SetMaxHealth += GetMaxHealth;
        Dieline.SetZeroHealth += GetZeroHealth;
    }

    private void OnDisable()
    {
        LevelFinisher.SetMaxHealth -= GetMaxHealth;
        Dieline.SetZeroHealth -= GetZeroHealth;
    }

    private void Update()
    {
        if (_currentHealth <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//restart
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
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
    private void GetMaxHealth()
    {
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }
    
    private void GetZeroHealth()
    {
        _currentHealth = 0;
        OnHealthChanged?.Invoke(_currentHealth);
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
    private IEnumerator GotDamaged()
    {
        _currentHealth--;
        OnHealthChanged?.Invoke(_currentHealth);
        anim.SetBool("isHitted",true);
        //rb2d.velocity = new Vector2(rb2d.velocity.x, plc.Data.jumpForce/1.6f);
        /*sr.material = matDamaged;
        yield return Blinking;
        sr.material = matDefault;
        yield return Blinking;
        sr.material = matDamaged;*/
        yield return Blinking;
        //sr.material = matDefault;
        anim.SetBool("isHitted",false);
    }
}