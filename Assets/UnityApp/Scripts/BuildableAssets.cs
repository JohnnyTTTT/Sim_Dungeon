using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class BuildableAssets : MonoBehaviour
    {
        public static BuildableAssets Instance
        {
            get
            {
                if (s_Instances == null)
                {
                    s_Instances = FindFirstObjectByType<BuildableAssets>();
                }
                return s_Instances;
            }
        }
        private static BuildableAssets s_Instances;

        public ReplaceableObjectSO stoneWall;
    }
}
