﻿using UnityEngine;

[CreateAssetMenu(menuName = "Game/Characters/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Profile Data")]
    public GameObject prefab;
    public string sceneName;
    public Color color;
    public Sprite characterSprite;

    [Header("Fighting")]
    public bool meleeAttack = true;
    public float attackTime = 0.4f;
    public float attackForceScale = 0.6f;
    public int possibleAttacks = 1;
    public GameObject bullet;

    [Header("Ultimate")] 
    public float ultimateTime = 8f;

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
    
    public GameObject Spawn(Transform transform)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        return obj;
    }
}