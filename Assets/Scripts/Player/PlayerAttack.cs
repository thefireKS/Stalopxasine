using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public PlayerData Data;
    [Space(10)]
    [SerializeField] private Transform bulletPositionRotation;
    [SerializeField] private Transform actualBulletPosition;
    
    private Animator anim;
    //private SpriteRenderer sr;
    private PlayerControls _playerControls;
    private PlayerController _playerController;

    private bool isLookingLeft;
    private int high = 0; //-2 - down, -1 - 45 deg down, 0 - middle, 1 - 45 deg up, 2 - up

    private float angle;
    
    private bool isAutoAttacking;
    //private float meleeDetection;
    private void Awake()
    {
        //meleeDetection = Data.meleeAttack ? 1 : -1;
        anim = GetComponentInChildren<Animator>();
        //sr = GetComponentInChildren<SpriteRenderer>();

        _playerController = GetComponent<PlayerController>();
        _playerControls = PlayerInputHandler.playerControls;
    }

    private void OnEnable()
    {
        _playerControls.Player.Move.started += SetShootingPoint;
        _playerControls.Player.Move.performed += SetShootingPoint;
        
        _playerControls.Player.Attack.started += PerformAttack;
        _playerControls.Player.AutoAttack.started += EnableAutoAttack;
    }

    private void OnDisable()
    {
        _playerControls.Player.Move.started -= SetShootingPoint;
        _playerControls.Player.Move.performed -= SetShootingPoint;

        _playerControls.Player.Attack.started -= PerformAttack;
        _playerControls.Player.AutoAttack.started -= EnableAutoAttack;
    }

    private void Update()
    {
        if (isAutoAttacking)
            PerformAttack();
    }

    private void EnableAutoAttack(InputAction.CallbackContext context)
    {
        isAutoAttacking = !isAutoAttacking;
    }
    
    private void PerformAttack()
    {
        if(_playerController.actionState == PlayerController.ActionStates.Attacking) return;
        
        StartCoroutine(Attack());
        anim.SetFloat("attackDir",high);
        var bullet = Instantiate(Data.bullet, actualBulletPosition.position, bulletPositionRotation.rotation);
        bullet.GetComponentInChildren<Animator>().SetFloat("Angle", bullet.transform.eulerAngles.z % 10 == 0 ? 0 : 1);
    }
    
    private void PerformAttack(InputAction.CallbackContext context)
    {
        if(_playerController.actionState == PlayerController.ActionStates.Attacking) return;

        StartCoroutine(Attack());
        anim.SetFloat("attackDir",high);
        var bullet = Instantiate(Data.bullet, actualBulletPosition.position, bulletPositionRotation.rotation);
        bullet.GetComponentInChildren<Animator>().SetFloat("Angle", bullet.transform.eulerAngles.z % 10 == 0 ? 0 : 1);
    }

    private IEnumerator Attack()
    {
        if (PlayerMeeting.DialogIsGoing) yield break;
        
        _playerController.actionState = PlayerController.ActionStates.Attacking;
        
        PerformAttack();
        yield return new WaitForSeconds(Data.attackTime);
        _playerController.actionState = PlayerController.ActionStates.Idle;
    }
    
    /*public Vector2 AttackVector()
    {
        Vector2 direction;
        
        float posX = actualBulletPosition.position.x - transform.position.x;
        float posY = actualBulletPosition.position.y - transform.position.y;

        posX *= Data.attackForceScale * meleeDetection;
        posY *= Data.attackForceScale * meleeDetection;

        direction = new Vector2(posX, posY);
        return direction;
    }*/
    private void SetShootingPoint(InputAction.CallbackContext context)
    {
        /*Vector2Int _halfScreenDims = new Vector2Int(Screen.width / 2, Screen.height / 2);
        Vector2 mouseDirection = new Vector2(Input.mousePosition.x - _halfScreenDims.x, Input.mousePosition.y - _halfScreenDims.y);

        var angle = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward));
        var coefficient = Mathf.Round(angle.z / 45);
        */

        //Debug.Log(angle);

        var inputVector = context.ReadValue<Vector2>();
        
        //if (inputAttack.magnitude >= 0)
        angle = Mathf.Atan2(inputVector.y,inputVector.x) * Mathf.Rad2Deg;
        SetDirectionForAnimator(angle);


        var bulletPositionTransform = bulletPositionRotation.transform;
        
        bulletPositionTransform.position = new Vector3(transform.position.x,transform.position.y, 0);
        bulletPositionTransform.localEulerAngles = new Vector3(0,0,angle);
    }

    // TODO : Move this function to animator controller 
    private void SetDirectionForAnimator(float shootingPointAngle)
    {
        high = Mathf.RoundToInt(Mathf.Sin(shootingPointAngle * Mathf.Deg2Rad)* 2);
        //isLookingLeft = Mathf.Abs(shootingPointAngle) > 90f && Mathf.Abs(high) < 2;

        //sr.flipX = isLookingLeft;

        /*var rotation = bulletPositionRotation.transform.eulerAngles.z;

        isLookingLeft = rotation is >= 90f and < 270f;
        
        if (rotation < 288f && rotation >= 252f)
            high = -2;
        else if (rotation is > 198f and <= 234f || rotation < 342f && rotation >= 306f)
            high = -1;
        else if (rotation <= 18f || rotation >= 342f || rotation >= 162f && rotation <= 198f)
            high = 0;
        else if (rotation is > 18f and <= 72f || rotation < 162f && rotation >= 108f)
            high = 1;
        else if (rotation > 72f && rotation <= 108f)
            high = 2;*/
    }
}