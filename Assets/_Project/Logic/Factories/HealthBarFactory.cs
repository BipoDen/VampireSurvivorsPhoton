using _Project.Logic.Entities.Player;
using _Project.Logic.UI.Gameplay;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.Logic.Factories
{
    public class HealthBarFactory
    {
        private HealthBarView _healthBarPrefab;
        private HealthBarHUDView _healthBarHUDPrefab;
        private Canvas _canvas;

        public HealthBarFactory(HealthBarView healthBarPrefab, HealthBarHUDView healthBarHUDPrefab, Canvas canvas)
        {
            _healthBarPrefab = healthBarPrefab;
            _healthBarHUDPrefab = healthBarHUDPrefab;
            _canvas = canvas;
        }

        public void Create(NetworkPlayer player, Transform anchor)
        {
            HealthBarView view = Object.Instantiate(_healthBarPrefab, _canvas.transform);
            HealthBarPresenter presenter = new(view, player, anchor);

            player.OnDied += Despawn;

            void Despawn()
            {
                player.OnDied -= Despawn;
                Object.Destroy(view.gameObject);
            }
        }

        public void CreateHUD(NetworkPlayer player)
        {
            HealthBarHUDView view = Object.Instantiate(_healthBarHUDPrefab, _canvas.transform);
            HealthBarHUDPresenter presenter = new(view, player);
        }
    }
}