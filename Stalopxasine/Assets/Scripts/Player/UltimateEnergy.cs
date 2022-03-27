using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateEnergy : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb2d;
    private PlayerController plc;

    public float AnimationTime;
    public int Energy = 2;
    public int FullEnergy = 8;

    IEnumerator UltimateWorks()
    {
        plc.enabled = false;
        anim.SetTrigger("isUlting");
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(AnimationTime);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        plc.enabled = true;
    }

    private void Start()
    {
        plc = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Energy > FullEnergy)
        {
            Energy = FullEnergy;
        }
        if (Energy == FullEnergy)
        {
            if (Input.GetKeyDown("x")&&!PlayerMeeting.DialogIsGoing)
            {
                StartCoroutine(UltimateWorks());
                Energy = 0;
            }
        }
    }
}