using System;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Factories;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
using _Project.Logic.UI.Gameplay;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Entities.Player
{
    public class NetworkPlayer : NetworkBehaviour, IAfterSpawned
    {
        [SerializeField] private Transform _healthBarAnchor;
        
        private PlayersRepository _playersRepository;
        private PlayerConfig _playerConfig;
        private CameraFollow _cameraFollow;
        private Camera _camera;
        private HealthBarFactory _healthBarFactory;
        
        public event Action OnDied;
        public event Action<Vector2> OnMove;

        [Inject]
        public void Construct(PlayersRepository playersRepository, CameraFollow cameraFollow,
            HealthBarFactory healthBarFactory, PlayerConfig playerConfig)
        {
            _playersRepository = playersRepository;
            _cameraFollow = cameraFollow;
            _camera = _cameraFollow.GetComponent<Camera>();
            _healthBarFactory = healthBarFactory;
            _playerConfig = playerConfig;
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

        private void LateUpdate()
        {
            OnMove?.Invoke(transform.position);
        }

        private void TryAttack()
        {
            
        }
        

        private void OnHpChanged()
        {
            
        }
        
        private void Die()
        {
            _playersRepository.UnregisterPlayer(Object.InputAuthority);
            OnDied?.Invoke();
        }
    
        public void AfterSpawned()
        {
            _playersRepository.RegisterPlayer(Object.InputAuthority, this);
            if (HasInputAuthority)
                _cameraFollow.SetTarget(transform);

            if (HasInputAuthority)
                _healthBarFactory.CreateHUD(this);
            else
                _healthBarFactory.Create(this, _healthBarAnchor);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            Die();
        }
    }
}