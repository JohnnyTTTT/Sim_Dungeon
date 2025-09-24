using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Edge : Entity
    {
        public Element_Edge edgeElement;



        public override void UpdateData()
        {
            base.UpdateData();

            var front = transform.position + transform.right + transform.forward;
            var back = transform.position + transform.right - transform.forward;

            var frontCell = ElementManager_Cell.Instance.GetElement(front);
            var backCell = ElementManager_Cell.Instance.GetElement(back);

            if (Direction == Direction.Up || Direction == Direction.Down)
            {
                var parentElement = front.z > back.z ? frontCell : backCell;
                edgeElement = parentElement.downEdge;
            }
            else
            {
                var parentElement = front.x > back.x ? frontCell : backCell;
                edgeElement = parentElement.leftEdge;
            }
        }
    }
}
