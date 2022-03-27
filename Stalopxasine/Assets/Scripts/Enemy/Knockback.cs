using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb2d;
    
    [SerializeField]
    private float force = 4000.0f;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void Knocking(Vector3 direction)
    {
        bool isLeft = direction.x < transform.position.x;
        int xFlip = isLeft? 1 : -1;
        rb2d.AddForce(Vector2.right * xFlip * force * Time.deltaTime);
        rb2d.AddForce(Vector2.up * force/50f);
    }
}
