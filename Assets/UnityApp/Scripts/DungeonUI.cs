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


        //Structure
        public Toggle structureMode;
        public Toggle extendMode;
        public Toggle makeRoomMode;
        public GameObject structurePanel;


        public Toggle Remove;
        private BuildableObjectUICategorySO activeBuildableObjectUICategorySO;

        private GridManager gridManager;
        private EasyGridBuilderPro activeEasyGridBuilderPro;
        private GridMode activeGridMode;

        private void Start()
        {
            gridManager = GridManager.Instance;
            gridManager.OnActiveEasyGridBuilderProChanged += OnActiveEasyGridBuilderProChanged;
            gridManager.OnActiveGridModeChanged += OnActiveGridModeChanged;


            m_CategoryUI.OnCategoryButtonPressed += OnCategoryButtonPressedMethod;
            m_BuildableUI.OnBuildableButtonPressed += OnBuildableButtonPressedMethod;



            activeEasyGridBuilderPro = GridManager.Instance.GetActiveEasyGridBuilderPro();

            //UI
            structureMode.onValueChanged.AddListener(StructureModeValueChanged);
            extendMode.onValueChanged.AddListener(ExtendModeValueChanged);
            makeRoomMode.onValueChanged.AddListener(MakeRoomModeValueChanged);
        }


        private void OnActiveEasyGridBuilderProChanged(EasyGridBuilderPro activeEasyGridBuilderProSystem)
        {
            activeEasyGridBuilderPro = activeEasyGridBuilderProSystem;
            activeGridMode = activeEasyGridBuilderPro.GetActiveGridMode();
            m_CategoryUI.Init(activeEasyGridBuilderPro);
            m_BuildableUI.Init(activeEasyGridBuilderPro);
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

        private void OnBuildableButtonPressedMethod( BuildableObjectSO buildableObjectSO)
        {
            foreach (var easyGridBuilderPro in gridManager.GetEasyGridBuilderProSystemsList())
            {
                easyGridBuilderPro.SetInputActiveBuildableObjectSO(buildableObjectSO, onlySetBuildableExistInBuildablesList: true);
            }
        }



        private void RemoveModeValueChanged(bool arg0)
        {
            DungeonController.Instance.structureMode = arg0 ? StructureMode.CreateSpace : StructureMode.None;
        }
        private void ExtendModeValueChanged(bool arg0)
        {

        }

        private void MakeRoomModeValueChanged(bool value)
        {
            if (value)
            {
                EasyGridBuilderProController.Instance.ChangeCurrentGrid(GridType.SizeFour);
                gridManager.SetActiveGridModeInAllGrids(GridMode.BuildMode);
            }
        }


        private void StructureModeValueChanged(bool value)
        {
            structurePanel.SetActive(value);
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
