using System;

namespace _Project.Logic.Entities
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        event Action OnDeath;
    }
}