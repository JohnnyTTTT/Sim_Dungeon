using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Observables;
using Loxodon.Framework.ViewModels;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class CategoryObjectsPanelViewModel : ListViewModel<CategoryObjectItemViewModel>
    {
        protected override void OnSelectedItemChanged()
        {
            if (SelectedItem != null)
            {
                BindingService.MainGameViewModel.ActiveCategoryObjectItemView = SelectedItem;
            }
            else
            {
                BindingService.MainGameViewModel.ActiveCategoryObjectItemView = null;
            }
        }

        public CategoryObjectItemViewModel CreateItem(BuildableObjectUICategorySO buildableObjectUICategorySO)
        {
            var item = new CategoryObjectItemViewModel(this.ItemSelectCommand, ItemClickCommand,buildableObjectUICategorySO);
            return item;
        }

    }

    public class CategoryObjectsPanelView : ViewBase<CategoryObjectsPanelViewModel>
    {
        public Dictionary<EasyGridBuilderPro, ObservableList<CategoryObjectItemViewModel>> AllItems = new Dictionary<EasyGridBuilderPro, ObservableList<CategoryObjectItemViewModel>>();

        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private CategoryObjectsListView m_ListView;

        public EasyGridBuilderPro ActiveEasyGridBuilderPro
        {
            get
            {
                return m_ActiveEasyGridBuilderPro;
            }
            set
            {
                if (m_ActiveEasyGridBuilderPro != value)
                {
                    m_ActiveEasyGridBuilderPro = value;
                    if (AllItems.TryGetValue(m_ActiveEasyGridBuilderPro, out var datas))
                    {
                        ViewModel.Items = datas;
                        ViewModel.SelectedItem = null;
                    }
                }
            }
        }
        private EasyGridBuilderPro m_ActiveEasyGridBuilderPro;

        protected override void Start()
        {
            ViewModel = BindingService.CategoryObjectsPanelViewModel;
            //GridManager.Instance.OnActiveBuildableSOChanged += OnActiveBuildableSOChanged;
            base.Start();
        }



        protected override void Binding(BindingSet<ViewBase<CategoryObjectsPanelViewModel>, CategoryObjectsPanelViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_ListView).For(v => v.Items).To(vm => vm.Items).OneWay();
        }

        protected override void StaticBinding(BindingSet<ViewBase<CategoryObjectsPanelViewModel>> staticBindingSet)
        {
            staticBindingSet.Bind(this.m_CanvasGroup).For(v => v.alpha).ToExpression(() =>
            BindingService.MainGameViewModel.GameMode == GameMode.Placement ||
            BindingService.MainGameViewModel.GameMode == GameMode.Structure ? 1f : 0f).OneWay();

            staticBindingSet.Bind(this).For(v => v.ActiveEasyGridBuilderPro).To(() => BindingService.MainGameViewModel.ActiveEasyGridBuilderPro).OneWay();

        }

        public void Init(EasyGridBuilderPro activeEasyGridBuilderPro)
        {
            AllItems[activeEasyGridBuilderPro] = new ObservableList<CategoryObjectItemViewModel>();

            var uniqueCategorieshashSet = new HashSet<BuildableObjectUICategorySO>();
            foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableGridObjectSOList())
            {
                if (buildableObjectSO.buildableObjectUICategorySO != null)
                {
                    uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
                }
            }
            foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableEdgeObjectSOList())
            {
                if (buildableObjectSO.buildableObjectUICategorySO != null)
                {
                    uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
                }
            }
            foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableFreeObjectSOList())
            {
                if (buildableObjectSO.buildableObjectUICategorySO != null)
                {
                    uniqueCategorieshashSet.Add(buildableObjectSO.buildableObjectUICategorySO);
                }
            }


            foreach (var uniqueCategories in uniqueCategorieshashSet)
            {
                var item = ViewModel.CreateItem(uniqueCategories);
                AllItems[activeEasyGridBuilderPro].Add(item);
            }

        }

        private void OnActiveBuildableSOChanged(EasyGridBuilderPro easyGridBuilderPro, BuildableObjectSO buildableObjectSO)
        {
            if (BindingService.MainGameViewModel.ActiveEasyGridBuilderPro != easyGridBuilderPro) return;
            if (buildableObjectSO == null && ViewModel.SelectedItem != null)
            {
                ViewModel.SelectedItem = null;
            }
        }


    }
}
