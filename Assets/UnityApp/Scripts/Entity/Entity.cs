using DungeonArchitect;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class Entity : MonoBehaviour
    {
        [ShowInInspector] [ReadOnly] public Element_Cell ParentElement { get; private set; }
        public bool drawGizmos;
        public GameObject telempte;
        public BuildableFreeObject currentObject;
        protected FourDirectionalRotation Direction;

        private void OnEnable()
        {
            if (DungeonController.Instance.worldDataInited)
            {
                UpdateData();
            }
        }

        protected virtual void Start()
        {
            if (DungeonController.Instance.worldDataInited)
            {
                CreateOrUpdateModel();
            }
        }

        private void OnDestroy()
        {
            //DungeonController.Instance.entities.Remove(this);
        }

        public virtual void CreateOrUpdateModel()
        {

        }

        public virtual void UpdateData()
        {

        }

        public void DestroyTelempte()
        {
            if (telempte != null)
            {
                Destroy(telempte);
            }
        }

        protected virtual void SetParentCellElement_JustUseThisFunction(Element_Cell element)
        {
            ParentElement = element;
        }

        protected bool TryAddOrUpdateModel(BuildableFreeObjectSO temelpte)
        {
            var needUpdate = false;
            if (currentObject == null)
            {
                needUpdate = true;
            }
            else if (currentObject.GetBuildableObjectSO() != temelpte)
            {
                needUpdate = true;
                if (EasyGridBuilderProController.Instance.TryDestroyBuildableFreeObject(currentObject))
                {
                    currentObject = null;
                }
            }

            if (needUpdate)
            {
                //if (ParentElement.randomPrefabsIndex == -1)
                //{
                var randomPrefabsIndex = RandomUtility.UpdateBuildableObjectSORandomPrefab(ParentElement.Data.TileCoord, temelpte);
                //}
                if (EasyGridBuilderProController.Instance.TryInitializeBuildableFreeObjectSinglePlacement(this, temelpte, randomPrefabsIndex, out var buildableFree))
                {
                    currentObject = buildableFree;
                    DestroyTelempte();
                    return true;
                }
            }
            return false;
        }

        public virtual bool TryDestroy()
        {
            if (currentObject == null)
            {
                return true;
            }
            if (EasyGridBuilderProController.Instance.TryDestroyBuildableFreeObject(currentObject))
            {
                return true;
            }
            return false;
        }
    }

}
