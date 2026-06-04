using System;
using _Project.Logic.Config.Gameplay;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Entities.Player
{
    public class EntityStats : NetworkBehaviour
    {
        [Networked, OnChangedRender(nameof(HealthChanged))] public float CurrentHealth { get; set; }
        [Networked] public float MaxHealth { get; private set; }
        [Networked] public float Damage { get; private set; }
        [Networked] public float Speed { get; private set; }
        [Networked] public float AttackInterval { get; private set; }
        [Networked] public float AttackRange { get; private set; }
        
        public event Action<float, float> OnHealthChanged;
        public event Action OnDied;

        public void Initialize(float health, 
            float damage, 
            float speed, 
            float attackInterval, 
            float attackRange)
        {
            MaxHealth = health;
            CurrentHealth = MaxHealth;
            Damage = damage;
            Speed = speed;
            AttackInterval = attackInterval;
            AttackRange = attackRange;
        }
        
        public void TakeDamage(float damage)
        {
            if (!HasStateAuthority) return;
            if (damage <= 0) return;
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
            if (CurrentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }
        
        public void Heal(float amount)
        {
            if (!HasStateAuthority) return;
            if (amount <= 0) return;
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        }

        private void HealthChanged()
        {
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void AddDamage(float value) => Damage += value;

        public void AddSpeed(float value) => Speed += value;

        public void AttackSpeedMultiplier(float value) => AttackInterval *= value;

        public void AddHealth(float value)
        {
            MaxHealth += value;
            CurrentHealth += value;
        }
    }
}