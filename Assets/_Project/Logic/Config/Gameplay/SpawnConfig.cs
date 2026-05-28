using UnityEngine;

namespace _Project.Logic.Config.Gameplay
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/Gameplay/SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        [field: SerializeField] public float MaxAttempts { get; private set; }
        [field: SerializeField] public float MinDistance { get; private set; }
        [field: SerializeField] public float MaxDistance { get;  private set; }
        [field: SerializeField] public float SpawnDelay { get; private set; }
    }
}