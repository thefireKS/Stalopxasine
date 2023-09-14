using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    public class InitializePlayer : MonoBehaviour
    {
        public async void Initialize(PlayerData playerData)
        {
            await InitializePlayerInputHandler();
            await InitializePlayerController(playerData);
            await InitializePlayerHealth(playerData);
            await InitializePlayerAttack(playerData);
            await InitializePlayerUltimateSystem(playerData);
            await InitializeAnimatorController(playerData);
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
        
        // TODO: rework this SHIT!!! => CharacterHP.cs
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
    }
}
