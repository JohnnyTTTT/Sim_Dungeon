using DungeonArchitect;
using EPOOutline;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class Entity : MonoBehaviour
    {

        public Direction Direction;
        private Outlinable m_Outlinable;

        public BuildableObjectSO buildableObjectSO;
        public BuildableObject buildableObject;

        public bool canSelect;
        public bool drawGizmos;

        private void OnEnable()
        {
            //if (DungeonController.Instance.worldDataInited)
            //{
            //    UpdateData();
            //}
        }

        protected virtual void Start()
        {
            buildableObject = GetComponent<BuildableObject>();
            m_Outlinable = GetComponent<Outlinable>();
            ShowOutline(false);
            //            // Render both behind and in front
            //            outlinable.RenderStyle = RenderStyle.FrontBack;

            //            // Render regardless of being obscured
            //            outlinable.RenderStyle = RenderStyle.Single;


            //            Drawing modes:
            //// Normal rendering
            //outlinable.DrawingMode = OutlinableDrawingMode.Normal;

            //            // Z-buffer only
            //            outlinable.DrawingMode = OutlinableDrawingMode.ZOnly;

            //            // Both
            //            outlinable.DrawingMode = OutlinableDrawingMode.Normal | OutlinableDrawingMode.ZOnly;

            //            Configuring outline parameters:
            //            // Outline color
            //            outlinable.OutlineParameters.Color = Color.green;

            //            // Blur shift
            //            outlinable.OutlineParameters.BlurShift = 0.5f;

            //            // Dilate shift
            //            outlinable.OutlineParameters.DilateShift = 0.5f;

            //            // Disable outline
            //            outlinable.OutlineParameters.Enabled = false;

            //            Managing layers and render targets:
            //            // Enable layer 3
            //            outlinable.OutlineLayer = 3;

            //            // Add a renderer
            //            outlinable.OutlineTargets.Add(new OutlineTarget(GetComponent<Renderer>()));

            //            Fill mode settings:
            //            outlinable.OutlineParameters.FillPass.Shader = Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");

            //            // Fill color
            //            outlinable.OutlineParameters.FillPass.SetColor("_PublicColor", Color.yellow);


        }

        private void OnDestroy()
        {
            //DungeonController.Instance.entities.Remove(this);
        }

        public void ShowOutline(bool value)
        {
            if (m_Outlinable != null)
            {
                m_Outlinable.enabled = value;
            }
        }

        public virtual void CreateOrUpdateModel()
        {

        }

        public virtual void UpdateData()
        {
            Direction = DirectionUtility.ToDirection(transform.rotation);
        }

        protected virtual void SetParentCellElement_JustUseThisFunction(Element_LargeCell element)
        {
            //ParentElement = element;
        }

        protected bool TryAddOrUpdateModel(BuildableFreeObjectSO temelpte)
        {
            //var needUpdate = false;
            //if (currentObject == null)
            //{
            //    needUpdate = true;
            //}
            //else if (currentObject.GetBuildableObjectSO() != temelpte)
            //{
            //    needUpdate = true;
            //    if (EasyGridBuilderProController.Instance.TryDestroyBuildableFreeObject(currentObject))
            //    {
            //        currentObject = null;
            //    }
            //}

            //if (needUpdate)
            //{
            //    //if (ParentElement.randomPrefabsIndex == -1)
            //    //{
            //    var randomPrefabsIndex = RandomUtility.UpdateBuildableObjectSORandomPrefab(ParentElement.Data.TileCoord, temelpte);
            //    //}
            //    if (EasyGridBuilderProController.Instance.TryInitializeBuildableFreeObjectSinglePlacement(this, temelpte, randomPrefabsIndex, out var buildableFree))
            //    {
            //        currentObject = buildableFree;
            //        DestroyTelempte();
            //        return true;
            //    }
            //}
            return false;
        }
    }
}
