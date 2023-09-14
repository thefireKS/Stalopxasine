using System;
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
    private UltimateAbility _ultimateAbility;
    
    public static Action<int, int> OnEnergyChanged;
    private void Awake()
    {
        _playerControls = PlayerInputHandler.PlayerControls;
        _playerControls.Ultimates.Disable();
    }

    private void OnEnable()
    {
        EnemyHP.GiveEnergy += SetEnergy;
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
        EnemyHP.GiveEnergy -= SetEnergy;
        _playerControls.Player.UltimateSkill.started -= StartUltimate;
    }

    private void Update()
    {
        /* if (currentUltimateExists)
            UltimateChecks(); */
    }
    private void SetEnergy()
    {
        currentEnergy++;
        if (currentEnergy > fullEnergy)
        {
            currentEnergy = fullEnergy;
        }
        SetEnergy(currentEnergy);
    }

    private void StartUltimate(InputAction.CallbackContext context)
    {
        if (currentEnergy < fullEnergy || PlayerMeeting.DialogIsGoing || currentUltimateExists) return;
        
        SetEnergy(0);
        
        _playerControls.Ultimates.Enable();
        
        /*anim.SetBool("isUlting",true);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerControls.Player.Disable();
        */
        _ultimateAbility.Activate();
    }

    private void SetEnergy(int energyAmount)
    {
        currentEnergy = energyAmount;
        OnEnergyChanged?.Invoke(currentEnergy, fullEnergy);
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