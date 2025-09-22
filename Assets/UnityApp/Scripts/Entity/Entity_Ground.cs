using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Ground : Entity
    {
        public Element_Cell cellElement;
        public override void UpdateData()
        {
            cellElement = ElementManager_Cell.Instance.GetElement(transform.position);
            transform.rotation = RandomUtility.GetRandomDirection(cellElement.Data.TileCoord);
            cellElement.ground = this;
        }

    }
}
