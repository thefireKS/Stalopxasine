using System;
using UnityEngine;

namespace Memory_Slots
{
    [CreateAssetMenu(menuName = "Game/Characters/PlayerStats")]
    public class UpgradePlayerStats: Upgrade
    {
        [Serializable]public struct PlayerStatistics
        {
            public float Speed;
            public float JumpForce;
        }

        [Space(10)]
        [SerializeField] public PlayerStatistics PlayerStats;

        public override void ApplyUpgrade()
        {
            var playerController = FindObjectOfType<PlayerController>();
            playerController.UpgradeSpeed(PlayerStats.Speed);
            playerController.UpgradeJumpForce(PlayerStats.JumpForce);
        }
    }
}
