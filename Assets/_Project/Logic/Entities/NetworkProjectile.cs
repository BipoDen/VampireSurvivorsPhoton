using System;
using _Project.Logic.Entities.Enemy;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Entities
{
    public class NetworkProjectile : NetworkBehaviour
    {
        [Networked] private Vector2 _direction { get; set; }
        [Networked] private float _speed { get; set; }
        [Networked] private float _damage { get; set; }
        [Networked] private TickTimer _life { get; set; }
        public void Initialize(Vector2 direction, float speed, float lifetime, float damage)
        {
            _direction = direction;
            _speed = speed;
            _damage = damage;
            _life = TickTimer.CreateFromSeconds(Runner, lifetime);
        }

        public override void FixedUpdateNetwork()
        {
            transform.position += (Vector3)(_direction * _speed * Runner.DeltaTime);

            if (!HasStateAuthority) return;

            if (_life.Expired(Runner))
                Runner.Despawn(Object);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!HasStateAuthority) return; 
            
            if (other.gameObject.TryGetComponent<NetworkEnemy>(out var enemy))
            {
                if (!enemy.Object || !enemy.Object.IsValid)
                    return;

                enemy.TakeDamage(_damage);
                Runner.Despawn(Object);
            }
        }
    }
}