using System;
using UnityEngine;

public class UltimateEnergy : MonoBehaviour
{
    public PlayerData Data;
    private Animator anim;
    private Rigidbody2D rb2d;
    private PlayerController plc;
    [SerializeField] private GameObject ultimateAbility;
    
    private int Energy = 1;
    [HideInInspector] public bool canEndEarlier = false;
    
    private int FullEnergy = 4;
    private float ultimateTime;
    private bool currentUltimateExists;

    private float realTimeElapsed = 0f;

    public static Action<int, int> OnEnergyChanged;
    private void Start()
    {
        ultimateTime = Data.ultimateTime;
        FullEnergy = Data.fullEnergy;
        
        OnEnergyChanged?.Invoke(Energy, FullEnergy);
        plc = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
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
                StartUltimate();
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

    private void StartUltimate()
    {
        Debug.Log("Ult called");
        anim.SetBool("isUlting",true);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        plc.enabled = false;
    }

    public void EnableQuickTimeEvent()
    {
        if(currentUltimateExists) return;
        Debug.Log("QTE enabled");
        currentUltimateExists = true;
        realTimeElapsed = 0f;
        Time.timeScale = 0.25f;
        Cursor.visible = false;
        ultimateAbility.gameObject.SetActive(true);
    }

    private void UltimateChecks()
    {
        realTimeElapsed += Time.unscaledDeltaTime;

        if (realTimeElapsed > ultimateTime || canEndEarlier)
        {
            EndUltimate();
            Debug.Log("Earlyend was called");
        }
    }

    private void EndUltimate()
    {
        Debug.Log("End");
        Time.timeScale = 1f;
        anim.SetBool("isUlting",false);
        ultimateAbility.gameObject.SetActive(false);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        plc.enabled = true;
        Cursor.visible = true;
        canEndEarlier = false;
        currentUltimateExists = false;
        realTimeElapsed = 0f;
    }
    
}