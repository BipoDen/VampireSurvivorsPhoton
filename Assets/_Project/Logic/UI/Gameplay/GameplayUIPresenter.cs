using _Project.Logic.Services;
using Fusion;
using UnityEngine;

namespace _Project.Logic.UI.Gameplay
{
    public class GameplayUIPresenter 
    {
        private GameplayUIView _view;
        
        private NetworkRunner _runner;

        public GameplayUIPresenter(INetworkSessionService sessionService)
        {
            _runner = sessionService.Runner;
        }
        
        public void Initialize(GameplayUIView view)
        {
            _view = view;
            _view.SetSessionID(_runner.SessionInfo.Name);
        }
    }
}