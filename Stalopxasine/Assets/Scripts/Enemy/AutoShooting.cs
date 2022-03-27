using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform BulletPosition;
    public float AttackTime;
    public bool isFacingLeft;
    bool isAttacks = true;

    private void Update()
    {
        if (isAttacks)
        {
            GameObject b = Instantiate(bullet);
            b.GetComponent<BulletFly>().Shooting(isFacingLeft);
            b.transform.position = BulletPosition.transform.position;
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        isAttacks = false;
        yield return new WaitForSeconds(AttackTime);
        isAttacks = true;
    }
}