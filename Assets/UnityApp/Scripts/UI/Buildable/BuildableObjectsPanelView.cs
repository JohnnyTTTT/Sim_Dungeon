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
        protected override void OnItemClick(BuildableObjectItemViewModel item)
        {
            foreach (var easyGridBuilderPro in GridManager.Instance.GetEasyGridBuilderProSystemsList())
            {
                easyGridBuilderPro.SetInputActiveBuildableObjectSO(item.Data, onlySetBuildableExistInBuildablesList: true);
            }
        }

        public BuildableObjectItemViewModel CreateItem(BuildableObjectSO buildableObjectSO)
        {
            var item = new BuildableObjectItemViewModel(ItemSelectCommand, ItemClickCommand, buildableObjectSO);
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
            //GridManager.Instance.OnActiveBuildableSOChanged += OnActiveBuildableSOChanged;
            base.Start();
        }

        protected override void Binding(BindingSet<ViewBase<BuildableObjectsPanelViewModel>, BuildableObjectsPanelViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_ListView).For(v => v.Items).To(vm => vm.Items).OneWay();
        }

        protected override void StaticBinding(BindingSet<ViewBase<BuildableObjectsPanelViewModel>> staticBindingSet)
        {
            staticBindingSet.Bind(this.gameObject).For(v => v.activeSelf).To(() => BindingService.MainGameViewModel.ShouldShowBuildableUI).OneWay();
            staticBindingSet.Bind(this).For(v => v.ActiveCategoryObjectItemView).To(() => BindingService.MainGameViewModel.ActiveCategoryObjectItemView).OneWay();
        }

        public void Init()
        {
            var buildableObjectSOs = new HashSet<BuildableObjectSO>();
            foreach (var category in BuildableAssets.Instance.Structures)
            {
                foreach (var buildableObjectSO in category.buildableObjectSOs)
                {
                    if (buildableObjectSO.buildableObjectUICategorySO != null)
                    {
                        buildableObjectSOs.Add(buildableObjectSO);
                    }
                }
            }
            foreach (var category in BuildableAssets.Instance.Placements)
            {
                foreach (var buildableObjectSO in category.buildableObjectSOs)
                {
                    if (buildableObjectSO.buildableObjectUICategorySO != null)
                    {
                        buildableObjectSOs.Add(buildableObjectSO);
                    }
                }
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
            //if (BindingService.MainGameViewModel.ActiveEasyGridBuilderPro != easyGridBuilderPro) return;
            //if (buildableObjectSO == null && ViewModel.SelectedItem != null)
            //{
            //    //ViewModel.SelectedItem = null;
            //}
        }
    }
}
