using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireGuitarNew : UltimateAbility
{
    [SerializeField] private GameObject fireGuitar;
    [SerializeField] private float fireGuitarSpeed;
    [SerializeField] private GameObject fireGuitarProjectile;
    [SerializeField] private float fireGuitarAttackCooldown;
    [SerializeField] private int fireGuitarMaximumAttackCount;
    
    private Rigidbody2D _rigidbody2D;
    private Animator _playerAnimator;

    private int fireGuitarAttacksLeft;
    private GameObject cachedFireGuitar;
    private Transform fireGuitarAttackPosition;
    
    private Transform _playerFollow;
    private CinemachineVirtualCamera _cinemachineMainVirtualCamera;
    
    private Vector2 inputVector;
    private bool isAbleToMove;
    private Coroutine cachedFireGuitarCoroutine, cachedAttackCoroutine;
    
    private void OnEnable()
    {
        _playerControls = PlayerInputHandler.playerControls;
        _playerControls.Ultimates.FireGuitarAttack.started += FireGuitarAttack;
    }

    private void OnDisable()
    {
        _playerControls.Ultimates.FireGuitarAttack.started -= FireGuitarAttack;
    }

    public override void Initialize()
    {
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
        _playerAnimator = GetComponentInParent<Animator>();

        if (Camera.main != null) _cinemachineMainVirtualCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();
        _playerFollow = transform;
        
        cachedFireGuitar = Instantiate(fireGuitar, transform.position, Quaternion.identity);

        fireGuitarAttackPosition = cachedFireGuitar.GetComponentsInChildren<Transform>()[1];

        cachedFireGuitar.SetActive(false);
    }

    public override void Activate()
    {
        cachedFireGuitar.transform.position = transform.position;
        fireGuitarAttacksLeft = fireGuitarMaximumAttackCount;
        
        _playerAnimator.SetBool("isUlting",true);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerControls.Player.Disable();
        _cinemachineMainVirtualCamera.Follow = cachedFireGuitar.transform;

        cachedFireGuitarCoroutine = StartCoroutine(FireGuitarTimer());
    }
    
    private IEnumerator FireGuitarTimer()
    {
        yield return new WaitForSeconds(0.875f);
        Time.timeScale = 0.25f;
        cachedFireGuitar.SetActive(true);
        isAbleToMove = true;
        yield return new WaitForSecondsRealtime(ultimateEventTime);
        StartCoroutine(Deactivate());
    }
    
    private IEnumerator Deactivate()
    {
        if (fireGuitarAttacksLeft == 0)
            yield return new WaitForSecondsRealtime(fireGuitarAttackCooldown);
        Time.timeScale = 1f;
        isAbleToMove = false;
        _cinemachineMainVirtualCamera.Follow = _playerFollow;
        _playerAnimator.SetBool("isUlting",false);
        yield return new WaitForSeconds(0.875f);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerControls.Player.Enable();
        _playerControls.Ultimates.Disable();
        cachedFireGuitar.SetActive(false);
    }
    
    private void Update()
    {
        inputVector = _playerControls.Ultimates.FireGuitar.ReadValue<Vector2>();
        FireGuitarMovement();
    }

    private void FireGuitarMovement()
    {
        if(!isAbleToMove) return;
        cachedFireGuitar.transform.position +=
            new Vector3(inputVector.x * fireGuitarSpeed * Time.unscaledDeltaTime, inputVector.y * fireGuitarSpeed * Time.unscaledDeltaTime);

        //var scaleX = inputVector.x > 0 ? 1 : -1;
        //cachedFireGuitar.transform.localScale = new Vector3(scaleX, 1);
    }
    
    private void FireGuitarAttack(InputAction.CallbackContext context)
    {
        cachedAttackCoroutine ??= StartCoroutine(Attack());
    }
    
    private IEnumerator Attack()
    {
        if (fireGuitarAttacksLeft <= 0) yield break;
        
        
        Instantiate(fireGuitarProjectile, fireGuitarAttackPosition.position, Quaternion.Euler(0,0,fireGuitarAttackPosition.rotation.eulerAngles.z));
        
        fireGuitarAttacksLeft--;

        if (fireGuitarAttacksLeft <= 0)
        {
            StopCoroutine(cachedFireGuitarCoroutine);
            StartCoroutine(Deactivate());
        }
        
        yield return new WaitForSecondsRealtime(fireGuitarAttackCooldown);
        cachedAttackCoroutine = null;
    }
}
