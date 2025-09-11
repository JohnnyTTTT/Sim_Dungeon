using System;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class DungeonUI : MonoBehaviour
    {
        public Toggle buildingMode;

        private void OnEnable()
        {
            buildingMode.onValueChanged.AddListener(BuildingModeValueChanged);
        }

        private void BuildingModeValueChanged(bool arg0)
        {

        }
    }
}
