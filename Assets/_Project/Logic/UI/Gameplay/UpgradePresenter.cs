using System;
using System.Collections.Generic;
using _Project.Logic.Config.Gameplay.Upgrades;
using _Project.Logic.Entities.Player;
using _Project.Logic.Services;

namespace _Project.Logic.UI.Gameplay
{
    public class UpgradePresenter : IDisposable
    {
        private UpgradeWindow _view;
        private UpgradeService _upgradeService;
        private NetworkPlayer _player;

        public UpgradePresenter(UpgradeWindow view, UpgradeService upgradeService, NetworkPlayer player)
        {
            _view = view;
            _upgradeService = upgradeService;
            _player = player;
            
            _upgradeService.OnChoiceRequested += HandleChoiceRequested;
            _upgradeService.OnAllChoicesResolved += HandleResolved;
            _view.OnUpgradeSelected += HandleUpgradeSelected;

            _view.Hide();
        }

        private void HandleChoiceRequested(IReadOnlyList<UpgradeConfig> options, IReadOnlyList<int> indices)
        {
            _view.Show(options, indices);
        }

        private void HandleUpgradeSelected(int registryIndex)
        {
            _player.SubmitUpgradeChoice(registryIndex);
        }

        private void HandleResolved() => _view.Hide();
        
        public void Dispose()
        {
            _upgradeService.OnChoiceRequested -= HandleChoiceRequested;
            _upgradeService.OnAllChoicesResolved -= HandleResolved;
            _view.OnUpgradeSelected -= HandleUpgradeSelected;
        }
    }
}