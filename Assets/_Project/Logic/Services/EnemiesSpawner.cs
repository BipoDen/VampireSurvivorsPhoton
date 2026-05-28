using System;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Factories;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Logic.Services
{
    public class EnemiesSpawner : ITickable
    {
        private readonly SpawnPositionProvider _positionProvider;
        private readonly EnemyFactory _factory;
        private readonly SpawnConfig _config;
        private bool _isReady;
        private bool _isSpawning;

        public EnemiesSpawner(SpawnPositionProvider positionProvider, EnemyFactory factory, SpawnConfig config)
        {
            _positionProvider = positionProvider;
            _factory = factory;
            _config = config;
        }
        
        public void Initialize()
        {
            _isReady = true;
        }

        public void Tick()
        {
            if (!_isReady) 
                return;
            
            if (!_isSpawning)
                SpawnEnemy();
        }

        private async UniTask SpawnEnemy()
        {
            _isSpawning = true;
            if (_positionProvider.TryGetPosition(out var position))
            {
                _factory.CreateEnemy(position);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_config.SpawnDelay));
            _isSpawning = false;
        }
    }
}