using System;
using System.Collections.Generic;
using _Project.Logic.Constants;
using _Project.Logic.Services;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Logic.Multiplayer
{
    public class NetworkRunnerCallbacksAdapter : INetworkRunnerCallbacks
    {
        public event Action<NetworkRunner, PlayerRef> PlayerJoined;
        public event Action<NetworkRunner, PlayerRef> PlayerLeft;
        public event Action<NetworkRunner> SceneLoad;
        public event Action<NetworkRunner, NetworkInput> Input;
        public event Action<NetworkRunner, ShutdownReason> Shutdown;
        public event Action<NetworkRunner, HostMigrationToken> HostMigration;
        public event Action<NetworkRunner, NetworkRunnerCallbackArgs.ConnectRequest, byte[]> ConnectRequest;
        
        private readonly SceneLoader _sceneLoader;

        public NetworkRunnerCallbacksAdapter(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            PlayerJoined?.Invoke(runner, player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            PlayerLeft?.Invoke(runner, player);
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (shutdownReason == ShutdownReason.HostMigration) 
                return;
            Shutdown?.Invoke(runner, shutdownReason);
            _sceneLoader.LoadScene(GameConstants.MAIN_MENU_SCENE_NAME);
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) 
        {
            runner.Shutdown();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
            ConnectRequest?.Invoke(runner, request, token);
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            Input?.Invoke(runner, input);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

        public void OnConnectedToServer(NetworkRunner runner) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            Debug.Log("OnHostMigration");
            HostMigration?.Invoke(runner, hostMigrationToken);
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            SceneLoad?.Invoke(runner);
        }

        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}