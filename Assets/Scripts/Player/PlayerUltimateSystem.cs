using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUltimateSystem : MonoBehaviour
{
    public PlayerData Data;
    private Animator anim;
    private Rigidbody2D rb2d;
    private PlayerControls _playerControls;
    [SerializeField] private GameObject ultimateAbility;
    
    private int Energy = 1;
    [HideInInspector] public bool canEndEarlier = false;
    
    private int FullEnergy = 1;
    private float ultimateTime;
    private bool currentUltimateExists;

    private float realTimeElapsed = 0f;

    public static Action<int, int> OnEnergyChanged;
    private void Awake()
    {
        _playerControls = PlayerInputHandler.playerControls;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ultimateTime = Data.ultimateTime;
        FullEnergy = Data.fullEnergy;
        
        OnEnergyChanged?.Invoke(Energy, FullEnergy);
    }

    private void OnEnable()
    {
        EnemyHP.GiveEnergy += SetEnergy;
        _playerControls.Player.UltimateSkill.started += StartUltimate;
    }

    private void OnDisable()
    {
        EnemyHP.GiveEnergy -= SetEnergy;
        _playerControls.Player.UltimateSkill.started -= StartUltimate;
    }

    private void Update()
    {
        if (currentUltimateExists)
            UltimateChecks();
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

    private void StartUltimate(InputAction.CallbackContext context)
    {
        if (Energy < FullEnergy || PlayerMeeting.DialogIsGoing || currentUltimateExists) return;
        
        OnEnergyChanged?.Invoke(Energy = 0, FullEnergy);
        
        Debug.Log(Energy);
        anim.SetBool("isUlting",true);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerControls.Player.Disable();
    }

    public void EnableQuickTimeEvent() //Used in UltimateStart animation
    {
        //if(currentUltimateExists) return;
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
        }
    }

    private void EndUltimate()
    {
        Debug.Log("End");
        Time.timeScale = 1f;
        anim.SetBool("isUlting",false);
    }

    public void DisableQuickTimeEvent() //Used in UltimateEnd animation
    {
        ultimateAbility.gameObject.SetActive(false);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerControls.Player.Enable();
        Cursor.visible = true;
        canEndEarlier = false;
        currentUltimateExists = false;
        realTimeElapsed = 0f;
    }
    
}