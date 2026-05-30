using Fusion;

namespace _Project.Logic.Entities.Player
{
    public struct PlayerStats : INetworkStruct
    {
        public float Health;
        public float MaxHealth;
        public float Speed;
        public float AttackInterval;
        public float AttackRange;
    }
}