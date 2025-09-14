using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class CategoryUI : MonoBehaviour
    {
        [SerializeField] private RectTransform m_ItemPrefab;
        [SerializeField] private Transform m_Content;
        [SerializeField] private ToggleGroup m_ToggleGroup;

        private List<BuildableObjectUICategorySO> buildableObjectUICategorySOList;
        public event OnCategoryButtonPressedDelegate OnCategoryButtonPressed;

        public delegate void OnCategoryButtonPressedDelegate(bool isOn, BuildableObjectUICategorySO buildableObjectUICategorySO);
        private Dictionary<BuildableObjectUICategorySO, RectTransform> instantiatedUICategoryObjectsDictionary = new Dictionary<BuildableObjectUICategorySO, RectTransform>();


        public void Init(EasyGridBuilderPro activeEasyGridBuilderPro)
        {
            Clear();

            var uniqueCategorieshashSet = new HashSet<BuildableObjectUICategorySO>();

            foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableGridObjectSOList())
            {
                if (buildableObjectSO.buildableObjectUICategorySO != null)
                {
                    uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
                }
            }

            buildableObjectUICategorySOList = new List<BuildableObjectUICategorySO>(uniqueCategorieshashSet);
            InstantiateUICategoryObjects();
        }

        private void InstantiateUICategoryObjects()
        {
            foreach (var buildableObjectUICategorySO in buildableObjectUICategorySOList)
            {
                var categoryUIObject = Instantiate(m_ItemPrefab, m_Content);

                if (buildableObjectUICategorySO.categoryIcon != null && categoryUIObject.transform.GetChild(0).TryGetComponent(out Image imageComponent))
                {
                    imageComponent.sprite = buildableObjectUICategorySO.categoryIcon;
                }
                instantiatedUICategoryObjectsDictionary.Add(buildableObjectUICategorySO, categoryUIObject);

                if (categoryUIObject.transform.GetChild(0).TryGetComponent(out Toggle toggle))
                {
                    toggle.group = m_ToggleGroup;
                    toggle.onValueChanged.AddListener(delegate { OnCategoryButtonPressed(toggle.isOn, buildableObjectUICategorySO); });
                }
            }
        }


        public void Clear()
        {

        }
    }
}
