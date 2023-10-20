using System.Mission.Objectives.Base;

namespace System.Mission.Objectives
{
    public class KillEnemies : QuantityObjective
    {
        private Enemy.Base[] _enemies;

        private void Awake()
        {
            // Note: can be expansive for many Enemies | Possible solution -> set enemies in the Inspector
            _enemies = FindObjectsOfType<Enemy.Base>();
            _targetCount = (uint)_enemies.Length;

            foreach (var enemy in _enemies)
            {
                enemy.onDeath += AddCount;
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in _enemies)
            {
                enemy.onDeath -= AddCount;
            }
        }
    }
}
