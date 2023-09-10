using UnityEngine;

namespace Player
{
    public class InitializeCharacter : MonoBehaviour
    {
        [SerializeField] private PlayerData characterData;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializePlayerInputHandler();
            InitializePlayerController();
            InitializePlayerHealth();
            InitializePlayerAttack();
            InitializePlayerUltimateSystem();
            InitializeAnimatorController();
            Debug.Log("Initialize: Complete!");
            Destroy(this);
        }

        private void InitializePlayerInputHandler()
        { 
            gameObject.AddComponent<PlayerInputHandler>();
            Debug.Log("Initalize: Initialize PlayerInputHandler complete!");
        }

        private void InitializePlayerController()
        {
            var playerController = gameObject.AddComponent<PlayerController>();
            playerController.SetSpeed(characterData.speed);
            playerController.SetAcceleration(characterData.acceleration);
            playerController.SetDeceleration(characterData.deceleration);
            playerController.SetJumpBufferTime(characterData.jumpBufferTime);
            playerController.SetJumpForce(characterData.jumpForce);
            playerController.SetFallGravityMultiplier(characterData.fallGravityMultiplier);
            playerController.SetJumpCoyoteTime(characterData.jumpCoyoteTime);
            
            playerController.SetLayerMask(characterData.layerMask);
            Debug.Log("Initialize: Initialize PlayerController complete!");
        }
        
        // TODO: rework this SHIT!!! => CharacterHP.cs
        private void InitializePlayerHealth()
        {
            var playerHealth = gameObject.AddComponent<PlayerHealth>();
            playerHealth.SetMaxHealth(characterData.maxHealth);
            Debug.Log("Initialize: Initialize PlayerHealth complete!");
        }

        private void InitializePlayerAttack()
        {
            var playerAttack = gameObject.AddComponent<PlayerAttack>();
            playerAttack.SetBullet(characterData.bullet);
            playerAttack.SetAttackTime(characterData.attackTime);
            playerAttack.SetBulletPositionAndRotation();
            Debug.Log("Initialize: Initialize PlayerAttack complete!");
        }

        private void InitializePlayerUltimateSystem()
        {
            var playerUltimateSystem = gameObject.AddComponent<PlayerUltimateSystem>();
            playerUltimateSystem.SetUltimateAbilityObject(characterData.ultimateObject);
            Debug.Log("Initialize: Initialize PlayerUltimateSystem complete!");
        }

        private void InitializeAnimatorController()
        {
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = characterData.controller;
            Debug.Log("Initialize: Initialize AnimatorController complete!");
        }
    }
}
