using System;
using _Project.Logic.Config;
using _Project.Logic.EntryPoint;
using _Project.Logic.Input;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.UI.Gameplay;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameplayUIView _uiView;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private NetworkConfig _networkConfig;
        [SerializeField] private PlayerSpawner _playerSpawnerPfb;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<JoystickInput>().AsSingle();
            BindConnection();
            BindServices();
            BindUI();
            Container.BindInterfacesTo<GameplayEntryPoint>().AsSingle();
        }

        private void BindConnection()
        {
            Container.Bind<InputManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void BindServices()
        {
            
        }

        private void BindUI()
        {
            GameplayUIView uiView = Container.InstantiatePrefabForComponent<GameplayUIView>(_uiView, _canvas.transform);
            Container.Bind<GameplayUIView>().FromInstance(uiView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayUIPresenter>().AsSingle();
        }
    }
}