using System;
using _Project.Logic.Config;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Config.Gameplay.Upgrades;
using _Project.Logic.Entities.Player;
using _Project.Logic.EntryPoint;
using _Project.Logic.Factories;
using _Project.Logic.Input;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
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
        [SerializeField] private CameraFollow _camera;
        [SerializeField] private NetworkConfig _networkConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private SpawnConfig _spawnConfig;
        [SerializeField] private ProjectileConfig _projectileConfig;
        [SerializeField] private HealthBarView _healthBarPrefab;
        [SerializeField] private HealthBarHUDView _hudHealthBarPrefab;
        [SerializeField] private ExpBarHUDView _hudExpBarPrefab;
        [SerializeField] private UpgradeRegistry _upgrades;
        [SerializeField] private UpgradeWindow _upgradeWindow;

        public override void InstallBindings()
        {
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            BindServices();
            BindConnection();
            BindEntities();
            BindUI();
            Container.BindInterfacesTo<GameplayEntryPoint>().AsSingle();
        }

        private void BindEntities()
        {
            Container.Bind<CameraFollow>().FromInstance(_camera).AsSingle();
            
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            Container.Bind<EnemyConfig>().FromInstance(_enemyConfig).AsSingle();
            Container.Bind<SpawnConfig>().FromInstance(_spawnConfig).AsSingle();
            Container.Bind<ProjectileConfig>().FromInstance(_projectileConfig).AsSingle();
            Container.Bind<UpgradeRegistry>().FromInstance(_upgrades).AsSingle();
            
            Container.Bind<HealthBarView>().FromInstance(_healthBarPrefab).AsSingle();
            Container.Bind<HealthBarHUDView>().FromInstance(_hudHealthBarPrefab).AsSingle();
            Container.Bind<ExpBarHUDView>().FromInstance(_hudExpBarPrefab).AsSingle();
            Container.Bind<UpgradeWindow>().FromInstance(_upgradeWindow).AsSingle();
            
            Container.Bind<GameplayUIFactory>().AsSingle();
            Container.Bind<ProjectileFactory>().AsSingle();
            Container.Bind<DropFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle().NonLazy();
            
            Container.Bind<SpawnPositionProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesSpawner>().AsSingle();
        }

        private void BindConnection()
        {
            Container.Bind<InputBehaviour>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<JoystickInput>().AsSingle();
            Container.Bind<PlayersRepository>().AsSingle();
            Container.Bind<EnemiesRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetService>().AsSingle();
            Container.Bind<UpgradeService>().AsSingle();
            Container.Bind<NetworkConfig>().FromInstance(_networkConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<BanService>().AsSingle();
        }

        private void BindUI()
        {
            GameplayUIView uiView = Container.InstantiatePrefabForComponent<GameplayUIView>(_uiView, _canvas.transform);
            Container.Bind<GameplayUIView>().FromInstance(uiView).AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayUIPresenter>().AsSingle();
        }
    }
}