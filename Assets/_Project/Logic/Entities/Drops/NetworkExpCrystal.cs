using _Project.Logic.Entities.Player;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Entities.Drops
{
    public class NetworkExpCrystal : NetworkItem
    {
        [SerializeField] private int _expAmount;
        protected override void OnPickup(NetworkPlayer player) => player.TakeExp(_expAmount);
    }
}