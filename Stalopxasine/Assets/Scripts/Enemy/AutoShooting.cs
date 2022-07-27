using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooting : MonoBehaviour
{
    public BulletFly bullet;
    public Transform BulletPosition;
    public float AttackTime;
    bool isAttacking = false;

    private void Update()
    {
        if (!isAttacking)
            StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        isAttacking = true;
        Instantiate(bullet,BulletPosition.position, BulletPosition.rotation);
        yield return new WaitForSeconds(AttackTime); //rotate turret to rotate bullet
        isAttacking = false;
    }
}