using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Logic.Services
{
    public interface INetworkSessionService
    {
        void CreateGame();
        void JoinGame(string sessionID);
    }
}