using System;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class DungeonUI : MonoBehaviour
    {
        [SerializeField] private EasyGridBuilderProController m_EasyGridBuilderProController;
        public Toggle buildingMode;

        private void OnEnable()
        {
            buildingMode.onValueChanged.AddListener(BuildingModeValueChanged);
        }

        private void BuildingModeValueChanged(bool arg0)
        {
            m_EasyGridBuilderProController.SetAllDisable(arg0);
        }
    }
}
