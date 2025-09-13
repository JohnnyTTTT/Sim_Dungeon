using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class DungeonUI : MonoBehaviour
    {
        [SerializeField] private EasyGridBuilderProController m_EasyGridBuilderProController;
        [SerializeField] private CategoryUI m_CategoryUI;
        [SerializeField] private BuildableUI m_BuildableUI;
        public Toggle buildMode;
        public Toggle Remove;
        private BuildableObjectUICategorySO activeBuildableObjectUICategorySO;

        private GridManager gridManager;
        private EasyGridBuilderPro activeEasyGridBuilderPro;
        private GridMode activeGridMode;

        private void Start()
        {

            gridManager = GridManager.Instance;
            Debug.Log(gridManager, gridManager);
            gridManager.OnActiveEasyGridBuilderProChanged += OnActiveEasyGridBuilderProChanged;
            gridManager.OnActiveGridModeChanged += OnActiveGridModeChanged;


            m_CategoryUI.OnCategoryButtonPressed += OnCategoryButtonPressedMethod;
            m_BuildableUI.OnBuildableButtonPressed += OnBuildableButtonPressedMethod;



            activeEasyGridBuilderPro = GridManager.Instance.GetActiveEasyGridBuilderPro();
            if (activeEasyGridBuilderPro)
            {
                activeGridMode = activeEasyGridBuilderPro.GetActiveGridMode();
                m_CategoryUI.Init(activeEasyGridBuilderPro);
                m_BuildableUI.Init(activeEasyGridBuilderPro);
            }
        }

        private IEnumerator LateStart()
        {
            yield return new WaitForEndOfFrame();


        }



        private void OnActiveEasyGridBuilderProChanged(EasyGridBuilderPro activeEasyGridBuilderProSystem)
        {
            Debug.Log(111);
            activeEasyGridBuilderPro = activeEasyGridBuilderProSystem;
            activeGridMode = activeEasyGridBuilderPro.GetActiveGridMode();

        }

        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {
            if (activeEasyGridBuilderPro != easyGridBuilderPro) return;
            activeGridMode = gridMode;
        }

        private void OnCategoryButtonPressedMethod(bool isOn, BuildableObjectUICategorySO buildableObjectUICategorySO)
        {
            if (isOn)
            {
                activeBuildableObjectUICategorySO = buildableObjectUICategorySO;
            }
        }

        private void OnBuildableButtonPressedMethod(bool isOn, BuildableObjectSO buildableObjectSO)
        {
            foreach (var easyGridBuilderPro in gridManager.GetEasyGridBuilderProSystemsList())
            {
                easyGridBuilderPro.SetInputActiveBuildableObjectSO(buildableObjectSO, onlySetBuildableExistInBuildablesList: true);
            }
        }








        private void OnEnable()
        {
            buildMode.onValueChanged.AddListener(BuildingModeValueChanged);
            Remove.onValueChanged.AddListener(RemoveModeValueChanged);
        }

        private void RemoveModeValueChanged(bool arg0)
        {
            DungeonController.Instance.structureMode = arg0 ? StructureMode.CreateSpace : StructureMode.None;
        }

        private void BuildingModeValueChanged(bool arg0)
        {
            if (arg0)
            {
                gridManager.SetActiveGridModeInAllGrids(GridMode.BuildMode);
            }

            //m_EasyGridBuilderProController.SetAllDisable(arg0);
        }

        private void HandleBuildableObjectsUIPanelActiveSelf(GridMode gridMode)
        {
            //if (buildableObjectsUIPanel.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
            //{ 

            //}
        }

    }
}
