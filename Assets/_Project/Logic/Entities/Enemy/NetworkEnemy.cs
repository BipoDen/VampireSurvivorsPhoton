using System;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities.Player;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Entities.Enemy
{
    public class NetworkEnemy : NetworkBehaviour, IAfterSpawned
    {
        [Networked] private NetworkPlayer Target { get; set; }
        [Networked] private TickTimer AttackCooldown { get; set; }
        [SerializeField] private EntityStats _stats;
        
        public bool IsAlive => _stats.CurrentHealth > 0;
        public event Action OnDied;
        
        private ITargetService _targetService;
        private EnemiesRepository _repository;
        private Rigidbody2D _rb;


        [Inject]
        public void Construct(ITargetService targetService, EnemiesRepository repository)
        {
            _targetService = targetService;
            _repository = repository;
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(EnemyConfig config)
        {
            _stats.Initialize(config.Health,
                config.Damage,
                config.Speed,
                config.AttackInterval,
                config.AttackRange);
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) 
                return;

            Target = _targetService.GetClosestPlayer(transform.position);
            if(Target == null)
                return;
            
            TryMove();
            TryAttack();
        }

        private void TryMove()
        {
            if(CanAttack())
                return;
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            _rb.linearVelocity = direction * _stats.Speed;
        }

        private void TryAttack()
        {
            if (CanAttack() && AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                Target.TakeDamage(_stats.Damage);
                AttackCooldown = TickTimer.CreateFromSeconds(Runner, _stats.AttackInterval);
            }
        }

        private bool CanAttack()
        {
            float distSqr = (Target.transform.position - transform.position).sqrMagnitude;
    
            if (distSqr <= _stats.AttackRange * _stats.AttackRange)
            {
                _rb.linearVelocity = Vector2.zero;
                return true;
            }
            return false;
        }

        public void TakeDamage(float damage)
        {
            _stats.CurrentHealth = Mathf.Max(_stats.CurrentHealth - damage, 0);
            if (_stats.CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _repository.UnregisterEnemy(this);
            OnDied?.Invoke();
        }

        public void AfterSpawned()
        {
            _repository.RegisterEnemy(this);
        }
    }
}