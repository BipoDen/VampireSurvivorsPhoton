using _Project.Logic.Config;
using _Project.Logic.Multiplayer;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private NetworkRunner _runnerPrefab;
        [SerializeField] private NetworkConfig _networkConfig;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
            
            Container.Bind<NetworkConfig>().FromInstance(_networkConfig).AsSingle();
            Container.Bind<NetworkRunner>().FromInstance(_runnerPrefab).AsSingle();
            Container.Bind<NetworkRunnerCallbacksAdapter>().AsSingle();
            Container.BindInterfacesAndSelfTo<NetworkSessionService>().AsSingle();

        }
    }
}