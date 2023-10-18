using Enemy;

namespace System.Mission.Objectives
{
    public class KillEnemies : Objective
    {
        private Base[] _enemies;

        private uint _enemiesCount;
        private uint _deadEnemies;

        private void Awake()
        {
            _enemies = FindObjectsOfType<Base>();
            _enemiesCount = (uint)_enemies.Length;

            foreach (var enemy in _enemies)
            {
                enemy.onDeath += AddDeadCount;
            }
        }

        private void OnDisable()
        {
            foreach (var enemy in _enemies)
            {
                enemy.onDeath -= AddDeadCount;
            }
        }

        private void AddDeadCount()
        {
            _deadEnemies++;
            CheckComplete();
        }

        protected override void CheckComplete()
        {
            if (_enemiesCount == _deadEnemies)
            {
                Complete();
            }
        }
    }
}
