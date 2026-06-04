using System;
using _Project.Logic.Entities.Enemy;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Entities
{
    public class NetworkProjectile : NetworkBehaviour
    {
        private Vector2 _direction;
        private float _speed;
        private float _damage;
        private float _lifeTime;
        public void Initialize(Vector2 direction, float speed, float lifetime, float damage)
        {
            _direction = direction;
            _speed = speed;
            _lifeTime = lifetime;
            _damage = damage;
        }

        public override void FixedUpdateNetwork()
        {
            transform.position += (Vector3)(_direction * _speed * Runner.DeltaTime);
            
            if (!HasStateAuthority) return;
            
            _lifeTime -= Runner.DeltaTime;
            if (_lifeTime <= 0f)
                Runner.Despawn(Object);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
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