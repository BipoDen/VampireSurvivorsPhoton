using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Factories
{
    public class ProjectileFactory
    {
        private NetworkPrefabRef _projectilePrefab;
        private ProjectileConfig _projectileConfig;
        private INetworkSessionService _sessionService;
        private NetworkRunner _runner;

        public ProjectileFactory(ProjectileConfig projectileConfig, INetworkSessionService sessionService)
        {
            _projectilePrefab = projectileConfig.ProjectilePrefab;
            _projectileConfig = projectileConfig;
            _sessionService = sessionService;
        }

        public void CreateProjectile(Transform startPosition, Transform target, float damage)
        {
            var runner = _sessionService.Runner;
            if(!runner.IsServer)
                return;
            
            NetworkProjectile projectile = runner.Spawn(_projectilePrefab, startPosition.position, Quaternion.identity).GetComponent<NetworkProjectile>();
            Vector2 direction = (target.position - startPosition.position).normalized;
            projectile.Initialize(direction, _projectileConfig.Speed, _projectileConfig.LifeTime, damage);
        }
    }
}