using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class DungeonUI : MonoBehaviour
    {
        [SerializeField] private CategoryObjectsPanelView m_CategoryUI;
        [SerializeField] private BuildableObjectsPanelView m_BuildableUI;


        //Structure
        public Toggle extendMode;
        public Toggle structureMode;
        public Toggle buildMode;


        private BuildableObjectUICategorySO activeBuildableObjectUICategorySO;

        private GridManager gridManager;
        private EasyGridBuilderPro activeEasyGridBuilderPro;
        private GridMode activeGridMode;

        private void Start()
        {
            gridManager = GridManager.Instance;
            gridManager.OnActiveEasyGridBuilderProChanged += OnActiveEasyGridBuilderProChanged;
            gridManager.OnActiveGridModeChanged += OnActiveGridModeChanged;


            //m_CategoryUI.OnCategoryButtonPressed += OnCategoryButtonPressedMethod;
            //m_BuildableUI.OnBuildableButtonPressed += OnBuildableButtonPressedMethod;



            activeEasyGridBuilderPro = GridManager.Instance.GetActiveEasyGridBuilderPro();

            //UI
            structureMode.onValueChanged.AddListener(StructureModeValueChanged);
            extendMode.onValueChanged.AddListener(ExtendModeValueChanged);
            buildMode.onValueChanged.AddListener(BuildModeValueChanged);
        }



        private void OnActiveEasyGridBuilderProChanged(EasyGridBuilderPro activeEasyGridBuilderProSystem)
        {
            activeEasyGridBuilderPro = activeEasyGridBuilderProSystem;
            activeGridMode = activeEasyGridBuilderPro.GetActiveGridMode();
            Init();
        }


        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {
            if (activeEasyGridBuilderPro != easyGridBuilderPro) return;
            activeGridMode = gridMode;
        }


        private void Init()
        {
            //activeBuildableObjectUICategorySO = m_CategoryUI.Init(activeEasyGridBuilderPro);
            //m_BuildableUI.Init(activeEasyGridBuilderPro);
            //if (activeBuildableObjectUICategorySO != null)
            //{
            //    HandleActiveBuildableObjectUICategorySOInteraction();
            //}
           
        }

        private void HandleActiveBuildableObjectUICategorySOInteraction()
        {
            //Image imageComponent;
            //foreach (var instantiatedUICategoryObject in m_CategoryUI.instantiatedUICategoryObjectsDictionary)
            //{
            //    if (instantiatedUICategoryObject.Key == activeBuildableObjectUICategorySO)
            //    {
            //        if (instantiatedUICategoryObject.Value.TryGetComponent<Image>(out imageComponent)) imageComponent.color = new Color(0.647f, 0.906f, 0.2f);
            //    }
            //    else if (instantiatedUICategoryObject.Value.TryGetComponent<Image>(out imageComponent)) imageComponent.color = new Color(0.125f, 0.482f, 1.0f);
            //}

            //foreach (var instantiatedUIBuildableObject in m_BuildableUI.instantiatedUIBuildableObjectsDictionary)
            //{
            //    if (instantiatedUIBuildableObject.Key.buildableObjectUICategorySO == activeBuildableObjectUICategorySO)
            //    {
            //        instantiatedUIBuildableObject.Value.gameObject.SetActive(true);
            //    }
            //    else instantiatedUIBuildableObject.Value.gameObject.SetActive(false);
            //}
        }


        private void OnCategoryButtonPressedMethod(BuildableObjectUICategorySO buildableObjectUICategorySO)
        {
            activeBuildableObjectUICategorySO = buildableObjectUICategorySO;
            HandleActiveBuildableObjectUICategorySOInteraction();
        }

        private void OnBuildableButtonPressedMethod(BuildableObjectSO buildableObjectSO)
        {
            foreach (var easyGridBuilderPro in gridManager.GetEasyGridBuilderProSystemsList())
            {
                if (buildableObjectSO is BuildableEdgeObjectSO)
                {
                    JohnnyBuildSystem.Instance.SetInputActiveBuildableObjectSO(buildableObjectSO);
                }
                else
                {
                    easyGridBuilderPro.SetInputActiveBuildableObjectSO(buildableObjectSO, onlySetBuildableExistInBuildablesList: true);
                }
            }
        }



        //private void RemoveModeValueChanged(bool arg0)
        //{
        //    DungeonController.Instance.structureMode = arg0 ? StructureMode.CreateSpace : StructureMode.None;
        //}
        private void ExtendModeValueChanged(bool arg0)
        {

        }

        private void MakeRoomModeValueChanged(bool value)
        {

        }


        private void StructureModeValueChanged(bool value)
        {
            if (value)
            {
                //EasyGridBuilderProController.Instance.ChangeCurrentGrid(GridType.SizeTwo);
                //gridManager.SetActiveGridModeInAllGrids(GridMode.BuildMode);
            }
        }

        private void BuildModeValueChanged(bool value)
        {
            if (value)
            {
                //EasyGridBuilderProController.Instance.ChangeCurrentGrid(GridType.SizeOne);
                //gridManager.SetActiveGridModeInAllGrids(GridMode.BuildMode);
            }
        }

        private void HandleBuildableObjectsUIPanelActiveSelf(GridMode gridMode)
        {
            //if (buildableObjectsUIPanel.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
            //{ 

            //}
        }

    }
}
