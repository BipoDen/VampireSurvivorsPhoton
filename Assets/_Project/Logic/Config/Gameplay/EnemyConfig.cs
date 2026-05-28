using Fusion;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Gameplay/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public NetworkPrefabRef EnemyPrefab { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float AttackInterval { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
    }
}