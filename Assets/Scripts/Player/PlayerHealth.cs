using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void Initialize(int maxHealth)
    {
        SetMaxHealth(maxHealth);
    }
    
    private Animator _animator;
    
    private const float DamageCoolDown = 1f; //damage getting cd
    private float _nextHitTime; //timer to cd of damage
    private int _currentHealth;
    private int _maxHealth;
    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public static event Action<int> OnHealthChanged;

    private WaitForSeconds Blinking = new WaitForSeconds(0.5f);

    private void OnEnable()
    {
        //LevelFinisher.SetMaxHealth += () => SetHealth(_maxHealth);
        //Dieline.SetZeroHealth += () => SetHealth(0);
    }

    private void Start()
    {
        SetHealth(_maxHealth);
        OnHealthChanged?.Invoke(_maxHealth);
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnDisable()
    {
        //LevelFinisher.SetMaxHealth -= () => SetHealth(_maxHealth);
        //Dieline.SetZeroHealth -= () => SetHealth(0);
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetHealth(int value)
    {
        _currentHealth = value;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    /* private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            if (Time.time >= _nextHitTime)
            {
                StartCoroutine(GotDamaged());
                _nextHitTime = Time.time + DamageCoolDown;
            }
        }
    } */

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            if (Time.time >= _nextHitTime)
            { 
                StartCoroutine(GotDamaged()); 
                _nextHitTime = Time.time + DamageCoolDown;
            }
        }
    } */

    private IEnumerator GotDamaged(int damage)
    {
        _currentHealth-=damage;
        CheckHealth();
        OnHealthChanged?.Invoke(_currentHealth);
        _animator.SetBool("isHitted",true);
        yield return Blinking;
        _animator.SetBool("isHitted",false);
    }

    public void TakeDamage(int dmg)
    {
        StartCoroutine(GotDamaged(dmg));
    }
}