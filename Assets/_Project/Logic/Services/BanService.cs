using System;
using _Project.Logic.Config;
using _Project.Logic.Multiplayer;
using _Project.Logic.Multiplayer.Gameplay;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Services
{
    public class BanService : IInitializable, IDisposable
    {
        private readonly NetworkRunnerCallbacksAdapter _adapter;
        private readonly INetworkSessionService _sessionService;
        private readonly NetworkConfig _config;

        public BanService(NetworkRunnerCallbacksAdapter adapter, INetworkSessionService sessionService, NetworkConfig config)
        {
            _adapter = adapter;
            _sessionService = sessionService;
            _config = config;
        }

        public void Initialize()
        {
            var runner = _sessionService.Runner;
            if(!runner.IsServer)
                return;
            _adapter.ConnectRequest += OnConnectRequest;
        }
        
        public void RegisterDeathAndKick(NetworkRunner runner, int tokenHash, PlayerRef player)
        {
            EnsureDeathRegistry(runner).MarkDead(tokenHash);

            if (player == runner.LocalPlayer)
                runner.Shutdown(shutdownReason: ShutdownReason.Ok);
            else if (player != PlayerRef.None)
                runner.Disconnect(player); 
        }

        private void OnConnectRequest(NetworkRunner runner,
            NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            var registry = EnsureDeathRegistry(runner);
            
            if (registry == null) { request.Refuse(); return; }

            int hash = ConnectionTokenUtils.HashToken(token);
            if (registry.IsDead(hash)) 
                request.Refuse();
            else                       
                request.Accept();
        }
        
        private DeathRegistry EnsureDeathRegistry(NetworkRunner runner)
        {
            foreach (var no in runner.GetAllNetworkObjects())
                if (no.TryGetComponent(out DeathRegistry reg))
                    return reg;

            return runner.Spawn(_config.DeathRegistry).GetComponent<DeathRegistry>();
        }
        
        public void Dispose()
        {
            _adapter.ConnectRequest -= OnConnectRequest;
        }
    }
}