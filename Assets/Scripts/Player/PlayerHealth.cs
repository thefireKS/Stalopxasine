using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
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
    
    private void Start()
    {
        GetMaxHealth();
        OnHealthChanged?.Invoke(_maxHealth);
        _animator = GetComponentInChildren<Animator>();
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

    private void CheckHealth()
    {
        if (_currentHealth <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            if (Time.time >= _nextHitTime)
            {
                StartCoroutine(GotDamaged());
                _nextHitTime = Time.time + DamageCoolDown;
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
            if (Time.time >= _nextHitTime)
            { 
                StartCoroutine(GotDamaged()); 
                _nextHitTime = Time.time + DamageCoolDown;
            }
        }
    }
    private IEnumerator GotDamaged()
    {
        _currentHealth--;
        CheckHealth();
        OnHealthChanged?.Invoke(_currentHealth);
        _animator.SetBool("isHitted",true);
        yield return Blinking;
        _animator.SetBool("isHitted",false);
    }
}