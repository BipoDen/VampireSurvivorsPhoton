using System.Collections.Generic;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay.Upgrades
{
    [CreateAssetMenu(menuName = "Configs/UpgradeRegistry")]
    public class UpgradeRegistry : ScriptableObject
    {
        [SerializeField] private List<UpgradeConfig> _upgrades;

        public IReadOnlyList<UpgradeConfig> Upgrades => _upgrades;

        public UpgradeConfig Get(int index) => _upgrades[index];
    }
}