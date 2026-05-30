using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.UI.Gameplay
{
    public class HealthBarPresenter
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
        }

        private void OnMove(Vector2 position)
        {
            _view.SetScreenPosition(_anchor);
        }
    }
}