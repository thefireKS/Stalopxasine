using UnityEngine;

public class ObstacleEnemy : MonoBehaviour, IDealDamage
{
    [SerializeField] private int damage;
    
    public void DealDamage(int dmg, IDamageable target)
    {
        
    }
}
