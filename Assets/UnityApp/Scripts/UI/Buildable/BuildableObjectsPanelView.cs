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

    public class BuildableObjectsPanelViewModel : ListViewModel<BuildableObjectItemViewModel>
    {
        protected override void OnItemSelect(BuildableObjectItemViewModel item)
        {
            base.OnItemSelect(item);
            if (item != null && item.IsSelected)
            {
                BindingService.MainPanelViewModel.InputActiveBuildableObjectSO = item.Data;
            }
        }

        public BuildableObjectItemViewModel CreateItem(BuildableObjectSO buildableObjectSO)
        {
            var item = new BuildableObjectItemViewModel(this.ItemSelectCommand, buildableObjectSO);
            return item;
        }
    }

    public class BuildableObjectsPanelView : ViewBase<BuildableObjectsPanelViewModel>
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private BuildableObjectListView m_ListView;
        public Dictionary<BuildableObjectUICategorySO, ObservableList<BuildableObjectItemViewModel>> AllItems = new Dictionary<BuildableObjectUICategorySO, ObservableList<BuildableObjectItemViewModel>>();

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
                    m_activeCategoryObjectItemView = value;

                    if (m_activeCategoryObjectItemView! != null)
                    {
                        if (AllItems.TryGetValue(m_activeCategoryObjectItemView.Data, out var datas))
                        {
                            ViewModel.Items = datas;
                        }
                    }

                }
            }
        }
        private CategoryObjectItemViewModel m_activeCategoryObjectItemView;


        protected override void Start()
        {
            ViewModel = BindingService.BuildableObjectsPanelViewModel;
            GridManager.Instance.OnActiveBuildableSOChanged += OnActiveBuildableSOChanged;
            base.Start();
        }

        protected override void Binding(BindingSet<ViewBase<BuildableObjectsPanelViewModel>, BuildableObjectsPanelViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_ListView).For(v => v.Items).To(vm => vm.Items).OneWay();
        }

        protected override void StaticBinding(BindingSet<ViewBase<BuildableObjectsPanelViewModel>> staticBindingSet)
        {
            staticBindingSet.Bind(this.m_CanvasGroup).For(v => v.alpha).ToExpression(() => BindingService.MainPanelViewModel.ActiveCategoryObjectItemView != null ? 1f : 0f).OneWay();

            staticBindingSet.Bind(this).For(v => v.ActiveCategoryObjectItemView).To(() => BindingService.MainPanelViewModel.ActiveCategoryObjectItemView).OneWay();
        }

        public void Init(EasyGridBuilderPro activeEasyGridBuilderPro)
        {
            var buildableObjectSOs = new HashSet<BuildableObjectSO>();
            foreach (var buildableObjectSO in activeEasyGridBuilderPro.GetBuildableGridObjectSOList())
            {
                buildableObjectSOs.Add(buildableObjectSO);
            }
            foreach (var buildableFreeObjectSO in activeEasyGridBuilderPro.GetBuildableFreeObjectSOList())
            {
                buildableObjectSOs.Add(buildableFreeObjectSO);
            }


            foreach (var buildableObjectSO in buildableObjectSOs)
            {
                if (buildableObjectSO.buildableObjectUICategorySO != null)
                {
                    var item = ViewModel.CreateItem(buildableObjectSO);
                    if (!AllItems.ContainsKey(item.Data.buildableObjectUICategorySO))
                    {
                        AllItems[item.Data.buildableObjectUICategorySO] = new ObservableList<BuildableObjectItemViewModel>();
                    }
                    AllItems[item.Data.buildableObjectUICategorySO].Add(item);
                }
            }
        }


        private void OnActiveBuildableSOChanged(EasyGridBuilderPro easyGridBuilderPro, BuildableObjectSO buildableObjectSO)
        {
            if (BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro != easyGridBuilderPro) return;
            if (buildableObjectSO == null && ViewModel.SelectedItem != null)
            {
                Debug.Log(11);
                ViewModel.SetSelectItem(null);
            }
        }
    }
}
