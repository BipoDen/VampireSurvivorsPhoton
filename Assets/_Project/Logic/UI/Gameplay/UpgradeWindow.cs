using System;
using System.Collections.Generic;
using _Project.Logic.Config.Gameplay.Upgrades;
using UnityEngine;

namespace _Project.Logic.UI.Gameplay
{
    public class UpgradeWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform _upgradeParent;
        [SerializeField] private UpgradeCard _cardPrefab;
        
        public event Action<int> OnUpgradeSelected;

        public void Show(IReadOnlyList<UpgradeConfig> options, IReadOnlyList<int> indices)
        {
            ClearList();
            gameObject.SetActive(true);

            for (int i = 0; i < options.Count; i++)
            {
                var card = Instantiate(_cardPrefab, _upgradeParent);
                card.SetCard(options[i], indices[i], HandleSelected);
            }
        }

        public void Hide()
        {
            ClearList();
            gameObject.SetActive(false);
        }

        private void ClearList()
        {
            foreach (Transform upgrade in _upgradeParent)
                Destroy(upgrade.gameObject);
        }

        private void HandleSelected(int registryIndex)
        {
            OnUpgradeSelected?.Invoke(registryIndex);
        }
    }
}