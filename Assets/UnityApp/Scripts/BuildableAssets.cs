using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    [System.Serializable]
    public class BuildableCategory
    {
        public string name;
        public GridType gridType;
        public List<BuildableObjectSO> buildableObjectSOs;
    }


    public class BuildableAssets : MonoBehaviour
    {
        public static BuildableAssets Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<BuildableAssets>();
                }
                return s_Instance;
            }

        }
        private static BuildableAssets s_Instance;

        public List< BuildableCategory> Structures;
        public List<BuildableCategory >Placements;
    }
}
