using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    public float speed;
    public float seconds;

    private Animator _animator;

    private void OnEnable()
    {
        Debug.Log(transform.eulerAngles.z);
        _animator = GetComponentInChildren<Animator>();
        _animator?.SetFloat("Angle", transform.eulerAngles.z % 5 == 0 ? 0 : 1);
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
