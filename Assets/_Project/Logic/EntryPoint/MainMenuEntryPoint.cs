using _Project.Logic.Multiplayer;
using _Project.Logic.Services;
using _Project.Logic.UI;
using _Project.Logic.UI.MainMenu;
using Fusion;
using Zenject;

namespace _Project.Logic.EntryPoint
{
    public class MainMenuEntryPoint : IInitializable
    {
        private NetworkRunner _runner;
        private NetworkRunnerCallbacksAdapter _adapter;
        private WindowsRepository _windowsRepository;
        private MainMenuView _mainView;
        private MainMenuPresenter _mainPresenter;
        private IWindowSwitcher _windowSwitcher;
        private JoinSessionPresenter _joinSessionPresenter;
        private JoinSessionView _joinSessionView;

        public MainMenuEntryPoint(MainMenuView mainView, MainMenuPresenter mainPresenter, WindowsRepository windowsRepository, IWindowSwitcher windowSwitcher, CreateSessionPresenter createSessionPresenter, CreateSessionView createSessionView, NetworkRunner runner, INetworkSessionService sessionService, NetworkRunnerCallbacksAdapter adapter, JoinSessionPresenter joinSessionPresenter, JoinSessionView joinSessionView)
        {
            _runner = runner;
            _adapter = adapter;
            _joinSessionPresenter = joinSessionPresenter;
            _joinSessionView = joinSessionView;
            _mainView = mainView;
            _mainPresenter = mainPresenter;
            _windowsRepository = windowsRepository;
            _windowSwitcher = windowSwitcher;
        }

        public void Initialize()
        {
            InitializeUI();
            _runner.AddCallbacks(_adapter);
        }

        private void InitializeUI()
        {
            _mainPresenter.Initialize(_mainView);
            _joinSessionPresenter.Initialize(_joinSessionView);
            
            _mainPresenter.Hide();
            _joinSessionView.Hide();
            
            _windowsRepository.Register(_mainPresenter);
            _windowsRepository.Register(_joinSessionPresenter);
            
            _windowSwitcher.Show<MainMenuPresenter>();
        }
    }
}