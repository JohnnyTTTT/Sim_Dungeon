using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
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
                    //switch (m_StructureMode)
                    //{
                    //    case StructureMode.None:
                    //        //BindingService.CategoryObjectsPanelViewModel.SetSelectItem(null);
                    //        ActiveCategoryObjectItemView = null;
                    //        break;
                    //    case StructureMode.LandExpand:
                    //        GridMode = GridMode.BuildMode;
                    //        break;
                    //    default:
                    //        break;
                    //}
                    RaisePropertyChanged();
                }
            }
        }
        private StructureMode m_StructureMode = StructureMode.None;

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
                    m_GridType = value;
                    var size1 = EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize1;
                    var size2 = EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2;
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

        protected override void Start()
        {
            ViewModel = BindingService.MainGameViewModel;
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
