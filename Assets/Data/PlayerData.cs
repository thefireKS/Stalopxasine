using UnityEngine;

[CreateAssetMenu(menuName = "Game/Characters/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Profile Data")]
    public int maxHealth;
    public string sceneName;
    public Color color;
    public Sprite characterSprite;
    public LayerMask layerMask;
    public AnimatorOverrideController controller;

    [Header("Fighting")]
    public bool meleeAttack = true;
    public float attackTime = 0.4f;
    public float attackForceScale = 0.6f;
    public int possibleAttacks = 1;
    public GameObject bullet;
/*
    [Header("Ultimate")] 
    public int fullEnergy = 4;
    public float ultimateTime = 8f;
*/

    public GameObject ultimateObject;
    
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
    
    /*public GameObject Spawn(Transform transform)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        Instantiate(ultimateObject, obj.transform);
        return obj;
    }*/
}
