using System;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities.Player;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Entities.Enemy
{
    public class NetworkEnemy : NetworkBehaviour
    {
        private ITargetService _targetService;
        private EnemyConfig _config;
        private Rigidbody2D _rb;
        
        [Networked] private NetworkPlayer Target { get; set; }

        [Networked] private TickTimer AttackCooldown { get; set; }

        public event Action OnDeath;
        private bool _canAttack;

        [Inject]
        public void Construct(ITargetService targetService, EnemyConfig config)
        {
            _targetService = targetService;
            _config = config;
            _rb = GetComponent<Rigidbody2D>();
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
            _rb.linearVelocity = direction * _config.Speed;
        }

        private void TryAttack()
        {
            if (CanAttack() && AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                //Target.TakeDamage(_config.Damage);
                AttackCooldown = TickTimer.CreateFromSeconds(Runner, _config.AttackInterval);
            }
        }

        private bool CanAttack()
        {
            float distSqr = (Target.transform.position - transform.position).sqrMagnitude;
    
            if (distSqr <= _config.AttackRange * _config.AttackRange)
            {
                _rb.linearVelocity = Vector2.zero;
                return true;
            }
            return false;
        }

        public void TakeDamage(float damage)
        {
            //_health.TakeDamage(damage);
        }
    }
}