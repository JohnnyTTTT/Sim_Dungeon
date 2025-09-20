using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Commands;
using Loxodon.Framework.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public abstract class ButtonViewModel<T> : ViewModelBase
    {
        public ICommand Click
        {
            get { return this.m_Click; }
        }
        private readonly SimpleCommand<T> m_Click;
        public ButtonViewModel()
        {
            this.m_Click = new SimpleCommand<T>(OnClick);
        }
        public abstract void OnClick(T item);
    }
}
