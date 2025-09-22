using Loxodon.Framework.Binding.Builder;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public class LandManagementItemViewModel : ButtonViewModel<StructureMode>
    {
        public override void OnClick(StructureMode item)
        {
            //BindingService.MainGameViewModel.StructureMode = item;
        }
    }
    public class LandManagementItemView : ViewBase<LandManagementItemViewModel>
    {
        [SerializeField] private StructureMode m_BingdingedStructureMode;

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
            this.ViewModel = new LandManagementItemViewModel();
            base.Start();
        }

        protected override void Binding(BindingSet<ViewBase<LandManagementItemViewModel>, LandManagementItemViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_Button).For(v => v.onClick).To(vm => vm.Click).CommandParameter(() => m_BingdingedStructureMode);
        }

        protected override void StaticBinding(BindingSet<ViewBase<LandManagementItemViewModel>> staticBindingSet)
        {
            //staticBindingSet.Bind(this).For(v => v.isCurrentMode).ToExpression(() => BindingService.MainGameViewModel.StructureMode == m_BingdingedStructureMode).OneWay();
        }

    }
}
