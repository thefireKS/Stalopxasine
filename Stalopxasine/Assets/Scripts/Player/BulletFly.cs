﻿using System;
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

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        Destroy(gameObject,seconds);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
            Destroy(gameObject);
    }
}