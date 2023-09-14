using UnityEngine;

public class AutoShooting : MonoBehaviour
{
    [SerializeField] private Projectile bullet;
    [SerializeField] private Transform BulletPosition;
    [SerializeField] private float AttackTime;

    private float timer = 0f;
    private float randomAttackCoefficient = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < AttackTime + randomAttackCoefficient) return;
        
        Attack();
    }

    private void Attack()
    {
        timer = 0f;
        randomAttackCoefficient = Random.Range(0.1f, 0.3f);
        Instantiate(bullet,BulletPosition.position, BulletPosition.rotation);
        //rotate turret to rotate bullet
    }
}