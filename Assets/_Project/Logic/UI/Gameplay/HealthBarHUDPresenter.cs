using _Project.Logic.Entities.Player;

namespace _Project.Logic.UI.Gameplay
{
    public class HealthBarHUDPresenter
    {
        private HealthBarHUDView _view;
        private NetworkPlayer _player;

        public HealthBarHUDPresenter(HealthBarHUDView view, NetworkPlayer player)
        {
            _view = view;
            _player = player;
            
            
        }
        
        
    }
}