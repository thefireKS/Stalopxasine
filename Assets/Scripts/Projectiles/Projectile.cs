using UnityEngine;

public abstract class Projectile : MonoBehaviour, IDealDamage
{
    [SerializeField] protected int damage;
    [SerializeField] protected float lifeTimeSeconds;

    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponentInChildren<Animator>();

        var angle = (int) transform.eulerAngles.z % 10f == 5f ? 1 : 0;

        _animator?.SetFloat("Angle", angle);

        Destroy(gameObject,lifeTimeSeconds);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " // " + other.tag);

        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            damageable.TakeDamage(damage);

        if (other.CompareTag("Ground"))
            Destroy(gameObject);
        //put some particles instead lol
    }

    public void DealDamage(int dmg, IDamageable target)
    {
        target.TakeDamage(dmg);
    }
}
