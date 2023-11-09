using System;
using System.Threading.Tasks;
using Player.States;
using UnityEngine;

namespace Player
{
    public class InitializePlayer : MonoBehaviour
    {
        #if UNITY_EDITOR
        [SerializeField] private PlayerData _playerData;

        [SerializeField] private bool _useCustomData;

        public static event Action OnInitialized;

        private void Awake()
        {
            FindObjectOfType<InitializeLevel>().enabled = false;
            if (_useCustomData) Initialize(_playerData);
        }
        #endif
        
        public async void Initialize(PlayerData playerData)
        {
            await InitializePlayerInputHandler();
            await InitializeActionState();
            await InitializePlayerController(playerData);
            await InitializePlayerHealth(playerData);
            await InitializePlayerAttack(playerData);
            await InitializePlayerUltimateSystem(playerData);
            await InitializeAnimatorController(playerData);
            //await InitializePlayerInteract();
            //await InitializeSpriteTrailRenderer(playerData);
            Debug.Log("Initialize: Complete!");
            OnInitialized?.Invoke();
            Destroy(this);
        }

        private Task InitializePlayerInputHandler()
        {
            gameObject.AddComponent<PlayerInputHandler>();
            Debug.Log("Initialize: Initialize PlayerInputHandler complete!");
            return Task.CompletedTask;
        }
        
        private Task InitializeActionState()
        {
            gameObject.AddComponent<ActionState>();
            Debug.Log("Initialize: Initialize ActionState complete!");
            return Task.CompletedTask;
        }


        private Task InitializePlayerController(PlayerData playerData)
        {
            var playerController = gameObject.AddComponent<Controller>();

            playerController.Initialize(playerData.speed, playerData.jumpBufferTime, playerData.jumpForce,
                playerData.fallGravityMultiplier, playerData.jumpCoyoteTime, playerData.layerMask);

            Debug.Log("Initialize: Initialize PlayerController complete!");
            return Task.CompletedTask;
        }

        

        private Task InitializePlayerHealth(PlayerData playerData)
        {
            var playerHealth = gameObject.AddComponent<Health>();
            playerHealth.Initialize(playerData.maxHealth);
            Debug.Log("Initialize: Initialize PlayerHealth complete!");
            return Task.CompletedTask;
        }

        private Task InitializePlayerAttack(PlayerData playerData)
        {
            var playerAttack = gameObject.AddComponent<Combat>();
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

        /* private Task InitializeSpriteTrailRenderer(PlayerData playerData)
        {
            var spriteTrail = gameObject.AddComponent<SpriteTrail.SpriteTrailRenderer>();
            spriteTrail.Initialize(playerData.trail);
            Debug.Log("Initialize: Initialize SpriteTrailRenderer complete!");
            spriteTrail.enabled = false;
            return Task.CompletedTask;
        } */

        /*private Task InitializePlayerInteract()
        {
            gameObject.AddComponent<PlayerInteract>();
            Debug.Log("Initalize: Initialize PlayerInteract complete!");
            return Task.CompletedTask;
        }*/
    }
}