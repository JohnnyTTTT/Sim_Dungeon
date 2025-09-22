using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Door : Entity
    {
        public override void UpdateData()
        {
            var parentElement = ElementManager_Cell.Instance.GetElement(transform.position);

            Element_Edge edgeElement;
            Direction = DirectionUtility.GetDirectionForWorld(transform.rotation);
            if (Direction == Direction.Up || Direction == Direction.Down)
            {
                edgeElement = parentElement.horizontalEdge;
            }
            else
            {
                edgeElement = parentElement.verticalEdge;
            }
            edgeElement.door = this;
            SetParentCellElement_JustUseThisFunction(parentElement);
        }

        protected override void SetParentCellElement_JustUseThisFunction(Element_Cell element)
        {
            base.SetParentCellElement_JustUseThisFunction(element);
            name = $"Door - {element.Data.TileCoord.x},{element.Data.TileCoord.y} - {Direction}";
        }


    }
}
