using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    Rigidbody2D rbtd;
    public float speed;
    public float seconds;

    private void Awake()
    {
        rbtd = GetComponent<Rigidbody2D>();
    }

    public void Shooting(bool isFacingLeft)
    {
        if (!isFacingLeft)
            rbtd.velocity = new Vector2(speed, 0);
        else
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            rbtd.velocity = new Vector2(-speed, 0);
        }
        Destroy(gameObject, seconds);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
