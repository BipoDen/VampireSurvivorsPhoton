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
        [SerializeField] private NetworkRunner _runner;
        [SerializeField] private NetworkConfig _networkConfig;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().AsSingle();
            
            var runner = Container.InstantiatePrefabForComponent<NetworkRunner>(_runner);
            DontDestroyOnLoad(runner.gameObject);
            Container.Bind<NetworkRunner>().FromInstance(runner).AsSingle();
            Container.Bind<NetworkRunnerCallbacksAdapter>().FromNewComponentOn(runner.gameObject).AsSingle().NonLazy();
            Container.Bind<NetworkConfig>().FromInstance(_networkConfig).AsSingle();
        }
    }
}