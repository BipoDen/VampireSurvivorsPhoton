using System;
using System.Collections.Generic;
using _Project.Logic.Config.Gameplay.Upgrades;
using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Services
{
    public class UpgradeService
    {
        private int _pendingChoices;
        private UpgradeRegistry _registry;

        public UpgradeService(UpgradeRegistry registry)
        {
            _registry = registry;
        }

        private const int OptionsCount = 3;
        
        public event Action<IReadOnlyList<UpgradeConfig>, IReadOnlyList<int>> OnChoiceRequested;
        public event Action OnAllChoicesResolved;

        public bool HasPending => _pendingChoices > 0;
        
        public void EnqueueChoice()
        {
            _pendingChoices++;
            if (_pendingChoices == 1)
                PresentNext();
        }

        public void ApplyUpgrade(EntityStats stats, int upgradeIndex)
        {
            _registry.Get(upgradeIndex).Apply(stats);
        }

        public void ConsumeChoice()
        {
            _pendingChoices = Mathf.Max(0, _pendingChoices - 1);
            if (_pendingChoices > 0)
                PresentNext();
            else
                OnAllChoicesResolved?.Invoke();
        }

        private void PresentNext()
        {
            var indices = RollIndices();
            var configs = new List<UpgradeConfig>(indices.Count);
            foreach (var i in indices)
                configs.Add(_registry.Get(i));
            OnChoiceRequested?.Invoke(configs, indices);
        }

        private List<int> RollIndices()
        {
            var pool = new List<int>(_registry.Upgrades.Count);
            for (int i = 0; i < _registry.Upgrades.Count; i++)
                pool.Add(i);

            int take = Mathf.Min(OptionsCount, pool.Count);
            var result = new List<int>(take);
            for (int i = 0; i < take; i++)
            {
                int pick = UnityEngine.Random.Range(0, pool.Count);
                result.Add(pool[pick]);
                pool.RemoveAt(pick);
            }
            return result;
        }
    }
}