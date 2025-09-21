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
        None,
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

        [SerializeField] private CategoryObjectsPanelView m_CategoryObjectsPanelView;
        [SerializeField] private BuildableObjectsPanelView m_BuildableObjectsPanelView;


        protected override void Start()
        {
            ViewModel = BindingService.MainGameViewModel;
            //GridManager.Instance.OnActiveEasyGridBuilderProChanged += OnActiveEasyGridBuilderProChanged;
            GridManager.Instance.OnActiveGridModeChanged += OnActiveGridModeChanged;

            base.Start();
            StartCoroutine(PostStart());
        }

        private IEnumerator PostStart()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            m_CategoryObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize1);
            m_CategoryObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2);

            m_BuildableObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize1);
            m_BuildableObjectsPanelView.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2);

            Debug.Log("[-----UI-----] : Init CategoryObjects And BuildableObjects");
        }

        protected override void Binding(BindingSet<ViewBase<MainGameViewModel>, MainGameViewModel> bindingSet)
        {

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
