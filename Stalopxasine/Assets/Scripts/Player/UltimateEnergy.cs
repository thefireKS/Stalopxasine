using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateEnergy : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spr;
    private Rigidbody2D rb2d;
    private PlayerController plc;

    public float AnimationTime;
    public int Energy = 2;
    public int FullEnergy = 8;

    public static Action<int, int> OnEnergyChanged;
    private void Start()
    {
        OnEnergyChanged?.Invoke(Energy, FullEnergy);
        plc = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
        spr = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void OnEnable() => EnemyHP.GiveEnergy += SetEnergy;
    private void OnDisable() => EnemyHP.GiveEnergy -= SetEnergy;

    private void Update()
    {
        if (Energy == FullEnergy)
        {
            if (Input.GetKeyDown("x")&&!PlayerMeeting.DialogIsGoing)
            {
                StartCoroutine(UltimateWorks());
                Energy = 0;
            }
        }
    }
    private void SetEnergy()
    {
        Energy++;
        OnEnergyChanged?.Invoke(Energy, FullEnergy);
        if (Energy > FullEnergy)
        {
            Energy = FullEnergy;
        }
    }
    
    IEnumerator UltimateWorks()
    {
        plc.enabled = false;
        anim.SetTrigger("isUlting");
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(AnimationTime);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        plc.enabled = true;
    }
}