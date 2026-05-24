using _Project.Logic.Multiplayer.Gameplay;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Config
{
    [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Configs/NetworkConfig")]
    public class NetworkConfig : ScriptableObject
    {
        [field: SerializeField] public NetworkPrefabRef PlayerPrefab { get; private set; }
        [field: SerializeField] public PlayerSpawner PlayerSpawnerPrefab { get; private set; }
        [field: SerializeField] public SimulationBehaviour InputManager { get; private set; }
    }
}
