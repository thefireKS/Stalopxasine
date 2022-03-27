using UnityEngine;

[CreateAssetMenu(menuName = "Game/Characters/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Fighting")]
    public bool MeleeAttack = true;
    public float AttackTime = 0.4f;
    public BulletFly Bullet;

    [Header("Movement")]
    public float Speed;
    public float Acceleration;
    public float Deceleration;
    public float VelocityPower;
    [Space(10)] 
    public float FrictionAmount;

    [Header("Jumping")]
    public float JumpForce;
    [Range(0, 1)] 
    public float JumpCutMultiplier;
    [Space(10)] 
    public float JumpCoyoteTime;
    public float JumpBufferTime;
    [Space(10)] 
    public float FallGravityMultiplier;
}
