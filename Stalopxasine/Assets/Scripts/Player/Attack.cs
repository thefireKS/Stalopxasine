using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Attack : MonoBehaviour
{
    public PlayerData Data;
    [Space(10)]
    [SerializeField] private Transform bulletPositionRotation;
    [SerializeField] private Transform actualBulletPosition;
    
    private Animator anim;
    
    private bool isAttacking;

    private float look = -1f; //Left or Right
    private int high = -1; //-2 - down, -1 - 45 deg down, 0 - middle, 1 - 45 deg up, 2 - up
    private float meleeDetection;


    private void Awake()
    {
        meleeDetection = Data.meleeAttack ? 1 : -1;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        DirectionCheck();
        SetShootingPoint();
    }
    
    public void Attacking()
    {
        anim.SetFloat("attackDir",high);
        anim.speed = 0.375f / Data.attackTime;  
        Debug.Log(anim.speed);
        Instantiate(Data.bullet, actualBulletPosition.position, bulletPositionRotation.rotation);
        transform.localScale = new Vector3(1 * look, 1, 1);
    }

    public Vector2 AttackVector()
    {
        Vector2 direction;
        
        float posX = actualBulletPosition.position.x - transform.position.x;
        float posY = actualBulletPosition.position.y - transform.position.y;

        posX *= Data.attackForceScale * meleeDetection;
        posY *= Data.attackForceScale * meleeDetection;

        direction = new Vector2(posX, posY);
        return direction;
    }
    private void SetShootingPoint()
    {
        Vector2Int _halfScreenDims = new Vector2Int(Screen.width / 2, Screen.height / 2);
        Vector2 mouseDirection = new Vector2(Input.mousePosition.x - _halfScreenDims.x, Input.mousePosition.y - _halfScreenDims.y);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        
        Debug.DrawLine( mousePosition, transform.position, Color.green);
        
        bulletPositionRotation.transform.position = new Vector3(transform.position.x,transform.position.y, 0);
        bulletPositionRotation.transform.localEulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, mouseDirection , Vector3.forward));
    }

    private void DirectionCheck()
    {
        float rotation = bulletPositionRotation.transform.eulerAngles.z;

        if (rotation >= 90f && rotation < 270f)
            look = -1f;
        else
            look = 1f;
        
        if (rotation < 288f && rotation >= 252f)
            high = -2;
        else if (rotation > 198f && rotation <= 234f || rotation < 342f && rotation >= 306f)
            high = -1;
        else if (rotation <= 18f || rotation >= 342f || rotation >= 162f && rotation <= 198f)
            high = 0;
        else if (rotation > 18f && rotation <= 72f || rotation < 162f && rotation >= 108f)
            high = 1;
        else if (rotation > 72f && rotation <= 108f)
            high = 2;
    }
}