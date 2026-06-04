using System;
using _Project.Logic.Config.Gameplay.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Logic.UI.Gameplay
{
    public class UpgradeCard : MonoBehaviour
    {
        [SerializeField] private Image _upgradeIcon;
        [SerializeField] private TextMeshProUGUI _upgradeName;
        [SerializeField] private TextMeshProUGUI _upgradeDescription;
        [SerializeField] private Button _upgradeButton;
        
        private int _registryIndex;
        private Action<int> _onClicked;
        
        public void SetCard(UpgradeConfig upgrade, int registryIndex, Action<int> onClicked)
        {
            _upgradeIcon.sprite = upgrade.Icon;
            _upgradeName.text = upgrade.Title;
            _upgradeDescription.text = upgrade.Description;

            _registryIndex = registryIndex;
            _onClicked = onClicked;
            
            _upgradeButton.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            _upgradeButton.onClick.RemoveAllListeners();
        }

        private void HandleClick() => _onClicked?.Invoke(_registryIndex);
    }
}