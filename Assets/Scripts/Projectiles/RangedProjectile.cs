using UnityEngine;

public class RangedProjectile : Projectile
{
    [SerializeField] protected float speed;

    private void Update()
    {
        var myTransform = transform;
        myTransform.position += myTransform.right * (speed * Time.deltaTime);
    }
}
