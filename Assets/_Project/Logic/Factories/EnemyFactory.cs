using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities.Enemy;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Factories
{
    public class EnemyFactory
    {
        private readonly NetworkRunner _runner;
        private readonly EnemiesRepository _repository;
        private readonly EnemyConfig _config;

        public EnemyFactory(NetworkRunner runner, EnemiesRepository repository, EnemyConfig config)
        {
            _runner = runner;
            _repository = repository;
            _config = config;
        }

        public void CreateEnemy(Vector2 position)
        {
            if(!_runner.IsServer)
                return;
            
            var enemy = _runner.Spawn(_config.EnemyPrefab, position, Quaternion.identity).GetComponent<NetworkEnemy>();
            _repository.RegisterEnemy(enemy);
            enemy.OnDeath += Despawn;

            void Despawn()
            {
                _repository.UnregisterEnemy(enemy);
                enemy.OnDeath -= Despawn;
            }
        }
    }
}