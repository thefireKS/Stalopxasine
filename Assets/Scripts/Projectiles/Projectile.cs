using UnityEngine;

public abstract class Projectile : MonoBehaviour, IDealDamage
{
    [SerializeField] protected int damage;
    [SerializeField] protected float lifeTimeSeconds;

    [SerializeField] protected bool needToDestroyOnCollision = true;

    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponentInChildren<Animator>();

        var angle = (int) transform.eulerAngles.z % 10f == 5f ? 1 : 0;

        _animator?.SetFloat("Angle", angle);
        
        Destroy(gameObject, lifeTimeSeconds);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            damageable.TakeDamage(damage);
        
        if(!needToDestroyOnCollision) return;
        
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
