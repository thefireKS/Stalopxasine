using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class HolyAura : UltimateAbility
{
    [SerializeField] private Animator auraAnimator;
    [SerializeField] private int maximumUltimateSize = 8;

    private int ultimateSize = 0, defaultSortingLayer;

    private float newAngle = 0, oldAngle = 0, angleDifference = 0;

    private Rigidbody2D _rigidbody2D;
    private Animator _playerAnimator;
    private SpriteRenderer _spriteRenderer;

    private Coroutine currentAuraCoroutine;

    private void OnEnable()
    {
        _playerControls = PlayerInputHandler.PlayerControls;
        _playerControls.Ultimates.HolyAura.started += AuraEvent;
        _playerControls.Ultimates.HolyAura.performed += AuraEvent;
    }

    private void OnDisable()
    {
        _playerControls.Ultimates.HolyAura.started -= AuraEvent;
        _playerControls.Ultimates.HolyAura.performed -= AuraEvent;
    }

    public override void Initialize()
    {
        _rigidbody2D = GetComponentInParent<Rigidbody2D>();
        _playerAnimator = GetComponentInParent<Animator>();

        _spriteRenderer = _playerAnimator.GetComponentInChildren<SpriteRenderer>();
        defaultSortingLayer = _spriteRenderer.sortingOrder;
    }

    public override void Activate()
    {
        ultimateSize = 0;
        _spriteRenderer.sortingOrder = 6;
        _playerAnimator.SetBool("isUlting",true);
        auraAnimator.gameObject.SetActive(true);
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        _playerControls.Player.Disable();

        currentAuraCoroutine = StartCoroutine(AuraTimer());
    }

    private IEnumerator AuraTimer()
    {
        yield return new WaitForSeconds(0.875f);
        Time.timeScale = 0.25f;
        yield return new WaitForSeconds(ultimateEventTime * Time.timeScale);
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        Time.timeScale = 1f;
        _playerAnimator.SetBool("isUlting",false);
        yield return new WaitForSeconds(0.875f); //default animation length
        _spriteRenderer.sortingOrder = defaultSortingLayer;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerControls.Player.Enable();
        _playerControls.Ultimates.Disable();
        auraAnimator.gameObject.SetActive(false);
    }

    private void AuraEvent(InputAction.CallbackContext context)
    {
        if(PauseMenu.IsPaused) return;
        
        var inputVector = context.ReadValue<Vector2>();
        newAngle = Mathf.Atan2(inputVector.y,inputVector.x) * Mathf.Rad2Deg;

        angleDifference = newAngle - oldAngle;

        oldAngle = newAngle;
        
        if (angleDifference > 90f)
            ultimateSize++;
        
        auraAnimator.SetInteger("ultimateSize",ultimateSize);

        if (ultimateSize < maximumUltimateSize) return;
        
        StopCoroutine(currentAuraCoroutine);
        StartCoroutine(Deactivate());
    }
}
