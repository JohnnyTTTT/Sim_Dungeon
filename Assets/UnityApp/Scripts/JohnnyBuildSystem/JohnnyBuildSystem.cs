using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class JohnnyBuildSystem : MonoBehaviour
    {
        public static JohnnyBuildSystem Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<JohnnyBuildSystem>();
                }
                return s_Instance;
            }

        }
        private static JohnnyBuildSystem s_Instance;

        [SerializeField]private BuildableObjectSO currentBuildable;

        public void SetInputActiveBuildableObjectSO(BuildableObjectSO buildableObjectSO)
        {
            currentBuildable = buildableObjectSO;
        }
    }
}
