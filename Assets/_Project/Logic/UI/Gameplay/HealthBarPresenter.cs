using System;
using _Project.Logic.Entities.Player;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Logic.UI.Gameplay
{
    public class HealthBarPresenter : IDisposable
    {
        private HealthBarView _view;
        private NetworkPlayer _player;
        private Transform _anchor;

        public HealthBarPresenter(HealthBarView view, NetworkPlayer player, Transform anchor)
        {
            _view = view;
            _player = player;
            _anchor = anchor;
            
            _player.OnMove += OnMove;
            _player.OnHealthChanged += OnHealthChanged;
        }

        private void OnMove(Vector2 position)
        {
            _view.SetScreenPosition(_anchor);
        }

        private void OnHealthChanged(float current, float maxHealth)
        {
            _view.SetHealthValue(current, maxHealth);
        }

        public void Despawn()
        {
            Object.Destroy(_view.gameObject);
            Dispose();
        }

        public void Dispose()
        {
            _player.OnMove -= OnMove;
            _player.OnHealthChanged -= OnHealthChanged;
        }
    }
}