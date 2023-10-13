using UnityEngine;

namespace Enemy
{
    public class Obstacle : MonoBehaviour, IDealDamage
    {
        [SerializeField] private int damage;
    
        public void DealDamage(int dmg, IDamageable target)
        {
            target.TakeDamage(dmg);
        }
    }
}
