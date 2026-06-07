using System;
using _Project.Logic.Config.Gameplay;
using _Project.Logic.Entities.Enemy;
using _Project.Logic.Factories;
using _Project.Logic.Multiplayer;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
using _Project.Logic.UI.Gameplay;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Logic.Entities.Player
{
    [RequireComponent(typeof(PlayerLevel), typeof(EntityStats))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private Transform _healthBarAnchor;
        [SerializeField] private EntityStats _stats;
        [SerializeField] private PlayerLevel _level;
        
        private PlayerConfig _config;
        private PlayersRepository _playersRepository;
        private ITargetService _targetService;
        private GameplayUIFactory _gameplayUIFactory;
        private ProjectileFactory _projectileFactory;
        private UpgradeService _upgradeService;
        
        [Networked] public int OwnerToken { get; set; }
        [Networked] private NetworkEnemy Target { get; set; }
        [Networked] private TickTimer AttackCooldown { get; set; }

        public bool IsAlive => _stats.CurrentHealth > 0;
        private bool _viewBuilt;
        
        public event Action OnDied;
        public event Action OnDespawn;
        public event Action<Vector2> OnMove;
        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnExpChanged;

        [Inject]
        public void Construct(PlayerConfig config, PlayersRepository playersRepository,
            GameplayUIFactory gameplayUIFactory, 
            ITargetService targetService, 
            PlayerConfig playerConfig,
            ProjectileFactory projectileFactory,
            UpgradeService upgradeService)
        {
            _config = config;
            _playersRepository = playersRepository;
            _gameplayUIFactory = gameplayUIFactory;
            _targetService = targetService;
            _projectileFactory = projectileFactory;
            _upgradeService = upgradeService;
        }

        public void Initialize()
        {
            if (HasStateAuthority && _stats.MaxHealth <= 0)
            {
                _stats.Initialize(_config.Health, _config.Damage, _config.Speed,
                    _config.AttackInterval, _config.AttackRange);
                _level.Initialize(_config.EXPtoLevel, _config.ExpMultiplier);
            }
        }

        public void InitializeView()
        {
            if (HasInputAuthority)
            {
                _gameplayUIFactory.CreateHUDHealthBar(this);
                _gameplayUIFactory.CreateHUDExpBar(this);
                _gameplayUIFactory.CreateUpgradeWindow(this);
                Camera.main.gameObject.GetComponent<CameraFollow>().SetTarget(transform);
            }
            else
            {
                _gameplayUIFactory.CreateHealthBar(this, _healthBarAnchor);
            }
            
            _stats.OnHealthChanged += OnHealthChanged;
            _stats.OnDied += Die;
            _level.OnExpChanged += OnExpChanged;
            _level.OnLevelChanged += LevelUp;
            
            OnExpChanged?.Invoke(_level.CurrentEXP, _level.MaxEXP);
            OnHealthChanged?.Invoke(_stats.CurrentHealth, _stats.MaxHealth);
        }
        
        public void OnResumed()
        {
            AttackCooldown = default;
        }
        
        public override void Render()
        {
            if (_viewBuilt) return;
            if (OwnerToken == 0) return;
            _viewBuilt = true;
            InitializeView();
        }

        public override void FixedUpdateNetwork()
        {
            if(!IsAlive)
                return;
            
            TryMove();
            TryAttack();
        }
        
        private void LateUpdate()
        {
            OnMove?.Invoke(transform.position);
        }

        public void TakeDamage(float damage) => _stats.TakeDamage(damage);

        public void Heal(float amount) => _stats.Heal(amount);

        public void TakeExp(float amount) => _level.TakeExp(amount);

        private void TryMove()
        {
            if (GetInput(out NetInput input))
            {
                Vector2 direction = input.Direction;
                transform.position += (Vector3)(direction * _stats.Speed * Runner.DeltaTime);
            }
        }

        private void TryAttack()
        {
            if (!HasStateAuthority) return;
            if (AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                Target = _targetService.GetClosestEnemy(transform.position, _stats.AttackRange);
                if (Target == null)
                    return;
                _projectileFactory.CreateProjectile(transform, Target.transform, _stats.Damage);
                AttackCooldown = TickTimer.CreateFromSeconds(Runner, _stats.AttackInterval);
            }
        }

        private void LevelUp()
        {
            if (!HasStateAuthority) return;
            RpcRequestChoice();
        }
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
        private void RpcRequestChoice()
        {
            _upgradeService.EnqueueChoice();
        }
        
        public void SubmitUpgradeChoice(int upgradeIndex)
        {
            RpcApplyUpgrade(upgradeIndex); 
            _upgradeService.ConsumeChoice(); 
        }
        
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RpcApplyUpgrade(int upgradeIndex)
        {
            _upgradeService.ApplyUpgrade(_stats, upgradeIndex);
        }
        
        private void Die()
        {
            if (HasStateAuthority)
                OnDied?.Invoke();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _stats.OnHealthChanged -= OnHealthChanged;
            _stats.OnDied -= Die;
            _level.OnExpChanged -= OnExpChanged;
            _level.OnLevelChanged -= LevelUp;
            OnDespawn?.Invoke();
        }
    }
}