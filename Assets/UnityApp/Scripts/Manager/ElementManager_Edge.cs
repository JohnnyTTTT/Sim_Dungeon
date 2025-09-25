using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Element_Edge : ElementData<FlowTilemapEdge>
    {
        public Element_LargeCell[] adjacentLargeCells = new Element_LargeCell[2];
        public List<Element_Edge> Neighbors;

        public Element_SmallCell[] containedSmallCells = new Element_SmallCell[3];

        private Entity_Wall m_WallEntity;
        public Entity_Door door;

        public List<Entity_Corner> corners = new List<Entity_Corner>();
        public Vector3 worldPosition;
        public Vector2Int coord;

        public Element_Edge(FlowTilemapEdge data) : base(data)
        {
            coord = new Vector2Int(data.EdgeCoord.x, data.EdgeCoord.y);
            if (data.HorizontalEdge)
            {
                worldPosition = CoordUtility.LargeCellCoordToWorldPosition(coord) + new Vector3(0f, 0f, -1f);
            }
            else
            {
                worldPosition = CoordUtility.LargeCellCoordToWorldPosition(coord) + Vector3.left;
            }
        }

        public void SetWallEntity(Entity_Wall wall)
        {
            m_WallEntity = wall;
            DungeonController.Instance.disablerController_SmallCell.AddDisablerCells(containedSmallCells);
        }

        public Entity_Wall GetWallEntity()
        {
            return m_WallEntity;
        }

        public void DrawGizmos()
        {
            if (Data.EdgeType == FlowTilemapEdgeType.Fence || Data.EdgeType == FlowTilemapEdgeType.Wall)
            {
                GizmoUnitily.DrawWall(worldPosition, Color.red, Data.HorizontalEdge);
            }
            else
            {
                GizmoUnitily.DrawWall(worldPosition, Color.blue, Data.HorizontalEdge);
            }
            //GizmoUnitily.DrawLabel(worldPosition,coord.ToString());
            if (m_WallEntity != null)
            {
                GizmoUnitily.DrawLine(worldPosition, worldPosition + new Vector3(0f, 2f, 0f), Color.yellow);
            }

        }

        public override string ToString()
        {
            return $"<{Data.EdgeCoord.x},{Data.EdgeCoord.y}> , HorizontalEdge : {Data.HorizontalEdge} , Entity : {m_WallEntity}";
        }

    }
    public class ElementManager_Edge : ElementManager
    {
        public static ElementManager_Edge Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_Edge>();
                }
                return s_Instance;
            }

        }
        private static ElementManager_Edge s_Instance;
        public Dictionary<Vector2Int, Element_Edge> horizontalMap = new Dictionary<Vector2Int, Element_Edge>();
        public Dictionary<Vector2Int, Element_Edge> verticalMap = new Dictionary<Vector2Int, Element_Edge>();
        public void Init(FlowTilemapEdgeDatabase edges)
        {
            if (Inited) return;
            horizontalMap.Clear();
            verticalMap.Clear();

            foreach (var edge in edges)
            {
                var data = new Element_Edge(edge);
                if (edge.HorizontalEdge)
                {
                    horizontalMap[edge.EdgeCoord.ToVector2Int()] = data;
                }
                else
                {
                    verticalMap[edge.EdgeCoord.ToVector2Int()] = data;
                }
            }
            Inited = true;
            Debug.Log($"[-----System-----] : DataManager_Edge inited , HorizontalMap count <{horizontalMap.Count}> - VerticalMap <{verticalMap.Count}>");
        }

        public void PostInit()
        {
            foreach (var kvp in horizontalMap)
            {
                var edge = kvp.Value;
                edge.adjacentLargeCells = GetAdjacentCells(edge);
                edge.Neighbors = GetNeighborEdges(edge);

                var midCell = ElementManager_SmallCell.Instance.GetElement(edge.worldPosition);
                if (midCell != null)
                {
                    edge.containedSmallCells[1] = midCell;
                    edge.containedSmallCells[0] = ElementManager_SmallCell.Instance.GetLeftCellFromCoord(midCell.coord);
                    edge.containedSmallCells[2] = ElementManager_SmallCell.Instance.GetRightCellFromCoord(midCell.coord);
                }


            }

            foreach (var kvp in verticalMap)
            {
                var edge = kvp.Value;
                edge.adjacentLargeCells = GetAdjacentCells(edge);
                edge.Neighbors = GetNeighborEdges(edge);

                var midCell = ElementManager_SmallCell.Instance.GetElement(edge.worldPosition);
                if (midCell != null)
                {
                    edge.containedSmallCells[1] = midCell;
                    edge.containedSmallCells[0] = ElementManager_SmallCell.Instance.GetUpCellFromCoord(edge.containedSmallCells[1].coord);
                    edge.containedSmallCells[2] = ElementManager_SmallCell.Instance.GetDownCellFromCoord(edge.containedSmallCells[1].coord);
                }


            }

        }

        public Element_Edge GetHorizontal(Vector2Int coord)
        {
            if (horizontalMap.TryGetValue(coord, out var data))
            {
                return data;
            }
            return null;
        }

        public Element_Edge GetVertical(Vector2Int cooed)
        {
            if (verticalMap.TryGetValue(cooed, out var data))
            {
                return data;
            }
            return null;
        }

        public Element_Edge GetLeftEdgeFromTileCoord(Vector2Int coord)
        {
            return GetVertical(coord);
        }

        public Element_Edge GetUpEdgeFromTileCoord(Vector2Int coord)
        {
            return GetHorizontal(coord + DirectionUtility.UP);
        }

        public Element_Edge GetRightEdgeFromTileCoord(Vector2Int coord)
        {
            return GetVertical(coord + DirectionUtility.RIGHT);
        }

        public Element_Edge GetDownEdgeFromTileCoord(Vector2Int coord)
        {
            return GetHorizontal(coord);
        }

        private List<Element_Edge> GetNeighborEdges(Element_Edge edge)
        {
            var neighborEdges = new List<Element_Edge>();
            var edgeCoord = edge.coord;

            if (edge.Data.HorizontalEdge)
            {
                var leftHorizontal = GetHorizontal(edgeCoord + DirectionUtility.LEFT);
                if (leftHorizontal != null) neighborEdges.Add(leftHorizontal);

                var rightHorizontal = GetHorizontal(edgeCoord + DirectionUtility.RIGHT);
                if (rightHorizontal != null) neighborEdges.Add(rightHorizontal);

                var upLeftVertical = GetVertical(edgeCoord);
                if (upLeftVertical != null) neighborEdges.Add(upLeftVertical);

                var upRightVertical = GetVertical(edgeCoord + DirectionUtility.RIGHT);
                if (upRightVertical != null) neighborEdges.Add(upRightVertical);

                var downLeftVertical = GetVertical(edgeCoord + DirectionUtility.DOWN);
                if (downLeftVertical != null) neighborEdges.Add(downLeftVertical);

                var downRightVertical = GetVertical(edgeCoord + DirectionUtility.DOWN + DirectionUtility.RIGHT);
                if (downRightVertical != null) neighborEdges.Add(downRightVertical);
            }
            else // Vertical Edge
            {
                var upVertical = GetVertical(edgeCoord + DirectionUtility.UP);
                if (upVertical != null) neighborEdges.Add(upVertical);

                var downVertical = GetVertical(edgeCoord + DirectionUtility.DOWN);
                if (downVertical != null) neighborEdges.Add(downVertical);

                var upLeftHorizontal = GetHorizontal(edgeCoord);
                if (upLeftHorizontal != null) neighborEdges.Add(upLeftHorizontal);

                var upRightHorizontal = GetHorizontal(edgeCoord + DirectionUtility.UP);
                if (upRightHorizontal != null) neighborEdges.Add(upRightHorizontal);

                var downLeftHorizontal = GetHorizontal(edgeCoord + DirectionUtility.LEFT);
                if (downLeftHorizontal != null) neighborEdges.Add(downLeftHorizontal);

                var downRightHorizontal = GetHorizontal(edgeCoord + DirectionUtility.LEFT + DirectionUtility.UP);
                if (downRightHorizontal != null) neighborEdges.Add(downRightHorizontal);
            }

            return neighborEdges;
        }

        private Element_LargeCell[] GetAdjacentCells(Element_Edge edge)
        {
            var adjacentCells = new Element_LargeCell[2];
            var edgeCoord = edge.coord;
            if (edge.Data.HorizontalEdge)
            {
                var frontCell = ElementManager_LargeCell.Instance.GetElement(edgeCoord);
                adjacentCells[0] = frontCell;
                var backCell = ElementManager_LargeCell.Instance.GetDownCellFromTileCoord(edgeCoord);
                adjacentCells[1] = backCell;
            }
            else
            {
                var frontCell = ElementManager_LargeCell.Instance.GetLeftCellFromTileCoord(edgeCoord);
                adjacentCells[0] = frontCell;
                var backCell = ElementManager_LargeCell.Instance.GetElement(edgeCoord);
                adjacentCells[1] = backCell;
            }
            return adjacentCells;
        }

        public int CountConnectedEdges(Element_Edge edge)
        {
            var count = 0;
            foreach (var neighbor in edge.Neighbors)
            {
                if (neighbor.Data.EdgeType != FlowTilemapEdgeType.Empty)
                {
                    count++;
                }
            }

            return count;
        }
        public void UnInit()
        {
            horizontalMap.Clear();
            verticalMap.Clear();
            Inited = false;
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in horizontalMap)
                {

                    // if (item.Value.Data.EdgeType == FlowTilemapEdgeType.Wall || item.Value.Data.EdgeType == FlowTilemapEdgeType.Fence)
                    {
                        item.Value.DrawGizmos();
                    }

                }
                foreach (var item in verticalMap)
                {
                    //    if (item.Value.Data.EdgeType == FlowTilemapEdgeType.Wall || item.Value.Data.EdgeType == FlowTilemapEdgeType.Fence)
                    {
                        item.Value.DrawGizmos();
                    }
                }
            }
        }

    }
}
