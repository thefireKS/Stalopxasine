using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateProjectile : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " // " + other.tag);

        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            damageable.TakeDamage(damage);
    }
}
