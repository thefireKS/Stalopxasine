using System.Mission.Objectives.Base;
using UnityEngine;

namespace System.Mission.Objectives
{
    public class KillEnemies : QuantityObjective
    {
        [SerializeField] private bool killAll;
        [SerializeField] private Enemy.Base[] enemies;

        private void Awake()
        {
            // Note: can be expansive for many Enemies | Possible solution -> set enemies in the Inspector
            if(killAll)enemies = FindObjectsOfType<Enemy.Base>();
            _targetCount = (uint)enemies.Length;

            foreach (var enemy in enemies)
            {
                enemy.onDeath += AddCount;
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in enemies)
            {
                enemy.onDeath -= AddCount;
            }
        }
    }
}
