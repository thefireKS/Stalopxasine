﻿using System;
using Enemy;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IDealDamage
{
    [SerializeField] protected int damage;
    [SerializeField] protected float lifeTimeSeconds;

    [SerializeField] protected bool needToDestroyOnCollision = true;
    [SerializeField] protected int hitsToDestroy;

    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponentInChildren<Animator>();

        var angle = Mathf.RoundToInt(transform.eulerAngles.z % 10f) == 5 ? 1 : 0;
        
        _animator?.SetFloat("Angle", angle);
        
        Destroy(gameObject, lifeTimeSeconds);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
            
            if (damageable as Base)
            {
                var hitImpact = other.GetComponent<HitImpact>();
                hitImpact.Flash();
            }
        }
            
        
        if(other.TryGetComponent(out Knockback knockback))
            knockback.ApplyKnockback(transform.position);
        
        
        
        if(!needToDestroyOnCollision) return;

        hitsToDestroy--;
        if (hitsToDestroy <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        if (other.CompareTag("Ground"))
            Destroy(gameObject);
        //put some particles instead lol
    }

    public void DealDamage(int dmg, IDamageable target)
    {
        target.TakeDamage(dmg);
    }

    public float GetLifetime()
    {
        return lifeTimeSeconds;
    }
}
