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

        public event OnBuildableButtonPressedDelegate OnBuildableButtonPressed;
        public delegate void OnBuildableButtonPressedDelegate(BuildableObjectSO buildableObjectSO);

        private List<BuildableObjectSO> buildableObjectSOList = new List<BuildableObjectSO>();
        public Dictionary<BuildableObjectSO, RectTransform> instantiatedUIBuildableObjectsDictionary = new Dictionary<BuildableObjectSO, RectTransform>();
        public void Init(EasyGridBuilderPro activeEasyGridBuilderPro)
        {
            Clear();

            foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableGridObjectSOList())
            {
                buildableObjectSOList.Add(buildableObjectSO);
            }

            foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableEdgeObjectSOList())
            {
                buildableObjectSOList.Add(buildableObjectSO);
            }

            InstantiateUIBuildableObjects();
        }


        private void InstantiateUIBuildableObjects()
        {
            foreach (var buildableObjectSO in buildableObjectSOList)
            {
                var buildableUIObject = Instantiate(m_ItemPrefab, m_Content);

                if (buildableObjectSO.objectIcon && buildableUIObject.TryGetComponent<IconButton>(out var iconButton))
                {
                    iconButton.SetIcon(buildableObjectSO.objectIcon);
                }

                instantiatedUIBuildableObjectsDictionary.Add(buildableObjectSO, buildableUIObject);
                if (buildableUIObject.transform.TryGetComponent(out Button button))
                {
                    button.onClick.AddListener(delegate { OnBuildableButtonPressed(buildableObjectSO); });
                }
            }
        }

        public void Clear()
        {
            for (int i = m_Content.childCount - 1; i >= 0; i--)
            {
                Destroy(m_Content.GetChild(i).gameObject);
            }
            buildableObjectSOList.Clear();
            instantiatedUIBuildableObjectsDictionary.Clear();
        }
    }
}
