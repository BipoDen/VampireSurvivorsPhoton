using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay.Upgrades
{
    public abstract class UpgradeConfig : ScriptableObject
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        public abstract void Apply(EntityStats target);
    }
}