using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class CategoryObjectItemViewModel : SelectableItemViewModel
    {
        public BuildableObjectUICategorySO Data
        {
            get { return this.data; }
            set { this.Set(ref data, value); }
        }
        private BuildableObjectUICategorySO data;

        public Sprite Icon
        {
            get { return this.icon; }
            set { this.Set(ref icon, value); }
        }
        private Sprite icon;

        public bool Active
        {
            get { return this.active; }
            set { this.Set(ref active, value); }
        }
        private bool active;

        public CategoryObjectItemViewModel(Loxodon.Framework.Commands.ICommand selectCommand,
            Loxodon.Framework.Commands.ICommand clickCommand,
            BuildableObjectUICategorySO categorySO) :
            base(selectCommand, clickCommand)
        {
            Data = categorySO;
            Icon = categorySO.categoryIcon;
        }
    }
    public class CategoryObjectItemView : ListButtonView<CategoryObjectItemViewModel>
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private Image[] m_Icons;

        protected override void Binding(BindingSet<ViewBase<CategoryObjectItemViewModel>, CategoryObjectItemViewModel> bindingSet)
        {
            base.Binding(bindingSet);
            foreach (var item in m_Icons)
            {
                bindingSet.Bind(item).For(v => v.sprite).To(vm => vm.Icon).OneWay();
            }
            bindingSet.Bind(this.m_Button).For(v => v.onClick).To(vm => vm.SelectCommand).CommandParameter(this.ViewModel);
        }

    }
}
