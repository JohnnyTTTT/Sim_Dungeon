using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using SoulGames.EasyGridBuilderPro;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class BuildableObjectItemViewModel : SelectableItemViewModel
    {
        public BuildableObjectSO Data
        {
            get { return this.data; }
            set { this.Set(ref data, value); }
        }
        private BuildableObjectSO data;

        public Sprite Icon
        {
            get { return this.icon; }
            set { this.Set(ref icon, value); }
        }
        private Sprite icon;

        public string Title
        {
            get { return this.m_Title; }
            set { this.Set(ref m_Title, value); }
        }
        private string m_Title;

        public bool Active
        {
            get { return this.active; }
            set { this.Set(ref active, value); }
        }
        private bool active;

        public BuildableObjectItemViewModel(Loxodon.Framework.Commands.ICommand selectCommand,
            Loxodon.Framework.Commands.ICommand clickCommand,
            BuildableObjectSO buildableObjectSO) :
            base(selectCommand, clickCommand)
        {
            Data = buildableObjectSO;
            Icon = buildableObjectSO.objectIcon;
            Title = buildableObjectSO.objectName;
        }
    }

    public class BuildableObjectItemView : ViewBase<BuildableObjectItemViewModel>
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private Image[] m_Icons;

        protected override void Binding(BindingSet<ViewBase<BuildableObjectItemViewModel>, BuildableObjectItemViewModel> bindingSet)
        {
            foreach (var item in m_Icons)
            {
                bindingSet.Bind(item).For(v => v.sprite).To(vm => vm.Icon).OneWay();
            }
            bindingSet.Bind(this).For(v => v.name).To(vm => vm.Title).OneWay();
            bindingSet.Bind(this.m_Button).For(v => v.onClick).To(vm => vm.ClickCommand).CommandParameter(this.ViewModel);
        }
    }
}
