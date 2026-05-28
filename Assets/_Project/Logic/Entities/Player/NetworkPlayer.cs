using System;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Entities.Player
{
    public class NetworkPlayer : NetworkBehaviour, IAfterSpawned, IDamageable, IHealable
    {
        private float _maxHealth;
        private float _health;
        
        public event Action<float, float> HealthChanged;
        public event Action OnDeath;
        
        private PlayersRepository _playersRepository;
        private PlayerConfig _playerConfig;
        private CameraFollow _cameraFollow;

        [Inject]
        public void Construct(PlayersRepository playersRepository, CameraFollow cameraFollow)
        {
            _playersRepository = playersRepository;
            _cameraFollow = cameraFollow;
        }

        public void Initialize(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;

            if (HasStateAuthority)
            {
                _maxHealth = _playerConfig.Health;
                _health = _playerConfig.Health;
            }
        }

        public override void FixedUpdateNetwork()
        {
            TryMove();
            TryAttack();
        }

        private void TryMove()
        {
            if (GetInput(out NetInput input))
            {
                Vector2 direction = input.Direction;
                transform.position += (Vector3)(direction * _playerConfig.Speed * Runner.DeltaTime);
            }
        }

        private void TryAttack()
        {
            
        }

        public void TakeDamage(float damage)
        {
            _health = Mathf.Max(_health - damage, 0);
            HealthChanged?.Invoke(_health, _maxHealth);
            if(_health == 0)
            {
                Die();
            }
        }

        public void Heal(float heal)
        {
            _health = Mathf.Min(_health + heal, _maxHealth);
            HealthChanged?.Invoke(_health, _maxHealth);
        }

        private void Die()
        {
            _playersRepository.UnregisterPlayer(Object.InputAuthority);
            OnDeath?.Invoke();
        }
                
        public void AfterSpawned()
        {
            _playersRepository.RegisterPlayer(Object.InputAuthority, this);
            if (HasInputAuthority)
                _cameraFollow.SetTarget(transform);

            HealthChanged?.Invoke(_health, _maxHealth);
        }
    }
}