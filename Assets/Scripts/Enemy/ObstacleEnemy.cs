using UnityEngine;

public class ObstacleEnemy : MonoBehaviour, IDealDamage
{
    [SerializeField] private int damage;
    
    public void DealDamage(int dmg, IDamageable target)
    {
        
    }

    public void DealDamage(int dmg)
    {
        throw new System.NotImplementedException();
    }
}
