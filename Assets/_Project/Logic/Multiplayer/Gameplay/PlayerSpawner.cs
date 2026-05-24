using System.Collections.Generic;
using _Project.Logic.Config;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Multiplayer.Gameplay
{
    public class PlayerSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        private NetworkPrefabRef _playerPrefab;
        [Networked, Capacity(4)] private NetworkDictionary<PlayerRef, NetworkPlayer> _players => default;
        
        [Inject]
        public void Construct(NetworkRunner runner, NetworkConfig config)
        {
            _playerPrefab = config.PlayerPrefab;
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (!HasStateAuthority)
                return;
            
            NetworkObject playerObject = Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player); 
            _players.Add(player, playerObject.GetComponent<NetworkPlayer>());
        }

        public void PlayerLeft(PlayerRef player)
        {
            if(!HasStateAuthority)
                return;
            
            if (_players.TryGet(player, out NetworkPlayer playerObject))
            {
                _players.Remove(player);
                Runner.Despawn(playerObject.Object);
            }
        }
    }
}