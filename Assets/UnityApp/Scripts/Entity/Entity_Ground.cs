using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Ground : Entity
    {
        private Data_Cell m_data;
        private BuildableGridObject currentObject;

        public override bool TryReplace(BuildableObjectSO temelpte, BuildableObjectSO.RandomPrefabs prefabs = null)
        {
            if (temelpte is BuildableGridObjectSO buildableFreeObject)
            {
                if (currentObject == null || currentObject.GetBuildableObjectSO() != buildableFreeObject)
                {
                    if (EasyGridBuilderProController.Instance.ReplaceGround(this, buildableFreeObject, out var buildable))
                    {
                        currentObject = buildable;
                        return true;
                    }
                }
            }
            return false;
        }
        public override void UpdateData()
        {
            m_data = DataManager_Cell.Instance.GetData(lastCoord);
            m_data.entity = this;
        }
    }
}
