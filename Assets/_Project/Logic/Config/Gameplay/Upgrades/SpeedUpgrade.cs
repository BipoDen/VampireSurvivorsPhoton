using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay.Upgrades
{
    [CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Configs/Gameplay/Upgrades/SpeedUpgrade")]
    public class SpeedUpgrade : UpgradeConfig
    {
        [SerializeField] private float _amount;
        
        public override void Apply(EntityStats target) => target.AddSpeed(_amount);
    }
}