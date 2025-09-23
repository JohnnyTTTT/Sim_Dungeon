using System;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class DevelopToolPanel : MonoBehaviour
    {
        [SerializeField] private Toggle areaDetection;
        [SerializeField] private Toggle cellDetection;
        private void Start()
        {
            cellDetection.onValueChanged.AddListener(OnCellDetection);
            areaDetection.onValueChanged.AddListener(OnAreaDetection);
        }

        private void OnCellDetection(bool value)
        {
            if (value)
            {
                DevelopManager.Instance.currentMode = DevelopMode.Cell;
            }
            else if (DevelopManager.Instance.currentMode == DevelopMode.Cell)
            {
                DevelopManager.Instance.currentMode = DevelopMode.None;
            }
        }

        private void OnAreaDetection(bool value)
        {
            if (value)
            {
                DevelopManager.Instance.currentMode = DevelopMode.Area;
            }
            else if(DevelopManager.Instance.currentMode == DevelopMode.Area)
            {
                DevelopManager.Instance.currentMode = DevelopMode.None;
            }

        }
    }
}
