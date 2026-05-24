using System;
using _Project.Logic.Config;
using _Project.Logic.Multiplayer;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.UI.Gameplay;
using Fusion;
using Zenject;

namespace _Project.Logic.EntryPoint
{
    public class GameplayEntryPoint : IInitializable, IDisposable
    {
        private GameplayUIPresenter _uiPresenter;
        private GameplayUIView _uiView;
        private NetworkConfig _config;
        private NetworkRunnerCallbacksAdapter _runnerAdapter;
        private InputManager _inputManager;

        public GameplayEntryPoint(GameplayUIPresenter uiPresenter, 
            GameplayUIView uiView, 
            NetworkRunner runner, 
            NetworkConfig config, 
            NetworkRunnerCallbacksAdapter runnerAdapter, 
            InputManager inputManager)
        {
            _uiPresenter = uiPresenter;
            _uiView = uiView;
            _config = config;
            _runnerAdapter = runnerAdapter;
            _inputManager = inputManager;
        }

        public void Initialize()
        {
            InitializeUI();
            _runnerAdapter.SceneLoad += InitializeNetwork;
        }
        
        private void InitializeNetwork(NetworkRunner runner)
        {
            runner.Spawn(_config.PlayerSpawnerPrefab);
            _inputManager.RegisterOnRunner();
        }

        private void InitializeUI()
        {
            _uiPresenter.Initialize(_uiView);
        }

        public void Dispose()
        {
            _runnerAdapter.SceneLoad -= InitializeNetwork;
        }
    }
}