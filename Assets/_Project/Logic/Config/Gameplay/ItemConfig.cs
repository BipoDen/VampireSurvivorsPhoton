using Fusion;
using UnityEngine;

namespace _Project.Logic.Config.Gameplay
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/Gameplay/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        [field: SerializeField] public NetworkPrefabRef ItemPrefab { get; private set; }
    }
}