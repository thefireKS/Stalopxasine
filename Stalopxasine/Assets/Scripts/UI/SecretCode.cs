using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretCode: MonoBehaviour
{
    public static string code = "";
    [SerializeField] 
    private string AnimationName;
    [SerializeField] 
    private char Key;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        code = "";
    }

    public void SecretKey()
    {
        code += Key;
        anim.Play(AnimationName);
        Destroy(gameObject,1f);
        Debug.Log(code);
    }
}
