using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Corner : Entity
    {
        private List<Element_Edge> parentEdges = new List<Element_Edge>();
        private Material cornerMaterial;


        private void Start()
        {
            if (Application.isPlaying)
            {
                cornerMaterial = GetComponentInChildren<Renderer>().material;
            }
        }

        public override void UpdateData()
        {
            //var cell = ElementManager_Cell.Instance.GetElement(transform.position);

            //var coord = cell.Data.TileCoord;
            //var coordLeft = new IntVector2(coord.x - 1, coord.y);
            //var coordDown = new IntVector2(coord.x, coord.y - 1);

            //var EdgeData = ElementManager_Edge.Instance;

            //var a = EdgeData.GetVertical(coord);
            //if (a.Data.EdgeType != FlowTilemapEdgeType.Empty)
            //{
            //    parentEdges.Add(a);
            //    a.corners.Add(this);
            //}
            //var b = EdgeData.GetHorizontal(coord);
            //if (b.Data.EdgeType != FlowTilemapEdgeType.Empty)
            //{
            //    parentEdges.Add(b);
            //    b.corners.Add(this);
            //}
            //var c = EdgeData.GetVertical(coordDown);
            //if (c.Data.EdgeType != FlowTilemapEdgeType.Empty)
            //{
            //    parentEdges.Add(c);
            //    c.corners.Add(this);
            //}
            //var d = EdgeData.GetHorizontal(coordLeft);
            //if (d.Data.EdgeType != FlowTilemapEdgeType.Empty)
            //{
            //    parentEdges.Add(d);
            //    d.corners.Add(this);
            //}
            //if (parentEdges.Count < 2)
            //{
            //    Debug.Log("未找到拐角对应的两条边，请检查 Corner 坐标或边数据是否对齐", gameObject);
            //}

            //SetParentCellElement_JustUseThisFunction(cell);
        }

        public void SetWallHide(float value)
        {
            cornerMaterial.SetFloat(Entity_EdgeGroup.CullPlaneHeight, value);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                GizmoUnitily.DrawLine(transform.position + new Vector3(0f, 4f, 0f), parentEdges[0].wall.transform.position, Color.red);
                GizmoUnitily.DrawLine(transform.position + new Vector3(0f, 4f, 0f), parentEdges[1].wall.transform.position, Color.yellow);
            }
        }


    }
}
