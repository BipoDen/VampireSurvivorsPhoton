using System;
using _Project.Logic.Config;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities.Player;
using _Project.Logic.Multiplayer;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Factories
{
    public class PlayerFactory : IDisposable
    {
        private NetworkPrefabRef _playerPrefab;
        private NetworkRunnerCallbacksAdapter _runnerAdapter;
        private PlayersRepository _playersRepository;
        private PlayerConfig _playerConfig;
        
        public PlayerFactory(PlayerConfig config, NetworkRunnerCallbacksAdapter runnerAdapter,
            PlayersRepository playersRepository)
        {
            _playerConfig = config;
            _runnerAdapter = runnerAdapter;
            _playersRepository = playersRepository;
            
            _runnerAdapter.PlayerJoined += CreateNetworkPlayer;
            _runnerAdapter.PlayerLeft += DespawnNetworkPlayer;
        }

        private void CreateNetworkPlayer(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;
            
            var networkPlayer = runner.Spawn(_playerConfig.PlayerPrefab, Vector3.zero, Quaternion.identity, player).GetComponent<NetworkPlayer>();
            networkPlayer.OnDied += Despawn;

            void Despawn()
            {
                networkPlayer.OnDied -= Despawn;
                //runner.Disconnect(player);
            }
        }

        public void DespawnNetworkPlayer(NetworkRunner runner, PlayerRef player)
        {
            if(!runner.IsServer)
                return;

            _playersRepository.Players.TryGetValue(player, out var playerObject);
            
            if (playerObject != null)
            {
                runner.Despawn(playerObject.Object);
            }
        }

        public void Dispose()
        {
            _runnerAdapter.PlayerJoined -= CreateNetworkPlayer;
            _runnerAdapter.PlayerLeft -= DespawnNetworkPlayer;
        }
    }
}