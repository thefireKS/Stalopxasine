using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulGirlNew : UltimateAbility
{
    [SerializeField] private GameObject soulGirlObject;
    [SerializeField] private float soulGirlSpeed;
    [SerializeField] private GameObject soulGirlProjectile;
    [SerializeField] private float soulGirlAttackCooldown;
    [SerializeField] private int soulGirlMaximumAttackCount;

    private Rigidbody2D _rigidbody2D;
    private Animator _playerAnimator;
    
    private int soulGirlAttacksLeft;
    private SpriteRenderer soulGirlSpriteRenderer;
    private GameObject cachedSoulGirlObject;
    private Transform[] soulGirlAttackPositions;

    private Transform _playerFollow;
    private CinemachineVirtualCamera _cinemachineMainVirtualCamera;

    private Vector2 inputVector;
    private bool isAbleToMove;
    private Coroutine cachedSoulGirlCoroutine, cachedAttackCoroutine;

    private void OnEnable()
    {
        _playerControls = PlayerInputHandler.playerControls;
        //_playerControls.Ultimates.SoulGirl.started += SoulGirlMovement;
        //_playerControls.Ultimates.SoulGirl.performed += SoulGirlMovement;
        _playerControls.Ultimates.SoulGirlAttack.performed += SoulGirlAttack;
    }

    private void OnDisable()
    {
        //_playerControls.Ultimates.SoulGirl.started -= SoulGirlMovement;
        //_playerControls.Ultimates.SoulGirl.performed -= SoulGirlMovement;
        _playerControls.Ultimates.SoulGirlAttack.performed -= SoulGirlAttack;
    }

    public override void Initialize()
    {
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
        _playerAnimator = GetComponentInParent<Animator>();

        if (Camera.main != null) _cinemachineMainVirtualCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();
        _playerFollow = transform;

        cachedSoulGirlObject = Instantiate(soulGirlObject, transform.position, quaternion.identity);

        soulGirlSpriteRenderer = cachedSoulGirlObject.GetComponent<SpriteRenderer>();
        soulGirlAttackPositions = cachedSoulGirlObject.GetComponentsInChildren<Transform>()[1..5];
        
        cachedSoulGirlObject.SetActive(false);
    }

    public override void Activate()
    {
        cachedSoulGirlObject.transform.position = transform.position;
        soulGirlAttacksLeft = soulGirlMaximumAttackCount;
        
        _playerAnimator.SetBool("isUlting",true);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerControls.Player.Disable();
        _cinemachineMainVirtualCamera.Follow = cachedSoulGirlObject.transform;

        cachedSoulGirlCoroutine = StartCoroutine(SoulGirlTimer());
    }

    private IEnumerator SoulGirlTimer()
    {
        yield return new WaitForSeconds(0.875f);
        Time.timeScale = 0.25f;
        cachedSoulGirlObject.SetActive(true);
        isAbleToMove = true;
        yield return new WaitForSecondsRealtime(ultimateEventTime);
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        Time.timeScale = 1f;
        isAbleToMove = false;
        _cinemachineMainVirtualCamera.Follow = _playerFollow;
        _playerAnimator.SetBool("isUlting",false);
        yield return new WaitForSeconds(0.875f);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerControls.Player.Enable();
        _playerControls.Ultimates.Disable();
        cachedSoulGirlObject.SetActive(false);
    }

    private void Update()
    {
        inputVector = _playerControls.Ultimates.SoulGirl.ReadValue<Vector2>();
        SoulGirlMovement();
    }

    private void SoulGirlMovement()
    {
        if(!isAbleToMove) return;
        cachedSoulGirlObject.transform.position +=
            new Vector3(inputVector.x * soulGirlSpeed * Time.unscaledDeltaTime, inputVector.y * soulGirlSpeed * Time.unscaledDeltaTime);

        soulGirlSpriteRenderer.flipX = inputVector.x < 0;
    }
    
    private void SoulGirlAttack(InputAction.CallbackContext context)
    {
        cachedAttackCoroutine ??= StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        if (soulGirlAttacksLeft <= 0) yield break;
        
        foreach (var atk in soulGirlAttackPositions)
        {
            Instantiate(soulGirlProjectile, atk.position, Quaternion.Euler(0,0,atk.rotation.eulerAngles.z));
        }
        soulGirlAttacksLeft--;

        if (soulGirlAttacksLeft <= 0)
        {
            StopCoroutine(cachedSoulGirlCoroutine);
            StartCoroutine(Deactivate());
        }
        
        yield return new WaitForSeconds(soulGirlAttackCooldown);
        cachedAttackCoroutine = null;
    }
}
