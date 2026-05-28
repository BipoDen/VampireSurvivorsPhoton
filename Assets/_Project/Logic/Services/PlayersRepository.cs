using System.Collections.Generic;
using _Project.Logic.Entities.Enemy;
using _Project.Logic.Entities.Player;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Services
{
    public class PlayersRepository
    {
        private Dictionary<PlayerRef, NetworkPlayer> _players = new();
        public IReadOnlyDictionary<PlayerRef, NetworkPlayer> Players => _players;

        public void RegisterPlayer(PlayerRef player, NetworkPlayer playerObj)
        {
            Debug.Log($"Registering player {player}");
            if (_players.ContainsKey(player))
            {
                _players[player] = playerObj;
                return;
            }
            _players.Add(player, playerObj);
        }

        public void UnregisterPlayer(PlayerRef player)
        {
            Debug.Log($"Unregistering player {player}");
            _players.Remove(player);
        }
    }
}