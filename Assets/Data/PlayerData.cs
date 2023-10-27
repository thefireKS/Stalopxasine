using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Game/Characters/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Profile Data")]
    public int maxHealth;
    public AnimatorOverrideController controller;
    public LayerMask layerMask;
    public SpriteTrailData trail;
    public LocalizedString uiDescription;
    
    [Header("Fighting")]
    public float attackTime = 0.4f;
    public GameObject bullet;
    public GameObject ultimateObject;
    
    [Header("Movement")]
    public float speed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCoyoteTime;
    public float jumpBufferTime;
    public float fallGravityMultiplier;
}
