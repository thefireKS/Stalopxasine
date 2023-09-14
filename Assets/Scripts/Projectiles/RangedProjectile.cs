using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : Projectile
{
    [SerializeField] protected float speed;

    private void Update()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }
}
