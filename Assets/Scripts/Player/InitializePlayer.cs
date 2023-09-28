using System.Threading.Tasks;
using Memory_Slots;
using UnityEngine;

namespace Player
{
    public class InitializePlayer : MonoBehaviour
    {
        #if UNITY_EDITOR

        [SerializeField] private PlayerData _playerData;

        [SerializeField] private bool _useCustomData;

        private void Awake()
        {
            FindObjectOfType<InitializeLevel>().enabled = false;
            if(_useCustomData) Initialize(_playerData);
        }

        #endif
        public async void Initialize(PlayerData playerData)
        {
            await InitializePlayerInputHandler();
            await InitializePlayerController(playerData);
            await InitializePlayerHealth(playerData);
            await InitializePlayerAttack(playerData);
            await InitializePlayerUltimateSystem(playerData);
            await InitializeAnimatorController(playerData);
            await InitializeUpgrades();
            //await InitializeSpriteTrailRenderer(playerData);
            Debug.Log("Initialize: Complete!");
            Destroy(this);
        }

        private Task InitializePlayerInputHandler()
        { 
            gameObject.AddComponent<PlayerInputHandler>();
            Debug.Log("Initalize: Initialize PlayerInputHandler complete!");
            return Task.CompletedTask;
        }

        private Task InitializePlayerController(PlayerData playerData)
        {
            var playerController = gameObject.AddComponent<PlayerController>();

            playerController.Initialize(playerData.speed, playerData.jumpBufferTime, playerData.jumpForce, playerData.fallGravityMultiplier, playerData.jumpCoyoteTime, playerData.layerMask);

            Debug.Log("Initialize: Initialize PlayerController complete!");
            return Task.CompletedTask;
        }
        
        private Task InitializePlayerHealth(PlayerData playerData)
        {
            var playerHealth = gameObject.AddComponent<PlayerHealth>();
            playerHealth.Initialize(playerData.maxHealth);
            Debug.Log("Initialize: Initialize PlayerHealth complete!");
            return Task.CompletedTask;
        }

        private Task InitializePlayerAttack(PlayerData playerData)
        {
            var playerAttack = gameObject.AddComponent<PlayerAttack>();
            playerAttack.Initialize(playerData.bullet, playerData.attackTime);
            Debug.Log("Initialize: Initialize PlayerAttack complete!");
            return Task.CompletedTask;
        }

        private Task InitializePlayerUltimateSystem(PlayerData playerData)
        {
            var playerUltimateSystem = gameObject.AddComponent<PlayerUltimateSystem>();
            playerUltimateSystem.Initialize(playerData.ultimateObject);
            Debug.Log("Initialize: Initialize PlayerUltimateSystem complete!");
            return Task.CompletedTask;
        }

        private Task InitializeAnimatorController(PlayerData playerData)
        {
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = playerData.controller;
            Debug.Log("Initialize: Initialize AnimatorController complete!");
            return Task.CompletedTask;
        }

        private Task InitializeUpgrades()
        {
            var upgradeInventory = FindObjectOfType<UpgradeInventory>();
            upgradeInventory.ApplyAllUpgrades();
            Debug.Log("Initialize: Initialize Upgrades complete!");
            return Task.CompletedTask;
        }

        /* private Task InitializeSpriteTrailRenderer(PlayerData playerData)
        {
            var spriteTrail = gameObject.AddComponent<SpriteTrail.SpriteTrailRenderer>();
            spriteTrail.Initialize(playerData.trail);
            Debug.Log("Initialize: Initialize SpriteTrailRenderer complete!");
            spriteTrail.enabled = false;
            return Task.CompletedTask;
        } */
    }
}
