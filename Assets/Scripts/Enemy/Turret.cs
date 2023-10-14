using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Turret : Base
    {
        [SerializeField] protected RangedProjectile projectile;

        [SerializeField] protected float timeBetweenShots;
        [SerializeField] protected Transform shotPoint;

        protected virtual void Start()
        {
            StartCoroutine(Shoot());
        }

        protected virtual IEnumerator Shoot()
        { 
            Instantiate(projectile, shotPoint.position, shotPoint.rotation);
            
            yield return new WaitForSeconds(timeBetweenShots);
            
            StartCoroutine(Shoot());
        }
    }
}
