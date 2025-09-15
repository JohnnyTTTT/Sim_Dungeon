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

        private List<BuildableObjectUICategorySO> buildableObjectUICategorySOList;
        public event OnCategoryButtonPressedDelegate OnCategoryButtonPressed;

        public delegate void OnCategoryButtonPressedDelegate(BuildableObjectUICategorySO buildableObjectUICategorySO);
        public Dictionary<BuildableObjectUICategorySO, RectTransform> instantiatedUICategoryObjectsDictionary = new Dictionary<BuildableObjectUICategorySO, RectTransform>();


        public BuildableObjectUICategorySO Init(EasyGridBuilderPro activeEasyGridBuilderPro)
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

            foreach (BuildableObjectSO buildableObjectSO in activeEasyGridBuilderPro.GetBuildableEdgeObjectSOList())
            {
                if (buildableObjectSO.buildableObjectUICategorySO != null) uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
            }


            buildableObjectUICategorySOList = new List<BuildableObjectUICategorySO>(uniqueCategorieshashSet);
            InstantiateUICategoryObjects();

            if (buildableObjectUICategorySOList.Count > 0)
            {
                return buildableObjectUICategorySOList[0];
            }
            return null;
        }

        private void InstantiateUICategoryObjects()
        {
            foreach (var buildableObjectUICategorySO in buildableObjectUICategorySOList)
            {
                var categoryUIObject = Instantiate(m_ItemPrefab, m_Content);

                if (buildableObjectUICategorySO.categoryIcon && categoryUIObject.TryGetComponent<IconButton>(out var iconButton))
                {
                    iconButton.SetIcon(buildableObjectUICategorySO.categoryIcon);
                }

                instantiatedUICategoryObjectsDictionary.Add(buildableObjectUICategorySO, categoryUIObject);

                if (categoryUIObject.TryGetComponent(out Button button))
                {
                    button.onClick.AddListener(delegate { OnCategoryButtonPressed(buildableObjectUICategorySO); });
                }
            }
        }


        public void Clear()
        {
            for (int i = m_Content.childCount - 1; i >= 0; i--)
            {
                Destroy(m_Content.GetChild(i).gameObject);
            }
        }
    }
}
