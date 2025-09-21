using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Ground : Entity
    {
        public override void UpdateData()
        {
            var cellElement = ElementManager_Cell.Instance.GetElement(transform.position);
            transform.rotation = RandomUtility.GetRandomDirection(cellElement.Data.TileCoord);
            SetParentCellElement_JustUseThisFunction(cellElement);
        }

        public override void CreateOrUpdateModel()
        {
            var room = ParentElement.room;
            BuildableFreeObjectSO groundTemplete = null;
            if (room == null)
            {
                groundTemplete = SpawnManager.Instance.defaultGround;
            }
            TryAddOrUpdateModel(groundTemplete);
        }

        protected override void SetParentCellElement_JustUseThisFunction(Element_Cell element)
        {
            base.SetParentCellElement_JustUseThisFunction(element);
            ParentElement.ground = this;
            name = $"Ground - {element.Data.TileCoord.x},{element.Data.TileCoord.y}";
        }

    }
}
