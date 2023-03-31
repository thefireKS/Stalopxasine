using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class fireKSWallMoving : MonoBehaviour
{
    public float speed;
    private bool facingLeft = false;
    private bool canLaunch = false;

    private Vector3 right = new Vector3(2, 0,0);
    private Vector3 left = new Vector3(-2, 0,0);

    private Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        fireKSWall.FlipObject += FlipWall;
        fireKSWall.LaunchWall += LaunchWall;
        fireKSWall.ChangeScale += UpdateScale;
    }
    private void Update()
    {
        if (canLaunch)
        {
            transform.position += transform.right * speed * Time.deltaTime;
            Destroy(gameObject,3f);
        }
    }

    private void FlipWall()
    {
        if (facingLeft)
        {
            transform.position += right;
            transform.rotation = Quaternion.Euler(0f,0f,0f);
        }
        else
        {
            transform.position += left;
            transform.rotation = Quaternion.Euler(0f,180f,0f);
        }
        facingLeft = !facingLeft;
    }
    private void LaunchWall()
    {
        canLaunch = true;
    }
    private void UpdateScale(int size)
    {
        anim.SetInteger("ultimateSize", size);
    }
    private void OnDisable()
    {
        fireKSWall.FlipObject -= FlipWall;
        fireKSWall.LaunchWall -= LaunchWall;
        fireKSWall.ChangeScale -= UpdateScale;
    }
}