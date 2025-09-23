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
        private List<Vector3> adjacentFloorPositions = new List<Vector3>();
        public bool isHidden;
        private Transform m_Camera;
        public float dotT;
        private void Update()
        {
            //if (m_Camera == null)
            //{
            //    m_Camera = CameraController.Instance.MainCamera.transform;
            //}
            //if (edgeElement == null) return;
            //isHidden = false;

            //// 遍历所有相邻格子
            //foreach (var floorPos in adjacentFloorPositions)
            //{
            //    // 摄像机相对于格子方向（XZ 平面）
            //    Vector3 camDir = m_Camera.position - floorPos;
            //    camDir.y = 0;

            //    // 墙中心相对于格子方向
            //    Vector3 wallDir = transform.position - floorPos;
            //    wallDir.y = 0;

            //    // 点乘判断摄像机是否在墙前面
            //    float dot = Vector3.Dot(camDir.normalized, wallDir.normalized);

            //    if (dot > dotT)
            //    {
            //        // 摄像机在墙前面 → 整段墙隐藏
            //        isHidden = true;
            //        break;
            //    }
            //}
            //primary.gameObject.SetActive(isHidden);
            //secondary.gameObject.SetActive(isHidden);
        }

        public override void UpdateData()
        {
            base.UpdateData();

            var primaryCell = ElementManager_Cell.Instance.GetElement(primary.transform.position);
            var secondaryCell = ElementManager_Cell.Instance.GetElement(secondary.transform.position);

            var primaryCellPosition = CoordUtility.TileCoordToWorldPosition(primaryCell.Data.TileCoord);
            var secondaryCellPosition = CoordUtility.TileCoordToWorldPosition(secondaryCell.Data.TileCoord);
            adjacentFloorPositions.Add(primaryCellPosition);
            adjacentFloorPositions.Add(secondaryCellPosition);

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
