using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Commands;
using Loxodon.Framework.ViewModels;
using Loxodon.Framework.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class MainButtonViewModel : ButtonViewModel<GameMode>
    {
        public override void OnClick(GameMode gameMode)
        {
            BindingService.MainGameViewModel.GameMode = gameMode;
        }
    }

    public class MainButtonView : ViewBase<MainButtonViewModel>
    {
        [SerializeField] private GameMode m_BingdingedGameMode;
        [SerializeField] private Color m_NormalColor;
        [SerializeField] private Color m_SelectedColor;

        [SerializeField] private Button m_Button;
        [SerializeField] private Image[] m_Images;

        public bool isCurrentMode
        {
            get
            {
                return m_IsCurrentMode;
            }
            set
            {
                if (m_IsCurrentMode != value)
                {
                    m_IsCurrentMode = value;
                    var color = m_IsCurrentMode ? m_SelectedColor : m_NormalColor;
                    foreach (var item in m_Images)
                    {
                        item.color = color;
                    }
                }
            }
        }
        private bool m_IsCurrentMode;

        protected override void Start()
        {
            this.ViewModel = new MainButtonViewModel();
            base.Start();
        }

        protected override void Binding(BindingSet<ViewBase<MainButtonViewModel>, MainButtonViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_Button).For(v => v.onClick).To(vm => vm.Click).CommandParameter(() => m_BingdingedGameMode);
        }

        protected override void StaticBinding(BindingSet<ViewBase<MainButtonViewModel>> staticBindingSet)
        {
            staticBindingSet.Bind(this).For(v => v.isCurrentMode).ToExpression(() => BindingService.MainGameViewModel.GameMode == m_BingdingedGameMode).OneWay();
        }
    }
}
