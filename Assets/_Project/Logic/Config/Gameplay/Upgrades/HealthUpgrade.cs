using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay.Upgrades
{
    [CreateAssetMenu(fileName = "HealthUpgrade", menuName = "Configs/Gameplay/Upgrades/HealthUpgrade")]
    public class HealthUpgrade : UpgradeConfig
    {
        [SerializeField] private float _amount;
        
        public override void Apply(EntityStats target) => target.AddHealth(_amount);
    }
}