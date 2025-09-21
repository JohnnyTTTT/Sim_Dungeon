using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class LandManagementViewModel : ListViewModel<CategoryObjectItemViewModel>
    { 
    
    }

    public class LandManagementView : ViewBase<LandManagementViewModel>
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private Button m_LandExpand;

        protected override void Binding(BindingSet<ViewBase<LandManagementViewModel>, LandManagementViewModel> bindingSet)
        {

        }

        protected override void StaticBinding(BindingSet<ViewBase<LandManagementViewModel>> staticBindingSet)
        {
            staticBindingSet.Bind(this.gameObject).For(v => v.activeSelf).ToExpression(() => BindingService.MainGameViewModel.GameMode == GameMode.Structure);
        }
    }
}
