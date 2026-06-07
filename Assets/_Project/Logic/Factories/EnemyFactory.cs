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
        private readonly EnemyConfig _config;
        private readonly DropFactory _dropFactory;
        private INetworkSessionService _sessionService;

        public EnemyFactory(INetworkSessionService sessionService, EnemyConfig config, DropFactory dropFactory)
        {
            _sessionService = sessionService;
            _config = config;
            _dropFactory = dropFactory;
        }

        public void CreateEnemy(Vector2 position)
        {
            var runner = _sessionService.Runner;
            if(!runner.IsServer)
                return;
            
            var networkEnemy = runner.Spawn(_config.EnemyPrefab, position, Quaternion.identity);
            var enemy = networkEnemy.GetComponent<NetworkEnemy>();
            enemy.Initialize(_config);
            enemy.OnDied += Despawn;

            void Despawn()
            {
                _dropFactory.SpawnDrop(_config.Drops, enemy.transform.position);
                runner.Despawn(networkEnemy);
                enemy.OnDied -= Despawn;
            }
        }
    }
}