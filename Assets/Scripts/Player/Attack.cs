using UnityEngine;
using Unity.Mathematics;

public class Attack : MonoBehaviour
{
    public PlayerData Data;
    [Space(10)]
    [SerializeField] private Transform bulletPositionRotation;
    [SerializeField] private Transform actualBulletPosition;
    
    private Animator anim;
    private SpriteRenderer sr;

    private bool look;
    private int high = -1; //-2 - down, -1 - 45 deg down, 0 - middle, 1 - 45 deg up, 2 - up
    private float meleeDetection;
    private void Awake()
    {
        meleeDetection = Data.meleeAttack ? 1 : -1;
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
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
        var bullet = Instantiate(Data.bullet, actualBulletPosition.position, bulletPositionRotation.rotation);
        bullet.GetComponentInChildren<Animator>().SetFloat("Angle", bullet.transform.eulerAngles.z % 10 == 0 ? 0 : 1);
        sr.flipX = look;
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

        var angle = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward));
        var coef = Mathf.Round(angle.z / 45);
        
        bulletPositionRotation.transform.position = new Vector3(transform.position.x,transform.position.y, 0);
        bulletPositionRotation.transform.localEulerAngles = new Vector3(0,0,coef * 45);
    }

    private void DirectionCheck()
    {
        float rotation = bulletPositionRotation.transform.eulerAngles.z;

        if (rotation >= 90f && rotation < 270f)
            look = true;
        else
            look = false;
        
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