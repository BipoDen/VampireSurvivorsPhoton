using System;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Entities.Player
{
    public class PlayerLevel : NetworkBehaviour
    {
        [Networked] public int Level { get; private set; }
        [Networked, OnChangedRender(nameof(ExpChanged))] public float CurrentEXP { get; private set; }
        [Networked] public float MaxEXP { get; private set; }
        [Networked] public float ExpModifier { get; private set; }
        
        public event Action<float, float> OnExpChanged;
        public event Action OnLevelChanged;
        
        public void Initialize(float expToLevel, float expMultiplier)
        {
            MaxEXP = expToLevel;
            ExpModifier = expMultiplier;
        }
        
        public void TakeExp(float value)
        {
            if (!HasStateAuthority) return;
            
            CurrentEXP += value;
            while (CurrentEXP >= MaxEXP)
            {
                CurrentEXP -= MaxEXP;
                MaxEXP *= ExpModifier;
                Level++;
                OnLevelChanged?.Invoke();
            }
        }

        private void ExpChanged()
        {
            OnExpChanged?.Invoke(CurrentEXP, MaxEXP);
        }
    }
}