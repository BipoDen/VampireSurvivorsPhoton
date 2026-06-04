using _Project.Logic.Entities.Player;
using _Project.Logic.Services;
using _Project.Logic.UI.Gameplay;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Logic.Factories
{
    public class GameplayUIFactory
    {
        private UpgradeService _upgradeService;
        
        private HealthBarView _healthBarPrefab;
        private HealthBarHUDView _healthBarHUDPrefab;
        private ExpBarHUDView _expBarHUDPrefab;
        private UpgradeWindow _upgradeWindowPrefab;
        private Canvas _canvas;

        public GameplayUIFactory(UpgradeService upgradeService, Canvas canvas, HealthBarView healthBarPrefab, HealthBarHUDView healthBarHUDPrefab,
            ExpBarHUDView expBarHUDPrefab, UpgradeWindow upgradeWindowPrefab)
        {
            _upgradeService = upgradeService;
            _canvas = canvas;
            
            _healthBarPrefab = healthBarPrefab;
            _healthBarHUDPrefab = healthBarHUDPrefab;
            _expBarHUDPrefab = expBarHUDPrefab;
            _upgradeWindowPrefab = upgradeWindowPrefab;
        }

        public void CreateHealthBar(NetworkPlayer player, Transform anchor)
        {
            HealthBarView view = Object.Instantiate(_healthBarPrefab, _canvas.transform);
            HealthBarPresenter presenter = new(view, player, anchor);

            player.OnDespawn += Despawn;

            void Despawn()
            {
                player.OnDespawn -= Despawn;
                presenter.Despawn();
            }
        }

        public void CreateHUDHealthBar(NetworkPlayer player)
        {
            HealthBarHUDView view = Object.Instantiate(_healthBarHUDPrefab, _canvas.transform);
            HealthBarHUDPresenter presenter = new(view, player);
        }

        public void CreateHUDExpBar(NetworkPlayer player)
        {
            ExpBarHUDView view = Object.Instantiate(_expBarHUDPrefab, _canvas.transform);
            ExpBarHUDPresenter presenter = new(view, player);
        }

        public void CreateUpgradeWindow(NetworkPlayer player)
        {
            UpgradeWindow view = Object.Instantiate(_upgradeWindowPrefab, _canvas.transform);
            UpgradePresenter presenter = new(view, _upgradeService, player);
        }
    }
}