using Fusion;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Gameplay/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [field: SerializeField] public NetworkPrefabRef PlayerPrefab { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float AttackInterval { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float EXPtoLevel { get; private set; }
        [field: SerializeField] public float ExpMultiplier { get; private set; }
    }
}