using _Project.Logic.Services;
using Fusion;

namespace _Project.Logic.Multiplayer.Gameplay
{
    public class DeathRegistry : NetworkBehaviour
    {
        [Networked, Capacity(64)]
        public NetworkDictionary<int, NetworkBool> DeadPlayers => default;

        public void MarkDead(int tokenHash)
        {
            if (Object.HasStateAuthority)
                DeadPlayers.Set(tokenHash, true);
        }

        public bool IsDead(int tokenHash)
            => DeadPlayers.TryGet(tokenHash, out var dead) && dead;
    }
}