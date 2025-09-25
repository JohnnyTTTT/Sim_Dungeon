using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class Element_LargeCell : ElementData<FlowTilemapCell>
    {
        public Element_LargeCell[] neighbors = new Element_LargeCell[4];
        public Element_SmallCell[] containedSmallCells = new Element_SmallCell[9];
        public Element_Edge[] edges = new Element_Edge[4];
        public Region region;
        private Entity_Floor m_Ground;
        public Entity_Ceiling ceiling;

        public Element_Edge horizontalEdge;
        public Element_Edge verticalEdge;

        public Entity_SubEdge horizontalSubEdge;
        public Entity_SubEdge verticalSubEdge;

        public Vector3 worldPosition;
        public Vector2Int coord;


        public Element_LargeCell(FlowTilemapCell data) : base(data)
        {
            coord = new Vector2Int(data.TileCoord.x, data.TileCoord.y);
            worldPosition = CoordUtility.LargeCoordToWorldPosition(coord);
        }

        public void SetGroundEntity(Entity_Floor ground)
        {
            m_Ground = ground;
            //var valid = 
            //for (int i = 0; i < length; i++)
            //{

            //}
            //DungeonController.Instance.disablerController_SmallCell.RemoveDisablerCells(containedSmallCells);
            //foreach (var item in containedSmallCells)
            //{
            //    item.isBuildingValid = true;
            //}
        }

        public Entity_Floor GetGroundEntity()
        {
            return m_Ground;
        }

        public void DrawGizmos()
        {
            GizmoUnitily.DrawTwoSizeCube(worldPosition, Color.green, true);
            GizmoUnitily.DrawLabel(coord, $"{new Vector2Int(Data.TileCoord.x, Data.TileCoord.y)} - {Data.CellType} - {region}");
        }

        public override string ToString()
        {
            return $"<{Data.TileCoord.x},{Data.TileCoord.y}> - {Data.CellType}";
        }

    }

    public class ElementManager_LargeCell : ElementManager<Element_LargeCell>
    {
        public static ElementManager_LargeCell Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_LargeCell>();
                }
                return s_Instance;
            }

        }
        private static ElementManager_LargeCell s_Instance;

        public Vector2Int drawGizmosCoord;



        public void Init(FlowTilemapCellDatabase cells)
        {
            map.Clear();

            foreach (var cell in cells)
            {
                var element = new Element_LargeCell(cell);
                map[cell.TileCoord.ToVector2Int()] = element;
            }

            Debug.Log($"[-----System-----] : DataManager Cell inited , Cell count <{map.Count}>");
        }

        public void PostInit()
        {
            var edgeManager = ElementManager_Edge.Instance;
            foreach (var element in map.Values)
            {
                var coord = element.coord;

                element.edges[0] = edgeManager.GetLeftEdgeFromTileCoord(coord);
                element.edges[1] = edgeManager.GetUpEdgeFromTileCoord(coord);
                element.edges[2] = edgeManager.GetRightEdgeFromTileCoord(coord);
                element.edges[3] = edgeManager.GetDownEdgeFromTileCoord(coord);

                element.neighbors[0] = GetLeftCellFromTileCoord(coord);
                element.neighbors[1] = GetUpCellFromTileCoord(coord);
                element.neighbors[2] = GetRightCellFromTileCoord(coord);
                element.neighbors[3] = GetDownCellFromTileCoord(coord);

                var startPosition = element.worldPosition + new Vector3(-0.5f, -0.5f);
                var startcoord = CoordUtility.WorldPositionToSmallCoord(startPosition);

                var s0 = ElementManager_SmallCell.Instance.GetElement(startcoord);
                if (s0 != null) element.containedSmallCells[0] = s0;

                var s1 = ElementManager_SmallCell.Instance.GetUpCellFromCoord(startcoord);
                if (s1 != null) element.containedSmallCells[1] = s1;

                var s2 = ElementManager_SmallCell.Instance.GetRightCellFromCoord(startcoord);
                if (s2 != null) element.containedSmallCells[2] = s2;

                var s3 = ElementManager_SmallCell.Instance.GetElement(startcoord + new Vector2Int(1, 1));
                if (s3 != null) element.containedSmallCells[3] = s3;

            }
        }

        public void UnInit()
        {
            //map.Clear();
        }

        public Element_LargeCell GetLeftCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.LEFT);
        }

        public Element_LargeCell GetUpCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.UP);
        }

        public Element_LargeCell GetRightCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.RIGHT);
        }

        public Element_LargeCell GetDownCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.DOWN);
        }


        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in map)
                {
                    if ( item.Value.Data.CellType == FlowTilemapCellType.Floor)
                    {
                        item.Value.DrawGizmos();
                    }
                }
            }
        }


    }
}