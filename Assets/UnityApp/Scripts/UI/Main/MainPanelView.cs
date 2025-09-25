using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Commands;
using Loxodon.Framework.Execution;
using Loxodon.Framework.ViewModels;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Johnny.SimDungeon
{
    public enum GameMode
    {
        Loading,
        God,
        Default,
        Structure,
        Placement,
    }

    public enum StructureMode
    {
        None,
        LandExpand,
    }


    public class MainPanelView : ViewBase<MainGameViewModel>
    {
        [SerializeField] private Toggle m_LandExpandToggle;
        [SerializeField] private Toggle m_DestroyToggle;
        [SerializeField] private CategoryObjectsPanelView m_CategoryObjectsPanelView;
        [SerializeField] private BuildableObjectsPanelView m_BuildableObjectsPanelView;
        private GridManager m_GridManager;
        private bool m_Inited;

        protected override void Start()
        {
            ViewModel = BindingService.MainGameViewModel;
            m_GridManager = GridManager.Instance;
            m_CategoryObjectsPanelView.Init();
            m_BuildableObjectsPanelView.Init();
            Debug.Log("[-----UI-----] : Init CategoryObjects And BuildableObjects");
            m_GridManager.OnActiveGridModeChanged += OnActiveGridModeChanged;

            base.Start();
        }



        protected override void Binding(BindingSet<ViewBase<MainGameViewModel>, MainGameViewModel> bindingSet)
        {
            bindingSet.Bind(this.m_DestroyToggle).For(v => v.isOn, v => v.onValueChanged).To(vm => vm.IsDestroyMode).TwoWay();
            bindingSet.Bind(this.m_LandExpandToggle).For(v => v.isOn, v => v.onValueChanged).To(vm => vm.IsLandExpandMode).TwoWay();
        }

        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {

        }

        //private void OnActiveEasyGridBuilderProChanged(EasyGridBuilderPro activeEasyGridBuilderProSystem)
        //{
        //    BindingService.CategoryObjectsPanelViewModel.ActiveEasyGridBuilderPro = activeEasyGridBuilderProSystem;
        //}

    }
}
