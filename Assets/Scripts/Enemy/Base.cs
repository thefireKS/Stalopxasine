using System;
using UnityEngine;

namespace Enemy
{
    public abstract class Base : Obstacle, IDamageable
    {
        [SerializeField] private int health;
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
    }
}
