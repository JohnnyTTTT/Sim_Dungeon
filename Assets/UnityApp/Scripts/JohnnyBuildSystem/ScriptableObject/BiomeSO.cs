using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum SpawnRule
    {
        Any,   
        OnlyEdge
    }

    [System.Serializable]
    public class BiomeSpawnObject 
    {
        public BuildableGridObjectSO prefab;
        public SpawnRule  spawnRule;
        public float probability;
    }

    [CreateAssetMenu(menuName = "Johnny/Build System/Biome", order = 100)]
    public class BiomeSO : ScriptableObject
    {
        public BiomeSpawnObject[] prefabs;
    }
}
