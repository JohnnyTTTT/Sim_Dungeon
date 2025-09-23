using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Observables;
using Loxodon.Framework.ViewModels;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
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

        public bool IsBuildableMode
        {
            get
            {
                return GameMode == GameMode.Structure || GameMode == GameMode.Placement;
            }
        }


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
                    var size1 = SpawnManager.Instance.m_EasyGridBuilderProSize1;
                    var size2 = SpawnManager.Instance.m_EasyGridBuilderProSize2;
                    switch (m_GridType)
                    {
                        case GridType.SizeOne:
                            size1.gameObject.SetActive(true);
                            size2.gameObject.SetActive(false);
                            BindingService.MainGameViewModel.ActiveEasyGridBuilderPro = size1;
                            break;
                        case GridType.SizeTwo:
                            size2.gameObject.SetActive(true);
                            size1.gameObject.SetActive(false);
                            BindingService.MainGameViewModel.ActiveEasyGridBuilderPro = size2;
                            break;
                    }
                }
            }
        }
        private GridType m_GridType;

        public bool IsLandExpandMode
        {
            get
            {
                return m_IsLandExpandMode;
            }
            set
            {
                if (m_IsLandExpandMode != value)
                {
                    Set(ref m_IsLandExpandMode, value);
                    var grid = SpawnManager.Instance.m_EasyGridBuilderProSize2;
                    var position = grid.transform.position;
                    if (m_IsLandExpandMode)
                    {
                        grid.transform.position = new Vector3(position.x, 4f, position.z);
                        grid.SetInputActiveBuildableObjectSO(SpawnManager.Instance.defaultAreaExpand, onlySetBuildableExistInBuildablesList: false);
                    }
                    else
                    {
                        grid.transform.position = new Vector3(position.x, 0f, position.z);
                    }
                    RaisePropertyChanged();
                }
            }
        }
        private bool m_IsLandExpandMode;

        public bool IsDestroyMode
        {
            get
            {
                return m_IsDestroyMode;
            }
            set
            {
                if (m_IsDestroyMode != value)
                {
                    Set(ref m_IsDestroyMode, value);
                    GridManager.Instance.SetActiveGridModeInAllGrids(GridMode.DestroyMode);
                    RaisePropertyChanged();
                }
            }
        }
        private bool m_IsDestroyMode;

        public bool IsBuildMode
        {
            get
            {
                return m_IsBuildMode;
            }
            set
            {
                if (m_IsBuildMode != value)
                {
                    Set(ref m_IsBuildMode, value);
                }
            }
        }
        private bool m_IsBuildMode;

        public bool ShouldShowCategoryUI
        {
            get
            {
                return IsBuildableMode;
            }
        }

        public bool ShouldShowBuildableUI
        {
            get
            {
                return IsBuildableMode && ActiveCategoryObjectItemView != null && !IsDestroyMode;
            }
        }

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
                    GridManager.Instance.SetActiveGridSystem(m_ActiveEasyGridBuilderPro);
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
                    //if (m_activeCategoryObjectItemView != null)
                    //{
                    //    if (StructureMode != StructureMode.None)
                    //    {
                    //        StructureMode = StructureMode.None;
                    //    }
                    //    GridMode = GridMode.BuildMode;
                    //}
                    //else
                    //{
                    //    GridMode = GridMode.None;
                    //}
                    RaisePropertyChanged();
                }
            }
        }
        private CategoryObjectItemViewModel m_activeCategoryObjectItemView;

        //public BuildableObjectSO InputActiveBuildableObjectSO
        //{
        //    get
        //    {
        //        return m_InputActiveBuildableObjectSO;
        //    }
        //    set
        //    {
        //        if (m_InputActiveBuildableObjectSO != value)
        //        {
        //            Set(ref m_InputActiveBuildableObjectSO, value);
        //            //if (m_InputActiveBuildableObjectSO != null)
        //            //{
        //            //    GridManager.Instance.SetActiveGridModeInAllGrids(GridMode.BuildMode);
        //            //}
        //            foreach (var easyGridBuilderPro in GridManager.Instance.GetEasyGridBuilderProSystemsList())
        //            {
        //                easyGridBuilderPro.SetInputActiveBuildableObjectSO(m_InputActiveBuildableObjectSO, onlySetBuildableExistInBuildablesList: true);
        //            }
        //            RaisePropertyChanged();
        //        }
        //    }
        //}
        //private BuildableObjectSO m_InputActiveBuildableObjectSO;

    }
    public class MainGameView : ViewBase<MainGameViewModel>
    {
        public static MainGameView Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<MainGameView>();
                }
                return s_Instance;
            }

        }
        private static MainGameView s_Instance;

        private GridManager m_GridManager;

        protected override void Start()
        {
            ViewModel = BindingService.MainGameViewModel;
            m_GridManager = GridManager.Instance;
            m_GridManager.OnActiveGridModeChanged += OnActiveGridModeChanged;
        }

        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {
            Debug.Log(gridMode);
            switch (gridMode)
            {
                case GridMode.None:
                    ViewModel.IsBuildMode = false;
                    ViewModel.IsLandExpandMode = false;
                    ViewModel.IsDestroyMode = false;
                    break;
                case GridMode.BuildMode:
                    ViewModel.IsBuildMode = true;
                    ViewModel.IsLandExpandMode = false;
                    ViewModel.IsDestroyMode = false;
                    break;
                case GridMode.DestroyMode:
                    ViewModel.IsBuildMode = false;
                    ViewModel.IsLandExpandMode = false;
                    ViewModel.IsDestroyMode = true;
                    break;
                case GridMode.SelectMode:
                    ViewModel.IsBuildMode = false;
                    ViewModel.IsLandExpandMode = false;
                    ViewModel.IsDestroyMode = false;
                    break;
                case GridMode.MoveMode:
                    ViewModel.IsBuildMode = false;
                    ViewModel.IsLandExpandMode = false;
                    ViewModel.IsDestroyMode = false;
                    break;
                default:
                    break;
            }
        }

        //[ShowInInspector]
        //public GridMode GridMode
        //{
        //    get
        //    {
        //        //if (BindingService.MainGameViewModel.ActiveEasyGridBuilderPro != null)
        //        //{
        //        //    return BindingService.MainGameViewModel.ActiveEasyGridBuilderPro.GetActiveGridMode();
        //        //}
        //        return m_GridMode;
        //    }
        //    set
        //    {
        //        if (m_GridMode != value)
        //        {
        //            m_GridMode = value;
        //            Debug.Log(1111);
        //            GridManager.Instance.SetActiveGridModeInAllGrids(m_GridMode);
        //        }
        //    }
        //}
        //private GridMode m_GridMode;

        protected override void Binding(BindingSet<ViewBase<MainGameViewModel>, MainGameViewModel> bindingSet)
        {
            //bindingSet.Bind(this).For(v => v.GridMode).ToExpression(vm => vm.InputActiveBuildableObjectSO != null ? GridMode.BuildMode : GridMode.None).OneWay();
            //bindingSet.Bind(this).For(v => v.GridType).ToExpression(vm => vm.InputActiveBuildableObjectSO != null);
        }


        //protected override void StaticBinding(BindingSet<ViewBase<MainGameViewModel>> staticBindingSet)
        //{
        //    staticBindingSet.Bind(this).For(v => v.GridType).ToExpression(() => BindingService.MainGameViewModel.GameMode == m_BingdingedGameMode).OneWay();
        //}
    }
}
