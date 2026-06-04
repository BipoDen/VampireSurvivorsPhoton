using System;
using _Project.Logic.Entities.Player;

namespace _Project.Logic.UI.Gameplay
{
    public class ExpBarHUDPresenter : IDisposable
    {
        private ExpBarHUDView _view;
        private NetworkPlayer _player;

        public ExpBarHUDPresenter(ExpBarHUDView view, NetworkPlayer player)
        {
            _view = view;
            _player = player;

            _player.OnExpChanged += OnExpChanged;
        }

        private void OnExpChanged(float current, float maxValue)
        {
            _view.SetExpValue(current, maxValue);
        }

        public void Dispose()
        {
            _player.OnExpChanged -= OnExpChanged;
        }
    }
}