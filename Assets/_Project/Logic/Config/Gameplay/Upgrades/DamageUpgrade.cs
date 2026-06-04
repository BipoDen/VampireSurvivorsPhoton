using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay.Upgrades
{
    [CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Configs/Gameplay/Upgrades/DamageUpgrade")]
    public class DamageUpgrade : UpgradeConfig
    {
        [SerializeField] private float _amount;
        
        public override void Apply(EntityStats target) => target.AddDamage(_amount);
    }
}