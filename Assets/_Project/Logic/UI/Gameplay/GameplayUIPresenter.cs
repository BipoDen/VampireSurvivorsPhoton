using Fusion;
using UnityEngine;

namespace _Project.Logic.UI.Gameplay
{
    public class GameplayUIPresenter
    {
        private GameplayUIView _view;
        
        private NetworkRunner _runner;

        public GameplayUIPresenter(NetworkRunner runner)
        {
            _runner = runner;
        }
        
        public void Initialize(GameplayUIView view)
        {
            _view = view;
            _view.SetSessionID(_runner.SessionInfo.Name);
        }
    }
}