using System;
using _Project.Logic.Entities.Player;

namespace _Project.Logic.UI.Gameplay
{
    public class HealthBarHUDPresenter : IDisposable
    {
        private HealthBarHUDView _view;
        private NetworkPlayer _player;

        public HealthBarHUDPresenter(HealthBarHUDView view, NetworkPlayer player)
        {
            _view = view;
            _player = player;

            _player.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float current, float maxHealth)
        {
            _view.SetHealthValue(current, maxHealth);
        }

        public void Dispose()
        {
            _player.OnHealthChanged -= OnHealthChanged;
        }
    }
}