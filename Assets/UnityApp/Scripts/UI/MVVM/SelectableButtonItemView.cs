using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    //public class SelectableButtonItemViewModel : SelectableItemViewModel
    //{
    //    public Sprite Icon
    //    {
    //        get { return this.icon; }
    //        set { this.Set(ref icon, value); }
    //    }
    //    private Sprite icon;

    //    public Color NormalColor;
    //    public Color SelectedColor;

    //    public Color CurrentColor
    //    {
    //        get { 
    //            return this.IsSelected ? SelectedColor : NormalColor; 
    //        }
    //    }
    //}

    //public class SelectableButtonItemView<VM> : ViewBase<VM> where VM : SelectableButtonItemViewModel
    //{
    //    [SerializeField] private Color m_NormalColor;
    //    [SerializeField] private Color m_SelectedColor;

    //    [SerializeField] protected Image[] m_Backgrounds;
    //    [SerializeField] protected Image[] m_Icons;

    //    protected override void Binding(BindingSet<ViewBase<VM>, VM> bindingSet)
    //    {
    //        foreach (var item in m_Icons)
    //        {
    //            bindingSet.Bind(item).For(v => v.sprite).To(vm => vm.Icon).OneWay();
    //        }

    //        foreach (var item in m_Backgrounds)
    //        {
    //            bindingSet.Bind(item).For(v => v.color).To(vm => vm.Icon).OneWay();
    //        }
    //    }
    //}
}
