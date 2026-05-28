using System;
using _Project.Logic.Config;
using _Project.Logic.Multiplayer;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
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
        private EnemiesSpawner _enemiesSpawner;
        private NetworkRunnerCallbacksAdapter _runnerAdapter;

        public GameplayEntryPoint(GameplayUIPresenter uiPresenter, 
            GameplayUIView uiView, 
            NetworkConfig config, 
            NetworkRunnerCallbacksAdapter runnerAdapter, EnemiesSpawner enemiesSpawner)
        {
            _uiPresenter = uiPresenter;
            _uiView = uiView;
            _config = config;
            _runnerAdapter = runnerAdapter;
            _enemiesSpawner = enemiesSpawner;
        }

        public void Initialize()
        {
            _enemiesSpawner.Initialize();
            InitializeUI();
            _runnerAdapter.SceneLoad += InitializeNetwork;
        }
        
        private void InitializeNetwork(NetworkRunner runner)
        {

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