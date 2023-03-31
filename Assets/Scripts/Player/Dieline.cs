using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dieline : MonoBehaviour
{
    public static Action SetZeroHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SetZeroHealth?.Invoke();
        
        if (other.CompareTag("Enemy"))
            Destroy(other.gameObject);
    }
}
