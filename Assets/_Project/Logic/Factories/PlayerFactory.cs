using System;
using System.Collections.Generic;
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
        private readonly NetworkPrefabRef _playerPrefab;
        private readonly NetworkRunnerCallbacksAdapter _runnerAdapter;
        private readonly PlayersRepository _playersRepository;
        private readonly PlayerConfig _playerConfig;
        private readonly INetworkSessionService _sessionService;
        private readonly BanService _banService;
        
        private Dictionary<int, NetworkPlayer> _playersMap = new();
        
        public PlayerFactory(PlayerConfig config, NetworkRunnerCallbacksAdapter runnerAdapter,
            PlayersRepository playersRepository, INetworkSessionService sessionService, BanService banService)
        {
            _playerConfig = config;
            _runnerAdapter = runnerAdapter;
            _playersRepository = playersRepository;
            _sessionService = sessionService;
            _banService = banService;

            _runnerAdapter.PlayerJoined += OnPlayerJoined;
            _runnerAdapter.PlayerLeft += OnPlayerLeft;
            _sessionService.PlayerRestored += SetConnectionTokenMapping;
            _sessionService.OnHostMigrationCleanUp += HostMigrationCleanUp;
        }

        private void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;

            int playerToken = GetPlayerToken(runner, player);

            if (_playersMap.TryGetValue(playerToken, out var playerObject))
            {
                playerObject.GetComponent<NetworkObject>().AssignInputAuthority(player);
                playerObject.OnResumed();
                SubscribeDeath(runner, player, playerObject);  
                _playersRepository.RegisterPlayer(player, playerObject);
                return;
            }
            
            var networkPlayer = runner.Spawn(_playerConfig.PlayerPrefab, Vector3.zero, Quaternion.identity, player).GetComponent<NetworkPlayer>();
            networkPlayer.OwnerToken = playerToken;
            SubscribeDeath(runner, player, networkPlayer);  
            networkPlayer.Initialize();
            _playersRepository.RegisterPlayer(player, networkPlayer);
            _playersMap[playerToken] = networkPlayer;
        }
        
        private void SubscribeDeath(NetworkRunner runner, PlayerRef player, NetworkPlayer playerObject)
        {
            void Despawn()
            {
                if (!runner.IsServer) return;

                playerObject.OnDied -= Despawn;
                _banService.RegisterDeathAndKick(runner, playerObject.OwnerToken, player);
            }

            playerObject.OnDied += Despawn;
        }

        private void HostMigrationCleanUp()
        {
            Debug.Log(_playersMap.Count);
            foreach (KeyValuePair<int, NetworkPlayer> entry in _playersMap)
            {
                NetworkObject networkObject = entry.Value.GetComponent<NetworkObject>();
                Debug.Log(networkObject.InputAuthority.ToString());
                if (networkObject.InputAuthority.IsNone)
                {
                    Debug.Log($"{Time.time} Found player that has not reconnected. Despawn {entry.Value}");
                    networkObject.Runner.Despawn(networkObject);
                }
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (!runner.IsServer)
                return;
            _playersRepository.Players.TryGetValue(player, out var playerObject);
            if (playerObject != null)
            {
                _playersMap.Remove(playerObject.OwnerToken);
                _playersRepository.UnregisterPlayer(player);
                runner.Despawn(playerObject.Object);
            }
        }

        public void SetConnectionTokenMapping(int token, NetworkPlayer player)
        {
            _playersMap[token] = player;
        }

        private int GetPlayerToken(NetworkRunner runner,  PlayerRef player)
        {
            if (runner.LocalPlayer == player)
            {
                return ConnectionTokenUtils.HashToken(_sessionService.GetConnectionToken());
            }
            else
            {
                var token = runner.GetPlayerConnectionToken(player);

                if (token != null)
                    return ConnectionTokenUtils.HashToken(token);
                
                Debug.LogError($"GetPlayerToken failed");
                
                return 0;
            }
        }

        public void Dispose()
        {
            _runnerAdapter.PlayerJoined -= OnPlayerJoined;
            _runnerAdapter.PlayerLeft -= OnPlayerLeft;
            _sessionService.PlayerRestored -= SetConnectionTokenMapping;
            _sessionService.OnHostMigrationCleanUp -= HostMigrationCleanUp;
        }
    }
}