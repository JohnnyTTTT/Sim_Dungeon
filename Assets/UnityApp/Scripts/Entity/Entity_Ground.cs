using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Ground : Entity
    {
        public Element_LargeCell cellElement;
        public override void UpdateData()
        {
            base.UpdateData();
            cellElement = ElementManager_LargeCell.Instance.GetElement(transform.position);
            cellElement.ground = this;
        }

    }
}
