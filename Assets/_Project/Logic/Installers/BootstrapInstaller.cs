using _Project.Logic.EntryPoint;
using Zenject;

namespace _Project.Logic.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapEntryPoint>().AsSingle();
        }
    }
}