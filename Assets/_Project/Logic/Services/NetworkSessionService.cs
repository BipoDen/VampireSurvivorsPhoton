using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Logic.Constants;
using _Project.Logic.Entities;
using _Project.Logic.Entities.Enemy;
using _Project.Logic.Entities.Player;
using _Project.Logic.Multiplayer;
using _Project.Logic.Multiplayer.Gameplay;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Logic.Services
{
    public class NetworkSessionService : INetworkSessionService, IDisposable
    {
        private readonly NetworkRunner _runnerPrefab;
        private readonly NetworkRunnerCallbacksAdapter _adapter;
        private NetworkRunner _runner;
        public NetworkRunner Runner => _runner;
        
        private byte[] _connectionToken;
        
        public event Action<NetworkRunner> OnRunnerMigration;
        public event Action<int, NetworkPlayer> PlayerRestored;
        public event Action OnHostMigrationCleanUp;

        public NetworkSessionService(NetworkRunner runnerPrefab, NetworkRunnerCallbacksAdapter adapter)
        {
            _runnerPrefab = runnerPrefab;
            _adapter = adapter;
            _connectionToken = ConnectionTokenUtils.NewToken();
            adapter.HostMigration += OnHostMigration;
        }
        
        public byte[] GetConnectionToken() => _connectionToken;

        private void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            Debug.Log($"Host Migration: {hostMigrationToken}");
            
            runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);
            
            StartHostMigration(hostMigrationToken);
        }

        private void StartHostMigration(HostMigrationToken hostMigrationToken)
        {
            CreateRunner();
            var clientTask = InitializeNetworkRunnerHostMigration(_runner, hostMigrationToken);
        }

        public void CreateGame()
        {
            CreateRunner();
            string sessionId = GenerateSessionId();
            var clientTask = InitializeNetworkRunner(_runner, GameMode.Host, sessionId);
        }

        public void JoinGame(string sessionID)
        { 
            CreateRunner();
            var clientTask = InitializeNetworkRunner(_runner, GameMode.Client, sessionID);
        }

        private INetworkSceneManager GetSceneManager(NetworkRunner runner)
        {
            var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

            if (sceneManager == null)
                sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            
            return sceneManager;
        }

        public void LeaveSession()
        {
            if (_runner == null) return;

            if (_runner.IsRunning)
                _runner.Shutdown();

            Object.Destroy(_runner.gameObject);
            _runner = null;
        }

        private NetworkRunner CreateRunner()
        {
            LeaveSession();

            _runner = Object.Instantiate(_runnerPrefab);
            Object.DontDestroyOnLoad(_runner.gameObject);
            
            _runner.AddCallbacks(_adapter);
            return _runner;
        }

        private async UniTask InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionID)
        {
            var sceneManager = GetSceneManager(runner);

            runner.ProvideInput = true;
            int sceneID = SceneUtility.GetBuildIndexByScenePath(GameConstants.GAME_SCENE_PATH);
            
            var result = await runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                SessionName = "sessionID",
                Scene = SceneRef.FromIndex(sceneID),
                PlayerCount = SessionConstants.PLAYER_COUNT,
                CustomLobbyName = SessionConstants.LOBBY_NAME_KEY,
                SceneManager = sceneManager,
                HostMigrationToken = null,
                ConnectionToken = _connectionToken
            });
        }

        private async UniTask InitializeNetworkRunnerHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            var sceneManager = GetSceneManager(runner);

            runner.ProvideInput = true;
            int sceneID = SceneUtility.GetBuildIndexByScenePath(GameConstants.GAME_SCENE_PATH);
            
            var result = await runner.StartGame(new StartGameArgs
            {
                SceneManager = sceneManager,
                HostMigrationToken = hostMigrationToken,
                HostMigrationResume = HostMigrationResume
            });
        }

        private void HostMigrationResume(NetworkRunner runner)
        {
            _runner = runner;
            OnRunnerMigration?.Invoke(runner); 
            
            foreach (var resumeNO in runner.GetResumeSnapshotNetworkObjects())
            {
                var hasTRSP  = resumeNO.TryGetBehaviour<NetworkTRSP>(out var trsp);
                var position = hasTRSP ? trsp.Data.Position : Vector3.zero;
                var rotation = hasTRSP ? trsp.Data.Rotation : Quaternion.identity;
                if(resumeNO.HasInputAuthority && !runner.IsPlayerValid(resumeNO.InputAuthority))
                    Debug.Log("Dont Valid");
                runner.Spawn(resumeNO, position: position, onBeforeSpawned: (networkRunner, newNO) =>
                {
                    newNO.CopyStateFrom(resumeNO);
                    if (resumeNO.TryGetBehaviour<NetworkPlayer>(out var oldPlayerBehaviour))
                    {
                        PlayerRestored?.Invoke(oldPlayerBehaviour.OwnerToken, newNO.GetComponent<NetworkPlayer>());
                    }
                });
            }

            HostMigrationCleanUp();
        }

        public async UniTask HostMigrationCleanUp()
        {
            await UniTask.WaitForSeconds(5f);
            OnHostMigrationCleanUp?.Invoke();
        }
        
        private string GenerateSessionId()
        {
            var guid = Guid.NewGuid().ToString("N").ToUpper();
            int start = UnityEngine.Random.Range(0, guid.Length - SessionConstants.ID_LENGTH);
            return guid.Substring(start, SessionConstants.ID_LENGTH);
        }

        public void Dispose()
        {
            _adapter.HostMigration -= OnHostMigration;
        }
    }
}