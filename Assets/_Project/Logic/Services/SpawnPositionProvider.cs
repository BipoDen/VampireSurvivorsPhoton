using System.Collections.Generic;
using System.Linq;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities.Player;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Services
{
    public class SpawnPositionProvider
    {
        private readonly PlayersRepository _playersRepository;
        private readonly SpawnConfig _spawnConfig;

        public SpawnPositionProvider(PlayersRepository playersRepository, SpawnConfig spawnConfig)
        {
            _playersRepository = playersRepository;
            _spawnConfig = spawnConfig;
        }

        public bool TryGetPosition(out Vector2 point)
        {
            point = default;
            var players = _playersRepository.Players;
            if (players.Count == 0) return false;

            for (int i = 0; i < _spawnConfig.MaxAttempts; i++)
            {
                var anchor = GetRandomPlayer(players);
                var candidate = GetRandomPointInRing(anchor.transform.position, _spawnConfig.MinDistance, _spawnConfig.MaxDistance);

                if (IsValid(candidate, players))
                {
                    point = candidate;
                    return true;
                }
            }
            return false;
        }
        
        private NetworkPlayer GetRandomPlayer(IReadOnlyDictionary<PlayerRef, NetworkPlayer> players)
        {
            if (players.Count == 0) return null;
    
            int index = Random.Range(0, players.Count);
            int i = 0;
            foreach (var player in players.Values)
            {
                if (i++ == index) return player;
            }
            return null;
        }
        
        private bool IsValid(Vector2 point, IReadOnlyDictionary<PlayerRef, NetworkPlayer> players)
        {
            foreach (var player in players.Values)
            {
                if (Vector2.Distance(point, player.transform.position) < _spawnConfig.MinDistance)
                    return false;
            }
            return true;
        }
        
        private Vector2 GetRandomPointInRing(Vector2 center, float minRadius, float maxRadius)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(minRadius, maxRadius);

            return center + new Vector2(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance
            );
        }
    }
}