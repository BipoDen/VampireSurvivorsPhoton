using _Project.Logic.Entities.Enemy;
using _Project.Logic.Entities.Player;
using UnityEngine;

namespace _Project.Logic.Services
{
    public interface ITargetService
    {
        NetworkPlayer GetClosestPlayer(Vector3 from);
        NetworkEnemy GetClosestEnemy(Vector3 from);
    }
}