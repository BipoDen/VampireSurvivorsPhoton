using System;
using System.Linq;
using _Project.Logic.Constants;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Logic.Services
{
    public class NetworkSessionService : INetworkSessionService
    {
        private NetworkRunner _runner;

        public NetworkSessionService(NetworkRunner runner)
        {
            _runner = runner;
        }

        private INetworkSceneManager GetSceneManager(NetworkRunner runner)
        {
            var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

            if (sceneManager == null)
                sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            
            return sceneManager;
        }

        private async UniTask InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionID)
        {
            var sceneManager = GetSceneManager(runner);

            runner.ProvideInput = true;
            int sceneID = SceneUtility.GetBuildIndexByScenePath(GameConstants.GAME_SCENE_PATH);
            
            var result = await runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                SessionName = sessionID,
                Scene = SceneRef.FromIndex(sceneID),
                PlayerCount = SessionConstants.PLAYER_COUNT,
                CustomLobbyName = SessionConstants.LOBBY_NAME_KEY,
                SceneManager = sceneManager
            });
        }

        public void CreateGame()
        {
            string sessionId = GenerateSessionId();
            var clientTask = InitializeNetworkRunner(_runner, GameMode.Host, sessionId);
        }

        public void JoinGame(string sessionID)
        { 
            var clientTask = InitializeNetworkRunner(_runner, GameMode.Client, sessionID);
        }
        
        private string GenerateSessionId()
        {
            var guid = Guid.NewGuid().ToString("N").ToUpper();
            int start = UnityEngine.Random.Range(0, guid.Length - SessionConstants.ID_LENGTH);
            return guid.Substring(start, SessionConstants.ID_LENGTH);
        }
    }
}