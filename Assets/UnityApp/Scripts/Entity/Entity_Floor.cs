using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Floor : Entity
    {
        public Element_LargeCell cellElement;
        public override void UpdateData()
        {
            base.UpdateData();
            cellElement = ElementManager_LargeCell.Instance.GetElement(transform.position);
            cellElement.SetGroundEntity(this);
        }

    }
}
