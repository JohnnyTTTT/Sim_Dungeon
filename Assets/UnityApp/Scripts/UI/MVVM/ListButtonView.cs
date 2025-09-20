using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class ListButtonView<VM> : ViewBase<VM> where VM : SelectableItemViewModel
    {
        [SerializeField] private Image[] m_Images;
        [SerializeField] private Color m_NormalColor;
        [SerializeField] private Color m_SelectedColor;

        public bool IsSelected
        {
            get
            {
                return m_IsSelected;
            }
            set
            {
                if (m_IsSelected != value)
                {
                    m_IsSelected = value;
                    var color = m_IsSelected ? m_SelectedColor : m_NormalColor;
                    foreach (var item in m_Images)
                    {
                        item.color = color;
                    }
                }
            }
        }
        private bool m_IsSelected;

        protected override void Binding(BindingSet<ViewBase<VM>, VM> bindingSet)
        {
            bindingSet.Bind(this).For(v => v.IsSelected).To(vm => vm.IsSelected).OneWay();
        }
    }
}
