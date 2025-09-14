using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class BuildableUI : MonoBehaviour
    {
        [SerializeField] private RectTransform m_ItemPrefab;
        [SerializeField] private Transform m_Content;
        [SerializeField] private ToggleGroup m_ToggleGroup;

        public event OnBuildableButtonPressedDelegate OnBuildableButtonPressed;
        public delegate void OnBuildableButtonPressedDelegate( BuildableObjectSO buildableObjectSO);

        private List<BuildableObjectSO> buildableObjectSOList = new List<BuildableObjectSO>();

        public void Init(EasyGridBuilderPro activeEasyGridBuilderPro)
        {
            foreach (BuildableObjectSO buildableObjectSO in activeEasyGridBuilderPro.GetBuildableGridObjectSOList())
            {
                buildableObjectSOList.Add(buildableObjectSO);
            }
            InstantiateUIBuildableObjects();
        }


        private void InstantiateUIBuildableObjects()
        {
            foreach (BuildableObjectSO buildableObjectSO in buildableObjectSOList)
            {
                var buildableUIObject = Instantiate(m_ItemPrefab, m_Content);

                if (buildableObjectSO.objectIcon && buildableUIObject.transform.GetChild(0).TryGetComponent(out Image imageComponent))
                {
                    imageComponent.sprite = buildableObjectSO.objectIcon;
                }

                if (buildableUIObject.transform.TryGetComponent(out Button button))
                {
                    button.onClick.AddListener(delegate { OnBuildableButtonPressed( buildableObjectSO); });
                }
            }
        }
    }
}
