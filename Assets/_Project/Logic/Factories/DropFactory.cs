using System.Collections.Generic;
using _Project.Logic.Config.Gameplay;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Logic.Factories
{
    public class DropFactory
    {
        private NetworkRunner _runner;

        public DropFactory(NetworkRunner runner)
        {
            _runner = runner;
        }

        public void SpawnDrop(List<DropEntry> drops, Vector2 position)
        {
            foreach (var drop in drops)
            {
                if (Random.value > drop.Chance) continue;

                int count = Random.Range(drop.MinCount, drop.MaxCount + 1);
                for (int i = 0; i < count; i++)
                {
                    var offset = Random.insideUnitCircle * 0.5f;
                    _runner.Spawn(drop.Item.ItemPrefab, position + offset);
                }
            }
        }
    }
}