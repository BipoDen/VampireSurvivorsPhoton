using System.Collections.Generic;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Services;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Logic.Factories
{
    public class DropFactory
    {
        private INetworkSessionService _sessionService;

        public DropFactory(INetworkSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public void SpawnDrop(List<DropEntry> drops, Vector2 position)
        {
            var runner = _sessionService.Runner;
            if(!runner.IsServer)
                return;
            
            foreach (var drop in drops)
            {
                if (Random.value > drop.Chance) continue;

                int count = Random.Range(drop.MinCount, drop.MaxCount + 1);
                for (int i = 0; i < count; i++)
                {
                    var offset = Random.insideUnitCircle * 0.5f;
                    runner.Spawn(drop.Item.ItemPrefab, position + offset);
                }
            }
        }
    }
}