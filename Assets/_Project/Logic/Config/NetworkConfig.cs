using _Project.Logic.Config.Gameplay;
using _Project.Logic.Multiplayer.Gameplay;
using _Project.Logic.Services;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Config
{
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Configs/NetworkConfig")]
    public class NetworkConfig : ScriptableObject
    {
        [field: SerializeField] public NetworkPrefabRef DeathRegistry { get; private set; }
    }
}
