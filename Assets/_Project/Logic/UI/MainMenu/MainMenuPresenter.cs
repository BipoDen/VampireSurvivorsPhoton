using System;
using _Project.Logic.Services;
using Fusion;
using Zenject;

namespace _Project.Logic.UI.MainMenu
{
    public class MainMenuPresenter : IWindowPresenter, IDisposable
    {
        private MainMenuView _view;
        private NetworkRunner _runner;
        private INetworkSessionService _sessionService;
        private IWindowSwitcher _windowSwitcher;

        public MainMenuPresenter(INetworkSessionService sessionService, IWindowSwitcher windowSwitcher)
        {
            _sessionService = sessionService;
            _windowSwitcher = windowSwitcher;
        }

        public void Initialize(MainMenuView view)
        {
            _view = view;
            
            _view.OnCreateGameButtonClicked.AddListener(OnCreateGame);
            _view.OnJoinGameButtonClicked.AddListener(OnJoinGame);
        }

        private void OnCreateGame()
        {
            _sessionService.CreateGame();
        }
        
        private void OnJoinGame()
        {
            _windowSwitcher.Show<JoinSessionPresenter>();
        }

        public void Show() => _view.Show();

        public void Hide() => _view.Hide();

        public void Dispose()
        {
            _view.OnCreateGameButtonClicked.RemoveListener(OnCreateGame);
            _view.OnJoinGameButtonClicked.RemoveListener(OnJoinGame);
        }
    }
}