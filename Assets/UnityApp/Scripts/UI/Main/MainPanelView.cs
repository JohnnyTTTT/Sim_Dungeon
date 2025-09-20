using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Commands;
using Loxodon.Framework.Execution;
using Loxodon.Framework.ViewModels;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public enum GameMode
    {
        None,
        Default,
        Structure,
        Placement,
    }

    public enum StructureMode
    {
        None,
        LandExpand,
    }

    public class MainGameViewModel : ViewModelBase
    {
        public GameMode GameMode
        {
            get
            {
                return m_GameMode;
            }
            set
            {
                if (m_GameMode != value)
                {
                    Set(ref m_GameMode, value);
                    switch (m_GameMode)
                    {
                        case GameMode.None:
                            break;
                        case GameMode.Default:
                            break;
                        case GameMode.Structure:
                            GridType = GridType.SizeTwo;
                            break;
                        case GameMode.Placement:
                            GridType = GridType.SizeOne;
                            break;
                        default:
                            break;
                    }
                    RaisePropertyChanged();
                }
            }
        }
        private GameMode m_GameMode = GameMode.Default;

        public StructureMode StructureMode
        {
            get
            {
                return m_StructureMode;
            }
            set
            {
                if (m_StructureMode != value)
                {
                    Set(ref m_StructureMode, value);
                    if (m_StructureMode != StructureMode.None)
                    {
                        BindingService.CategoryObjectsPanelViewModel.SetSelectItem(null);
                        ActiveCategoryObjectItemView = null;
                    }
                    RaisePropertyChanged();
                }
            }
        }
        private StructureMode m_StructureMode = StructureMode.None;

        public GridMode GridMode
        {
            get
            {
                return m_GridMode;
            }
            set
            {
                if (m_GridMode != value)
                {
                    Set(ref m_GridMode, value);
                    GridManager.Instance.SetActiveGridModeInAllGrids(m_GridMode);
                }
            }
        }
        private GridMode m_GridMode;

        public GridType GridType
        {
            get
            {
                return m_GridType;
            }
            set
            {
                if (m_GridType != value)
                {
                    Set(ref m_GridType, value);
                    var size1 = EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize1;
                    var size2 = EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2;
                    switch (m_GridType)
                    {
                        case GridType.SizeOne:
                            size1.gameObject.SetActive(true);
                            size2.gameObject.SetActive(false);
                            GridManager.Instance.SetActiveGridSystem(size1);
                            break;
                        case GridType.SizeTwo:
                            size2.gameObject.SetActive(true);
                            size1.gameObject.SetActive(false);
                            GridManager.Instance.SetActiveGridSystem(size2);
                            break;
                    }
                    ActiveEasyGridBuilderPro = GridManager.Instance.GetActiveEasyGridBuilderPro() as EasyGridBuilderProXZ;
                    GridManager.Instance.SetActiveGridModeInAllGrids(m_GridMode);
                }
            }
        }
        private GridType m_GridType;

        public EasyGridBuilderProXZ ActiveEasyGridBuilderPro
        {
            get
            {
                return m_ActiveEasyGridBuilderPro;
            }
            set
            {
                if (m_ActiveEasyGridBuilderPro != value)
                {
                    Set(ref m_ActiveEasyGridBuilderPro, value);
                    RaisePropertyChanged();
                }
            }
        }
        private EasyGridBuilderProXZ m_ActiveEasyGridBuilderPro;

        public CategoryObjectItemViewModel ActiveCategoryObjectItemView
        {
            get
            {
                return m_activeCategoryObjectItemView;
            }
            set
            {
                if (m_activeCategoryObjectItemView != value)
                {
                    Set(ref m_activeCategoryObjectItemView, value);
                    if (m_activeCategoryObjectItemView != null)
                    {
                        if (StructureMode != StructureMode.None)
                        {
                            StructureMode = StructureMode.None;
                        }
                        GridMode = GridMode.BuildMode;
                    }
                    else
                    {
                        GridMode = GridMode.None;
                    }
                    RaisePropertyChanged();
                }
            }
        }
        private CategoryObjectItemViewModel m_activeCategoryObjectItemView;

        public BuildableObjectSO InputActiveBuildableObjectSO
        {
            get
            {
                return m_InputActiveBuildableObjectSO;
            }
            set
            {
                if (m_InputActiveBuildableObjectSO != value)
                {
                    Set(ref m_InputActiveBuildableObjectSO, value);
                    foreach (var easyGridBuilderPro in GridManager.Instance.GetEasyGridBuilderProSystemsList())
                    {
                        easyGridBuilderPro.SetInputActiveBuildableObjectSO(m_InputActiveBuildableObjectSO, onlySetBuildableExistInBuildablesList: true);
                    }
                }
            }
        }
        private BuildableObjectSO m_InputActiveBuildableObjectSO;

    }

    public class MainPanelView : ViewBase<MainGameViewModel>
    {

        [SerializeField] private CategoryObjectsPanelView m_CategoryObjectsPanelView;
        [SerializeField] private BuildableObjectsPanelView m_BuildableObjectsPanelView;


        protected override void Start()
        {
            ViewModel = BindingService.MainPanelViewModel;
            //GridManager.Instance.OnActiveEasyGridBuilderProChanged += OnActiveEasyGridBuilderProChanged;
            GridManager.Instance.OnActiveGridModeChanged += OnActiveGridModeChanged;

            base.Start();
            StartCoroutine(PostStart());
        }

        private IEnumerator PostStart()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            //m_CategoryObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize1);
            m_CategoryObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2);

            //m_BuildableObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize1);
            m_BuildableObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2);

            Debug.Log("[-----UI-----] : Init CategoryObjects And BuildableObjects");
        }

        protected override void Binding(BindingSet<ViewBase<MainGameViewModel>, MainGameViewModel> bindingSet)
        {

        }

        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {

        }

        //private void OnActiveEasyGridBuilderProChanged(EasyGridBuilderPro activeEasyGridBuilderProSystem)
        //{
        //    BindingService.CategoryObjectsPanelViewModel.ActiveEasyGridBuilderPro = activeEasyGridBuilderProSystem;
        //}

    }
}
