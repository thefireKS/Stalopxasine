using System.Collections;
using UnityEngine;

public class EnemyTurret : EnemyBase
{
    [SerializeField] private RangedProjectile projectile;

    [SerializeField] private float timeBetweenShots;
    [SerializeField] private Transform shotPoint;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        Instantiate(projectile, shotPoint.position, shotPoint.rotation);
        yield return new WaitForSeconds(timeBetweenShots);
        StartCoroutine(Shoot());
    }
}
