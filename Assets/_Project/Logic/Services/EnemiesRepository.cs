using System.Collections.Generic;
using _Project.Logic.Entities.Enemy;

namespace _Project.Logic.Services
{
    public class EnemiesRepository
    {
        private List<NetworkEnemy> _enemies = new();
        public IReadOnlyList<NetworkEnemy> Enemies => _enemies;
        
        public void RegisterEnemy(NetworkEnemy enemyObj)
        {
            if (_enemies.Contains(enemyObj))
                return;
            
            _enemies.Add(enemyObj);
        }

        public void UnregisterEnemy(NetworkEnemy enemyObj)
        {
            _enemies.Remove(enemyObj);
        }
    }
}