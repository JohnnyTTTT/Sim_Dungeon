using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class GridManagersTest : MonoBehaviour
    {
        [Button]
        private void Set1()
        {
            GridManager.Instance.GetActiveEasyGridBuilderPro().SetCellSize(1);
        }

        [Button]
        private void Set2()
        {
            GridManager.Instance.GetActiveEasyGridBuilderPro().SetCellSize(2);
        }

        protected  void Start()
        {
            GridManager.Instance.OnActiveGridModeChanged += OnActiveGridModeChanged;
        }

        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {
            Debug.Log(gridMode);
        }
    }
}
