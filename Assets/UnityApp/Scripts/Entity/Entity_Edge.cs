using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Edge : Entity
    {
        public Entity_SubEdge primary;
        public Entity_SubEdge secondary;
        public Element_Edge edgeElement;

        private void Update()
        {
            //bool hide = false;

            //foreach (var floorPo in floorPos)
            //{
            //    Vector3 camDir = m_MainCam.position - floorPo;
            //    camDir.y = 0; // 只考虑 XZ 平面

            //    Vector3 wallToCam = transform.position - floorPo;
            //    wallToCam.y = 0;

            //    float dot = Vector3.Dot(camDir.normalized, wallToCam.normalized);

            //    if (dot > 0f)
            //    {
            //        hide = true;
            //        break; // 一旦摄像机在正面，整面墙隐藏
            //    }
            //}

            //if (primary.currentObject)
            //{
            //    primary.currentObject.gameObject.SetActive(!hide);
            //}
            //if (secondary.currentObject)
            //{
            //    secondary.currentObject.gameObject.SetActive(!hide);
            //}

            //gameObject.SetActive(!hide);
        }

        public override void UpdateData()
        {
            base.UpdateData();

            var primaryCell = ElementManager_Cell.Instance.GetElement(primary.transform.position);
            var secondaryCell = ElementManager_Cell.Instance.GetElement(secondary.transform.position);

            if (Direction == Direction.Up || Direction == Direction.Down)
            {
                var parentElement = primaryCell.Data.TileCoord.y > secondaryCell.Data.TileCoord.y ? primaryCell : secondaryCell;
                edgeElement = parentElement.downEdge;

            }
            else
            {
                var parentElement = primaryCell.Data.TileCoord.x > secondaryCell.Data.TileCoord.x ? primaryCell : secondaryCell;
                edgeElement = parentElement.leftEdge;
            }

            edgeElement.wall = this;
            edgeElement.primaryCell = primaryCell;
            edgeElement.secondaryCell = secondaryCell;
        }

        public void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                var pPosition = CoordUtility.TileCoordToWorldPosition(edgeElement.primaryCell.Data.TileCoord);
                var sPosition = CoordUtility.TileCoordToWorldPosition(edgeElement.secondaryCell.Data.TileCoord);
                GizmoUnitily.DrawLine(primary.transform.position + new Vector3(0f, 2f, 0f), pPosition, Color.yellow);
                GizmoUnitily.DrawLine(secondary.transform.position + new Vector3(0f, 2f, 0f), sPosition, Color.green);
            }
        }
    }
}
