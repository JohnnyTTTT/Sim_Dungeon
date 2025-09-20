using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class Entity : MonoBehaviour
    {
        public IntVector2 lastCoord;
        public bool drawGizmos;
        public GameObject telempte;
        protected BuildableFreeObject currentObject;
        public virtual void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
        public virtual void ApplyBiomeRule()
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

        protected virtual bool TryReplace(BuildableFreeObjectSO temelpte)
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
                if (EasyGridBuilderProController.Instance.TryInitializeBuildableFreeObjectSinglePlacement(this, temelpte, out var buildableFree))
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
            Debug.Log(currentObject, currentObject);
            if (currentObject == null)
            {
                return true;
            }
            if (EasyGridBuilderProController.Instance.TryDestroyBuildableFreeObject(currentObject))
            {
                Debug.Log(currentObject, currentObject);
                return true;
            }
            return false;
        }


    }

    public abstract class Entity<T> : Entity where T : ElementData
    {
        public T ParentData { get; private set; }
        protected virtual void SetParentCellData_JustUseThisFunction(T data)
        {
            ParentData = data;
        }


    }

}
