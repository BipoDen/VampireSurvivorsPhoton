using System;
using _Project.Logic.Services;

namespace _Project.Logic.UI.MainMenu
{
    public class JoinSessionPresenter : IWindowPresenter, IDisposable
    {
        private JoinSessionView _view;
        
        private INetworkSessionService _sessionService;
        private IWindowSwitcher _windowSwitcher;

        public JoinSessionPresenter(INetworkSessionService sessionService, IWindowSwitcher windowSwitcher)
        {
            _sessionService = sessionService;
            _windowSwitcher = windowSwitcher;
        }

        public void Initialize(JoinSessionView view)
        {
            _view = view;
            
            _view.OnJoinSessionButtonClicked.AddListener(JoinSession);
            _view.OnBackButtonClicked.AddListener(OnClose);
        }

        private void JoinSession()
        {
            _sessionService.JoinGame(_view.SessionID);
        }
        
        private void OnClose()
        {
            _windowSwitcher.Back();
        }

        public void Show() => _view.Show();

        public void Hide() => _view.Hide();

        public void Dispose()
        {
            _view.OnJoinSessionButtonClicked.RemoveListener(JoinSession);
            _view.OnBackButtonClicked.RemoveListener(OnClose);
        }
    }
}