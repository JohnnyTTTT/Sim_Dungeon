using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    [CreateAssetMenu(menuName = "Johnny/Build System/Buildable Probability ObjectSO", order = 100)]
    public class BuildableProbabilityObjectSO : ScriptableObject
    {
        public BuildableGridObjectSO prefab;
        public float probability;
    }
}
