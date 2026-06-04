using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Entities.Drops
{
    public class NetworkHealPotion : NetworkItem
    {
        [SerializeField] private int _healAmount;
        protected override void OnPickup(NetworkPlayer player) => player.Heal(_healAmount);
    }
}