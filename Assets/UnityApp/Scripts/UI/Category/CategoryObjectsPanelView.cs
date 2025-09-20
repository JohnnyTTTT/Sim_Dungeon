using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Observables;
using Loxodon.Framework.ViewModels;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class CategoryObjectsPanelViewModel : ListViewModel<CategoryObjectItemViewModel>
    {
        protected override void OnItemSelect(CategoryObjectItemViewModel item)
        {
            base.OnItemSelect(item);
            if (item.IsSelected)
            {
                BindingService.MainPanelViewModel.ActiveCategoryObjectItemView = item;
            }
            else
            {
                BindingService.MainPanelViewModel.ActiveCategoryObjectItemView = null;
            }

        }

        public CategoryObjectItemViewModel CreateItem(BuildableObjectUICategorySO buildableObjectUICategorySO)
        {
            var item = new CategoryObjectItemViewModel(this.ItemSelectCommand, buildableObjectUICategorySO);
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
                    }

                }
            }
        }
        private EasyGridBuilderPro m_ActiveEasyGridBuilderPro;

        protected override void Start()
        {
            ViewModel = BindingService.CategoryObjectsPanelViewModel;
            base.Start();
        }

        protected override void Binding(BindingSet<ViewBase<CategoryObjectsPanelViewModel>, CategoryObjectsPanelViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_ListView).For(v => v.Items).To(vm => vm.Items).OneWay();
        }

        protected override void StaticBinding(BindingSet<ViewBase<CategoryObjectsPanelViewModel>> staticBindingSet)
        {
            staticBindingSet.Bind(this.m_CanvasGroup).For(v => v.alpha).ToExpression(() =>
            BindingService.MainPanelViewModel.GameMode == GameMode.Placement ||
            BindingService.MainPanelViewModel.GameMode == GameMode.Structure ? 1f : 0f).OneWay();

            staticBindingSet.Bind(this).For(v => v.ActiveEasyGridBuilderPro).To(() => BindingService.MainPanelViewModel.ActiveEasyGridBuilderPro).OneWay();

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

        //private void InstantiateUICategoryObjects()
        //{
        //    foreach (var buildableObjectUICategorySO in buildableObjectUICategorySOList)
        //    {
        //        var categoryUIObject = Instantiate(m_ItemPrefab, m_Content);

        //        if (buildableObjectUICategorySO.categoryIcon && categoryUIObject.TryGetComponent<IconButton>(out var iconButton))
        //        {
        //            iconButton.SetIcon(buildableObjectUICategorySO.categoryIcon);
        //        }

        //        instantiatedUICategoryObjectsDictionary.Add(buildableObjectUICategorySO, categoryUIObject);

        //        if (categoryUIObject.TryGetComponent(out Button button))
        //        {
        //            button.onClick.AddListener(delegate { OnCategoryButtonPressed(buildableObjectUICategorySO); });
        //        }
        //    }
        //}


    }
}
