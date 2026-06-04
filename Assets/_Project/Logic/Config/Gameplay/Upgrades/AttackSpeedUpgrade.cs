using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay.Upgrades
{
    [CreateAssetMenu(fileName = "AttackSpeedUpgrade", menuName = "Configs/Gameplay/Upgrades/AttackSpeedUpgrade")]
    public class AttackSpeedUpgrade : UpgradeConfig
    {
        [SerializeField] private float _multiplier;
        
        public override void Apply(EntityStats target) => target.AttackSpeedMultiplier(_multiplier);
    }
}