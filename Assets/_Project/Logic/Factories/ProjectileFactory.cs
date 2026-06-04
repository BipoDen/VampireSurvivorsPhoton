using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Factories
{
    public class ProjectileFactory
    {
        private NetworkPrefabRef _projectilePrefab;
        private ProjectileConfig _projectileConfig;
        private NetworkRunner _runner;

        public ProjectileFactory(ProjectileConfig projectileConfig, NetworkRunner runner)
        {
            _projectilePrefab = projectileConfig.ProjectilePrefab;
            _projectileConfig = projectileConfig;
            _runner = runner;
        }

        public void CreateProjectile(Transform startPosition, Transform target, float damage)
        {
            if(!_runner.IsServer)
                return;
            
            NetworkProjectile projectile = _runner.Spawn(_projectilePrefab, startPosition.position, Quaternion.identity).GetComponent<NetworkProjectile>();
            Vector2 direction = (target.position - startPosition.position).normalized;
            projectile.Initialize(direction, _projectileConfig.Speed, _projectileConfig.LifeTime, damage);
        }
    }
}