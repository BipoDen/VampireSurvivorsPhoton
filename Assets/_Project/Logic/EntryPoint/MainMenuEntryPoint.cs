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
        private WindowsRepository _windowsRepository;
        private MainMenuView _mainView;
        private MainMenuPresenter _mainPresenter;
        private IWindowSwitcher _windowSwitcher;
        private JoinSessionPresenter _joinSessionPresenter;
        private JoinSessionView _joinSessionView;

        public MainMenuEntryPoint(MainMenuView mainView, 
            MainMenuPresenter mainPresenter, 
            WindowsRepository windowsRepository, 
            IWindowSwitcher windowSwitcher, 
            JoinSessionPresenter joinSessionPresenter, 
            JoinSessionView joinSessionView)
        {
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