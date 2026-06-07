using System;
using System.Collections.Generic;
using _Project.Logic.Entities.Player;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Logic.Services
{
    public interface INetworkSessionService
    {
        public NetworkRunner Runner { get; }
        public event Action<NetworkRunner> OnRunnerMigration;
        public event Action<int, NetworkPlayer> PlayerRestored;
        public event Action OnHostMigrationCleanUp;
        byte[] GetConnectionToken();
        void CreateGame();
        void JoinGame(string sessionID);
    }
}