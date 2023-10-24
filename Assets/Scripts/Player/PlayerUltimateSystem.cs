using System;
using Player.States;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUltimateSystem : MonoBehaviour
{
    public void Initialize(GameObject ultimateObject)
    {
        SetUltimateAbilityObject(ultimateObject);
    }
    
    private GameObject _ultimateAbilityObject;

    public void SetUltimateAbilityObject(GameObject ultimateAbility)
    {
        _ultimateAbilityObject = ultimateAbility;
    }

    private int currentEnergy = 1;
    [HideInInspector] public bool canEndEarlier = false;
    
    private int fullEnergy = 1;
    private float ultimateTime;
    private bool currentUltimateExists;

    private float realTimeElapsed = 0f;

    private PlayerControls _playerControls;
    private ActionState _actionState;
    
    private UltimateAbility _ultimateAbility;
    
    public static Action<int, int> OnEnergyChanged;
    public static Action<int> AddEnergy;
    private void Awake()
    {
        _playerControls = PlayerInputHandler.PlayerControls;
        _playerControls.Ultimates.Disable();
        
        _actionState = GetComponent<ActionState>();
    }

    private void OnEnable()
    {
        //EnemyHP.GiveEnergy += SetEnergy;
        AddEnergy += SetEnergy;
        _playerControls.Player.UltimateSkill.started += StartUltimate;
    }

    private void Start()
    {
        _ultimateAbility = Instantiate(_ultimateAbilityObject, transform).GetComponent<UltimateAbility>();
        fullEnergy = _ultimateAbility.fullEnergy;
        
        Debug.Log(_ultimateAbility);
        
        _ultimateAbility.Initialize();
        SetEnergy(currentEnergy);
    }

    private void OnDisable()
    {
        //EnemyHP.GiveEnergy -= SetEnergy;
        AddEnergy -= SetEnergy;
        _playerControls.Player.UltimateSkill.started -= StartUltimate;
    }

    private void Update()
    {
        /* if (currentUltimateExists)
            UltimateChecks(); */
    }
    private void SetEnergy(int energyAmount)
    {
        currentEnergy+=energyAmount;
        if (currentEnergy > fullEnergy)
        {
            currentEnergy = fullEnergy;
        }
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        OnEnergyChanged?.Invoke(currentEnergy, fullEnergy);
    }

    private void StartUltimate(InputAction.CallbackContext context)
    {
        if (_actionState.GetState() == ActionState.States.Dialogue) return;
        if (currentEnergy < fullEnergy || PlayerMeeting.DialogIsGoing || currentUltimateExists) return;
        
        SetEnergy(-fullEnergy);
        
        _playerControls.Ultimates.Enable();
        
        /*anim.SetBool("isUlting",true);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerControls.Player.Disable();
        */
        _ultimateAbility.Activate();
    }



    public void EnableQuickTimeEvent() //Used in UltimateStart animation
    {
        //if(currentUltimateExists) return;
        /*
        Debug.Log("QTE enabled");
        currentUltimateExists = true;
        realTimeElapsed = 0f;
        Time.timeScale = 0.25f;
        Cursor.visible = false;
        ultimateAbility.gameObject.SetActive(true);
        */
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
        /*
        Debug.Log("End");
        Time.timeScale = 1f;
        anim.SetBool("isUlting",false);
        */
    }

    public void DisableQuickTimeEvent() //Used in UltimateEnd animation
    {
        /*
        ultimateAbility.gameObject.SetActive(false);
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerControls.Player.Enable();
        Cursor.visible = true;
        canEndEarlier = false;
        currentUltimateExists = false;
        realTimeElapsed = 0f;
        */
    }
    
}