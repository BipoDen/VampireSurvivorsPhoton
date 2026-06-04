using _Project.Logic.Entities.Enemy;
using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Services
{
    public class TargetService : ITargetService
    {
        private PlayersRepository _playersRepository;
        private EnemiesRepository _enemiesRepository;

        public TargetService(PlayersRepository playersRepository, EnemiesRepository enemiesRepository)
        {
            _playersRepository = playersRepository;
            _enemiesRepository = enemiesRepository;
        }

        public NetworkPlayer GetClosestPlayer(Vector3 from)
        {
            NetworkPlayer nearest = null;
            float bestDist = float.MaxValue;
            foreach (var playerKey in _playersRepository.Players)
            {
                var player = playerKey.Value;
                if(!player.IsAlive) continue;
                float dist = (player.transform.position - from).sqrMagnitude;
                if (dist < bestDist)
                {
                    bestDist = dist; 
                    nearest = player;
                }
            }
            return nearest;
        }

        public NetworkEnemy GetClosestEnemy(Vector3 from, float attackRadius)
        {
            NetworkEnemy nearest = null;
            float bestDist = float.MaxValue;
            float radiusSqr = Mathf.Pow(attackRadius, 2);
            foreach (var enemy in _enemiesRepository.Enemies)
            {
                if (enemy == null) continue;
                if(!enemy.IsAlive) continue;
                
                float dist = (enemy.transform.position - from).sqrMagnitude;
        
                if (dist > radiusSqr) continue;
                
                if (dist < bestDist)
                {
                    bestDist = dist; 
                    nearest = enemy;
                }
            }
            return nearest;
        }
    }
}