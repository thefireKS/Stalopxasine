using UnityEngine;

namespace Enemy
{
    public class Obstacle : MonoBehaviour, IDealDamage
    {
        [SerializeField] protected int damage;
    
        public void DealDamage(int dmg, IDamageable damageable)
        {
            damageable.TakeDamage(dmg);
        }

        protected virtual void CollisionBehavior(Collision2D other)
        {
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
                DealDamage(damage, damageable);
            CollisionBehavior(other);
        }
    }
}
