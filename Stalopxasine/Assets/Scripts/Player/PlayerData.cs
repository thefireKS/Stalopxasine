using UnityEngine;

[CreateAssetMenu(menuName = "Game/Characters/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Fighting")]
    public bool meleeAttack = true;
    public float attackTime = 0.4f;
    public GameObject bullet;

    [Header("Movement")]
    public float speed;
    public float acceleration;
    public float deceleration;

    [Header("Jumping")]
    public float jumpForce;
    [Space(10)] 
    public float jumpCoyoteTime;
    [Space(10)] 
    public float fallGravityMultiplier;
}
