﻿using UnityEngine;

[CreateAssetMenu(menuName = "Game/Characters/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Fighting")]
    public bool meleeAttack = true;
    public float attackTime = 0.4f;
    public float attackForceScale = 0.6f;
    public int possibleAttacks = 1;
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
    public float jumpBufferTime;
    [Space(10)] 
    public float fallGravityMultiplier;
}