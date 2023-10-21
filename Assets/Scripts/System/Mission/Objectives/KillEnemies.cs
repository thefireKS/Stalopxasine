using System.Mission.Objectives.Base;
using System.Threading.Tasks;
using UnityEngine;

namespace System.Mission.Objectives
{
    public class KillEnemies : QuantityObjective
    {
        [SerializeField] private bool killAll;
        [SerializeField] private Enemy.Base[] enemies;

        protected override void InitializeJob()
        {
            if (killAll) enemies = FindObjectsOfType<Enemy.Base>();
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
