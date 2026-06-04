using _Project.Logic.Entities;
using Fusion;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/Gameplay/ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public NetworkPrefabRef ProjectilePrefab { get; set; }
        [field: SerializeField] public float LifeTime { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}