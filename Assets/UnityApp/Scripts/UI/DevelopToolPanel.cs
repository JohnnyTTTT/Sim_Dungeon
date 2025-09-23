using System;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class DevelopToolPanel : MonoBehaviour
    {
        [SerializeField] private Toggle areaDetection;
        private void Start()
        {
            areaDetection.onValueChanged.AddListener(OnAreaDetection);
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
