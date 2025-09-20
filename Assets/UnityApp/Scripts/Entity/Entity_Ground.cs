using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Ground : Entity<Data_Cell>
    {
        public override void ApplyBiomeRule()
        {
            var room = DataManager_Room.Instance.GetData(ParentData.Data.TileCoord);
            var groundTemplete = RandomUtility.GetRandomElement(room.biome.grounds);

            TryReplace(groundTemplete);
        }

        public override void UpdateData()
        {
           var data = DataManager_Cell.Instance.GetData(lastCoord);
            SetParentCellData_JustUseThisFunction(data);
        }

        protected override void SetParentCellData_JustUseThisFunction(Data_Cell data)
        {
            base.SetParentCellData_JustUseThisFunction(data);
            data.entity = this;
            name = $"{GetType()} - {data.Data.TileCoord.x},{data.Data.TileCoord.y}";
        }

        public override void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            base.SetTransform(position, rotation, scale);
            if (currentObject != null)
            {
                currentObject.transform.position = position;
                currentObject.transform.rotation = rotation;
            }
        }

    }
}
