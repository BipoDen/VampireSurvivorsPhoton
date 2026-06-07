using _Project.Logic.EntryPoint;
using _Project.Logic.Multiplayer;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
using _Project.Logic.UI;
using _Project.Logic.UI.MainMenu;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MainMenuView _mainView;
        [SerializeField] private JoinSessionView _joinView;

        public override void InstallBindings()
        {
            BindUI();
            
            Container.BindInterfacesTo<MainMenuEntryPoint>().AsSingle();
        }
        

        private void BindUI()
        {
            Container.Bind<WindowsRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<WindowSwitcher>().AsSingle();
            
            MainMenuView uiView = Container.InstantiatePrefabForComponent<MainMenuView>(_mainView, _canvas.transform);
            Container.Bind<MainMenuView>().FromInstance(uiView).AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle();
            
            JoinSessionView joinView = Container.InstantiatePrefabForComponent<JoinSessionView>(_joinView, _canvas.transform);
            Container.Bind<JoinSessionView>().FromInstance(joinView).AsSingle();
            Container.BindInterfacesAndSelfTo<JoinSessionPresenter>().AsSingle();
        }
    }
}