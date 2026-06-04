using System;
using _Project.Logic.Entities.Player;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Entities.Drops
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class NetworkItem : NetworkBehaviour
    {
        public void Pickup(NetworkPlayer player)
        {
            if (!HasStateAuthority) return;

            OnPickup(player);
            Runner.Despawn(Object);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!HasStateAuthority) return;
            
            if (collision.gameObject.TryGetComponent(out NetworkPlayer player))
                Pickup(player);
        }

        protected abstract void OnPickup(NetworkPlayer player);
    }
}