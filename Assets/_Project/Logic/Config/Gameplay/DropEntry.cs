using UnityEngine;

namespace _Project.Logic.Config.Gameplay
{
    [System.Serializable]
    public struct DropEntry
    {
        public ItemConfig Item;
        [Range(0f, 1f)] public float Chance;
        public int MinCount;
        public int MaxCount;
    }
}