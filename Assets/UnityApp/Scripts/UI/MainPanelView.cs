using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.ViewModels;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class MainPanelViewModel : ViewModelBase
    {
        public bool LandManagementMode
        {
            get
            {
                return m_LandManagementMode;
            }
            set
            {
                Set(ref m_LandManagementMode, value);
            }
        }
        private bool m_LandManagementMode = true;

        public bool StructureMode
        {
            get
            {
                return m_StructureMode;
            }
            set
            {
                Set(ref m_StructureMode, value);
            }
        }
        private bool m_StructureMode = true;

        public bool PlacementMode
        {
            get
            {
                return m_PlacementMode;
            }
            set
            {
                Set(ref m_PlacementMode, value);
            }
        }
        private bool m_PlacementMode = true;

        public bool DefaultMode
        {
            get
            {
                return m_DefaultMode;
            }
            set
            {
                Set(ref m_DefaultMode, value);
            }
        }
        private bool m_DefaultMode = true;

    }
    public class MainPanelView : ViewBase<MainPanelViewModel>
    {
        protected override void Start()
        {
            ViewModel = BindingService.MainPanelViewModel;
            base.Start();
        }
        protected override void Binding(BindingSet<ViewBase<MainPanelViewModel>, MainPanelViewModel> bindingSet)
        {

        }
    }
}
