using System;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable, IDealDamage
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private int energyOnDeath;
    
    public void TakeDamage(int dmg)
    {
        if (dmg <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(dmg));
        }

        health -= dmg;
        
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerUltimateSystem.AddEnergy(energyOnDeath);
        Destroy(gameObject);
    }

    public void DealDamage(int dmg, IDamageable damageable)
    {
        damageable.TakeDamage(dmg);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
            DealDamage(damage, damageable);
    }
}
