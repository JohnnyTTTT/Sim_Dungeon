using DungeonArchitect.Flow.Domains.Tilemap;
using UnityEngine;
using DungeonArchitect;
using System;
using System.Collections.Generic;
using SoulGames.EasyGridBuilderPro;
using System.Linq;

namespace Johnny.SimDungeon
{
    public class Element_Edge : ElementData<FlowTilemapEdge>
    {
        public Element_Cell[] adjacentCells = new Element_Cell[2];
        public List<Element_Edge> Neighbors;

        public Entity_Wall wall;
        public Entity_Door door;

        public List<Entity_Corner> corners = new List<Entity_Corner>();
        public Vector3 worldPosition;
        public Vector2Int coord;

        public Element_Edge(FlowTilemapEdge data) : base(data)
        {
            worldPosition = CoordUtility.TileCoordToWorldPosition(data.EdgeCoord);
            coord = new Vector2Int(data.EdgeCoord.x, data.EdgeCoord.y);
        }



        public void DrawGizmos()
        {
            if (Data.EdgeType == FlowTilemapEdgeType.Fence || Data.EdgeType == FlowTilemapEdgeType.Wall)
            {
                GizmoUnitily.DrawWall(Data.EdgeCoord, Color.red, Data.HorizontalEdge);
            }
            else
            {
                GizmoUnitily.DrawWall(Data.EdgeCoord, Color.blue, Data.HorizontalEdge);
            }
            if (wall != null)
            {
                GizmoUnitily.DrawLine(CoordUtility.TileCoordToWorldPosition(Data.EdgeCoord),
                    wall.transform.GetChild(0).position + new Vector3(0f, 2f, 0f), Color.yellow);
            }

        }

        public override string ToString()
        {
            return $"<{Data.EdgeCoord.x},{Data.EdgeCoord.y}> , HorizontalEdge : {Data.HorizontalEdge} , Entity : {wall}";
        }

    }
}
