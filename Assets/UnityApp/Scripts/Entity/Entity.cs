using DungeonArchitect;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class Entity : MonoBehaviour
    {
        public bool drawGizmos;
        public Direction Direction;

        public BuildableObjectSO buildableObjectSO;

        private void OnEnable()
        {
            //if (DungeonController.Instance.worldDataInited)
            //{
            //    UpdateData();
            //}
        }

        protected virtual void Start()
        {
            //if (DungeonController.Instance.worldDataInited)
            //{
            //    CreateOrUpdateModel();
            //}
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
            Direction = DirectionUtility.GetDirectionForWorld(transform.rotation);
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
