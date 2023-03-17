using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateEnergy : MonoBehaviour
{
    public PlayerData Data;
    private Animator anim;
    private Rigidbody2D rb2d;
    private PlayerController plc;
    [SerializeField] private GameObject ultimateAbility;
    
    private int Energy = 1;
    [SerializeField] private int FullEnergy = 4;
    [HideInInspector] public bool canEndEarlier = false;

    private float animationStop = 1f;
    private float ultimateTime;
    private bool currentUltimateExists;

    private float timer = 0f;

    public static Action<int, int> OnEnergyChanged;
    private void Start()
    {
        OnEnergyChanged?.Invoke(Energy, FullEnergy);
        plc = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        ultimateTime = Data.ultimateTime;
    }
    private void OnEnable() => EnemyHP.GiveEnergy += SetEnergy;
    private void OnDisable() => EnemyHP.GiveEnergy -= SetEnergy;

    private void Update()
    {
        if (currentUltimateExists)
            UltimateChecks();
        
        if (Energy == FullEnergy)
        {
            if (Input.GetKeyDown("x")&&!PlayerMeeting.DialogIsGoing&&!currentUltimateExists)
            {
                Energy = 0;
                OnEnergyChanged?.Invoke(Energy, FullEnergy);
                timer = 0f;
                UltimateWorks();
                currentUltimateExists = true;
            }
        }
    }
    private void SetEnergy()
    {
        Energy++;
        if (Energy > FullEnergy)
        {
            Energy = FullEnergy;
        }
        OnEnergyChanged?.Invoke(Energy, FullEnergy);
    }

    private void UltimateWorks()
    {
        Cursor.visible = false;
        plc.enabled = false;
        anim.SetTrigger("isUlting");
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        ultimateAbility.gameObject.SetActive(true);
    }

    private void UltimateChecks()
    {
        timer += Time.unscaledDeltaTime;

        if (timer > animationStop)
        {
            anim.speed = 0;
            Time.timeScale = 0.25f;
        }

        if (timer > ultimateTime || canEndEarlier)
            Ending();
    }

    private void Ending()
    {
        ultimateAbility.gameObject.SetActive(false);
        Time.timeScale = 1f;
        anim.speed = 1f;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        plc.enabled = true;
        Cursor.visible = true;
        canEndEarlier = false;
        currentUltimateExists = false;
        timer = 0f;
    }
    
}