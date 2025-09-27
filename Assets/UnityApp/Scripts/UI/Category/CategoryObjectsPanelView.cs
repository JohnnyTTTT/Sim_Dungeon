using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Observables;
using Loxodon.Framework.ViewModels;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class CategoryObjectsPanelViewModel : ListViewModel<CategoryObjectItemViewModel>
    {
        public CategoryObjectItemViewModel destroyItem;
        public CategoryObjectItemViewModel moveItem;

        protected override void OnSelectedItemChanged()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.GridMode == GridMode.BuildMode)
                {
                    BindingService.MainGameViewModel.ActiveCategoryObjectItemView = SelectedItem;
                }
                BindingService.MainGameViewModel.GridType = SelectedItem.GridType;
                if (GridManager.Instance.GetActiveEasyGridBuilderPro().GetActiveGridMode() != SelectedItem.GridMode)
                {
                    GridManager.Instance.SetActiveGridModeInAllGrids(SelectedItem.GridMode);
                }
            }
            else
            {
                BindingService.MainGameViewModel.ActiveCategoryObjectItemView = null;
                if (GridManager.Instance.GetActiveEasyGridBuilderPro().GetActiveGridMode() != GridMode.None)
                {
                    GridManager.Instance.SetActiveGridModeInAllGrids(GridMode.None);
                }
                BindingService.MainGameViewModel.GridType = GridType.Nothing;
            }
        }

        public CategoryObjectItemViewModel CreateItem(BuildableObjectUICategorySO buildableObjectUICategorySO, GridMode gridMode, GridType gridType)
        {
            var item = new CategoryObjectItemViewModel(gridMode, gridType, this.ItemSelectCommand, ItemClickCommand, buildableObjectUICategorySO);
            return item;
        }

    }

    public class CategoryObjectsPanelView : ViewBase<CategoryObjectsPanelViewModel>
    {
        public Dictionary<GameMode, ObservableList<CategoryObjectItemViewModel>> AllItems = new Dictionary<GameMode, ObservableList<CategoryObjectItemViewModel>>();

        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private CategoryObjectsListView m_ListView;
        [SerializeField] private CategoryObjectItemView m_DestroyItemView;
        [SerializeField] private CategoryObjectItemView m_MoveItemView;

        protected override void Start()
        {
            ViewModel = BindingService.CategoryObjectsPanelViewModel;
            MainGameViewModel.OnGameModeChanged += OnGameModeChanged;
            GridManager.Instance.OnActiveGridModeChanged += OnActiveGridModeChanged;
            base.Start();
        }

        private void OnGameModeChanged(GameMode gameMode)
        {
            if (AllItems.TryGetValue(gameMode, out var datas))
            {
                ViewModel.Items = datas;
                ViewModel.SelectedItem = null;
            }
        }

        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {
            if (gridMode != GridMode.BuildMode && ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem = null;
            }
        }

        protected override void Binding(BindingSet<ViewBase<CategoryObjectsPanelViewModel>, CategoryObjectsPanelViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_ListView).For(v => v.Items).To(vm => vm.Items).OneWay();
            //bindingSet.Bind(m_DestroyItemView).For(v => v.ViewModel).To(vm => vm.destroyItem).OneWay();
            //bindingSet.Bind(m_MoveItemView).For(v => v.ViewModel).To(vm => vm.moveItem).OneWay();
        }

        protected override void StaticBinding(BindingSet<ViewBase<CategoryObjectsPanelViewModel>> staticBindingSet)
        {
            staticBindingSet.Bind(this.gameObject).For(v => v.activeSelf).To(() => BindingService.MainGameViewModel.ShouldShowCategoryUI).OneWay();
        }

        public void Init()
        {
            ViewModel.destroyItem = ViewModel.CreateItem(null, GridMode.DestroyMode, GridType.Large);
            ViewModel.moveItem = ViewModel.CreateItem(null, GridMode.MoveMode, GridType.Small);

            m_DestroyItemView.ViewModel =(ViewModel.destroyItem);
            m_MoveItemView.ViewModel=(ViewModel.moveItem);

            AllItems[GameMode.Structure] = new ObservableList<CategoryObjectItemViewModel>();
            foreach (var category in BuildableAssets.Instance.Structures)
            {
                foreach (var buildableObjectSO in category.buildableObjectSOs)
                {
                    if (buildableObjectSO.buildableObjectUICategorySO != null)
                    {
                        if (!AllItems[GameMode.Structure].Any(x => x.Data == buildableObjectSO))
                        {
                            var item = ViewModel.CreateItem(buildableObjectSO.buildableObjectUICategorySO, GridMode.BuildMode, category.gridType);
                            AllItems[GameMode.Structure].Add(item);
                        }
                    }
                }
            }

            AllItems[GameMode.Placement] = new ObservableList<CategoryObjectItemViewModel>();
            foreach (var category in BuildableAssets.Instance.Placements)
            {
                foreach (var buildableObjectSO in category.buildableObjectSOs)
                {
                    if (buildableObjectSO.buildableObjectUICategorySO != null)
                    {
                        if (!AllItems[GameMode.Placement].Any(x => x.Data == buildableObjectSO))
                        {
                            var item = ViewModel.CreateItem(buildableObjectSO.buildableObjectUICategorySO, GridMode.BuildMode, category.gridType);
                            AllItems[GameMode.Placement].Add(item);
                        }
                    }
                }
            }

            //foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableGridObjectSOList())
            //{
            //    if (buildableObjectSO.buildableObjectUICategorySO != null)
            //    {
            //        uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
            //    }
            //}
            //foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableEdgeObjectSOList())
            //{
            //    if (buildableObjectSO.buildableObjectUICategorySO != null)
            //    {
            //        uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
            //    }
            //}
            //foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableFreeObjectSOList())
            //{
            //    if (buildableObjectSO.buildableObjectUICategorySO != null)
            //    {
            //        uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
            //    }
            //}
            //foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableCornerObjectSOList())
            //{
            //    if (buildableObjectSO.buildableObjectUICategorySO != null)
            //    {
            //        uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
            //    }
            //}

            //foreach (var uniqueCategories in uniqueCategorieshashSet)
            //{
            //    var item = ViewModel.CreateItem(uniqueCategories);
            //    AllItems[activeEasyGridBuilderPro].Add(item);
            //}

        }

        private void OnActiveBuildableSOChanged(EasyGridBuilderPro easyGridBuilderPro, BuildableObjectSO buildableObjectSO)
        {
            //if (BindingService.MainGameViewModel.ActiveEasyGridBuilderPro != easyGridBuilderPro) return;
            //if (buildableObjectSO == null && ViewModel.SelectedItem != null)
            //{
            //    ViewModel.SelectedItem = null;
            //}
        }


    }
}
