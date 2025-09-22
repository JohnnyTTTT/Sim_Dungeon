using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class GridManagersTest : MonoBehaviour
    {
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
