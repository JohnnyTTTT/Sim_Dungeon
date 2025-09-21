using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using Loxodon.Framework.Views;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class ViewBase<VM> : UIView where VM : ViewModelBase
    {
        public VM ViewModel
        {
            get
            {
                return (VM)this.GetDataContext();
            }
            set { this.SetDataContext(value); }
        }

        protected override void Start()
        {
            Binding();
            StaticBinding();
        }

        private void Binding()
        {
            var bindingSet = this.CreateBindingSet<ViewBase<VM>, VM>();
            Binding(bindingSet);
            bindingSet.Build();
        }

        private void StaticBinding()
        {
            var staticBindingSet = this.CreateBindingSet();
            StaticBinding(staticBindingSet);
            staticBindingSet.Build();
        }

        protected virtual void StaticBinding(BindingSet<ViewBase<VM>> staticBindingSet)
        {
        }

        protected abstract void Binding(BindingSet<ViewBase<VM>, VM> bindingSet);
    }
}
