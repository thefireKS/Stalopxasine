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

    private readonly WaitForSecondsRealtime animationStop = new WaitForSecondsRealtime(1f);
    private WaitForSecondsRealtime ultimateTime;
    private Coroutine currentUltimate;

    public static Action<int, int> OnEnergyChanged;
    private void Start()
    {
        OnEnergyChanged?.Invoke(Energy, FullEnergy);
        plc = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        ultimateTime = new WaitForSecondsRealtime(Data.ultimateTime);
    }
    private void OnEnable() => EnemyHP.GiveEnergy += SetEnergy;
    private void OnDisable() => EnemyHP.GiveEnergy -= SetEnergy;

    private void Update()
    {
        Debug.Log(canEndEarlier);
        if (Energy == FullEnergy)
        {
            if (Input.GetKeyDown("x")&&!PlayerMeeting.DialogIsGoing)
            {
                Energy = 0;
                OnEnergyChanged?.Invoke(Energy, FullEnergy);
                currentUltimate = StartCoroutine(UltimateWorks());
            }
        }
        if (canEndEarlier)
        {
            Ending();
            StopCoroutine(currentUltimate);
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

    IEnumerator UltimateWorks()
    {
        plc.enabled = false;
        anim.SetTrigger("isUlting");
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        ultimateAbility.gameObject.SetActive(true);
        yield return animationStop;
        anim.speed = 0;
        Time.timeScale = 0.25f;
        yield return ultimateTime;
        Ending();
    }

    private void Ending()
    {
        ultimateAbility.gameObject.SetActive(false);
        Time.timeScale = 1f;
        anim.speed = 1f;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        plc.enabled = true;
        canEndEarlier = false;
    }
}