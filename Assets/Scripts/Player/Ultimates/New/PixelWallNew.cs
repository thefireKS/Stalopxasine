using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PixelWallNew : UltimateAbility
{
    [SerializeField] private GameObject wallObject;
    [SerializeField] private float wallProjectileFlyingTime = 3f;
    [SerializeField] private float wallProjectileSpeed = 5f;
    [SerializeField] private int maximumUltimateSize = 8;
    
    private float ultimateSize = 0;
    private bool isNextMoveDown, isWallTurnedLeft, isWallAbleToMove;

    private Transform _wallTransform;
    private Animator _wallAnimator;
    private GameObject cachedWallObject;

    private Transform _parentTransform;
    private Rigidbody2D _rigidbody2D;
    private Animator _playerAnimator;
    private SpriteRenderer _spriteRenderer;

    private Coroutine currentWallCoroutine, currentProjectileCoroutine;

    private void OnEnable()
    {
        _playerControls = PlayerInputHandler.playerControls;
        _playerControls.Ultimates.PixelWall.started += WallEvent;
        _playerControls.Ultimates.PixelWall.performed += WallEvent;
    }

    private void OnDisable()
    {
        _playerControls.Ultimates.PixelWall.started -= WallEvent;
        _playerControls.Ultimates.PixelWall.performed -= WallEvent;
    }

    private void Update()
    {
        if(!isWallAbleToMove) return;
        _wallTransform.position += _wallTransform.right * (wallProjectileSpeed * Time.deltaTime);
    }

    public override void Initialize()
    {
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
        _playerAnimator = GetComponentInParent<Animator>();

        _parentTransform = _rigidbody2D.GetComponent<Transform>();
        
        _spriteRenderer = _parentTransform.GetComponentInChildren<SpriteRenderer>();

        cachedWallObject = Instantiate(wallObject);

        _wallTransform = cachedWallObject.GetComponent<Transform>();
        _wallAnimator = cachedWallObject.GetComponent<Animator>();
        
        cachedWallObject.SetActive(false);
    }

    public override void Activate()
    {
        ultimateSize = 0;
        isWallAbleToMove = false;
        cachedWallObject.SetActive(false);
        _wallAnimator.SetFloat("ultimateSize",0);
        
        _playerAnimator.SetBool("isUlting",true);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerControls.Player.Disable();
        
        if(currentProjectileCoroutine != null)
            StopCoroutine(currentProjectileCoroutine);
        
        currentWallCoroutine = StartCoroutine(WallTimer());
    }
    
    private IEnumerator WallTimer()
    {
        yield return new WaitForSeconds(0.875f);
        Time.timeScale = 0.25f;
        cachedWallObject.SetActive(true);
        cachedWallObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        SetWallPosition();
        yield return new WaitForSecondsRealtime(ultimateEventTime);
        currentProjectileCoroutine = StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        Time.timeScale = 1f;
        _playerAnimator.SetBool("isUlting",false);
        yield return new WaitForSeconds(0.875f);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerControls.Player.Enable();
        _playerControls.Ultimates.Disable();
        isWallAbleToMove = true;
        yield return new WaitForSeconds(wallProjectileFlyingTime);
        isWallAbleToMove = false;
        cachedWallObject.SetActive(false);
    }

    private void SetWallPosition()
    {
        float positionX;
        if (_spriteRenderer.flipX)
        {
            positionX = _parentTransform.position.x - 1;
            isWallTurnedLeft = true;
        }
        else
        {
            positionX = _parentTransform.position.x + 1;
            isWallTurnedLeft = false;
        }
        
        cachedWallObject.transform.position = new Vector3(positionX,_parentTransform.position.y);

        Debug.Log($"isWallTurnedLeft: {isWallTurnedLeft}");
        var angleZ = isWallTurnedLeft ? 180f : 0f;
        
        cachedWallObject.transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }

    private void WallEvent(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();

        if ((inputVector.y > 0 && !isNextMoveDown) || (inputVector.y < 0 && isNextMoveDown))
        {
            ultimateSize+=0.5f;
            isNextMoveDown = !isNextMoveDown;
        }

        if (ultimateSize > maximumUltimateSize)
        {
            StopCoroutine(currentWallCoroutine);
            currentProjectileCoroutine = StartCoroutine(Deactivate());
        }

        _wallAnimator.SetFloat("ultimateSize",ultimateSize);
        
        if(inputVector.x != 0)
            _spriteRenderer.flipX = !(inputVector.x > 0);

        SetWallPosition();
    }
}
