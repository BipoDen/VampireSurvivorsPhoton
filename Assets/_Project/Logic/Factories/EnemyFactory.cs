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
        private readonly DropFactory _dropFactory;

        public EnemyFactory(NetworkRunner runner, EnemiesRepository repository, EnemyConfig config, DropFactory dropFactory)
        {
            _runner = runner;
            _repository = repository;
            _config = config;
            _dropFactory = dropFactory;
        }

        public void CreateEnemy(Vector2 position)
        {
            if(!_runner.IsServer)
                return;
            
            var networkEnemy = _runner.Spawn(_config.EnemyPrefab, position, Quaternion.identity);
            var enemy = networkEnemy.GetComponent<NetworkEnemy>();
            enemy.Initialize(_config);
            enemy.OnDied += Despawn;

            void Despawn()
            {
                _dropFactory.SpawnDrop(_config.Drops, enemy.transform.position);
                _runner.Despawn(networkEnemy);
                enemy.OnDied -= Despawn;
            }
        }
    }
}