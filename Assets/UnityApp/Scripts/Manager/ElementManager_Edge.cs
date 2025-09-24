using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
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
        public Dictionary<IntVector2, Element_Edge> horizontalMap = new Dictionary<IntVector2, Element_Edge>();
        public Dictionary<IntVector2, Element_Edge> verticalMap = new Dictionary<IntVector2, Element_Edge>();
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
                    horizontalMap[edge.EdgeCoord] = data;
                }
                else
                {
                    verticalMap[edge.EdgeCoord] = data;
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
                edge.adjacentCells = GetAdjacentCells(edge);
                edge.Neighbors = GetNeighborEdges(edge);

            }

            foreach (var kvp in verticalMap)
            {
                var edge = kvp.Value;
                edge.adjacentCells = GetAdjacentCells(edge);
                edge.Neighbors = GetNeighborEdges(edge);
            }
        }

        public Element_Edge GetHorizontal(IntVector2 cooed)
        {
            if (horizontalMap.TryGetValue(cooed, out var data))
            {
                return data;
            }
            return null;
        }

        public Element_Edge GetVertical(IntVector2 cooed)
        {
            if (verticalMap.TryGetValue(cooed, out var data))
            {
                return data;
            }
            return null;
        }

        public Element_Edge GetLeftEdgeFromTileCoord(IntVector2 coord)
        {
            return GetVertical(coord);
        }

        public Element_Edge GetUpEdgeFromTileCoord(IntVector2 coord)
        {
            return GetHorizontal(coord + DirectionUtility.UP);
        }

        public Element_Edge GetRightEdgeFromTileCoord(IntVector2 coord)
        {
            return GetVertical(coord + DirectionUtility.RIGHT);
        }

        public Element_Edge GetDownEdgeFromTileCoord(IntVector2 coord)
        {
            return GetHorizontal(coord);
        }

        private List<Element_Edge> GetNeighborEdges(Element_Edge edge)
        {
            var neighborEdges = new List<Element_Edge>();
            var edgeCoord = edge.Data.EdgeCoord;

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

        private Element_Cell[] GetAdjacentCells(Element_Edge edge)
        {
            var adjacentCells = new Element_Cell[2];
            var edgeCoord = edge.Data.EdgeCoord;
            if (edge.Data.HorizontalEdge)
            {
                var frontCell = ElementManager_Cell.Instance.GetElement(edgeCoord);
                adjacentCells[0]=frontCell;
                var backCell = ElementManager_Cell.Instance.GetDownCellFromTileCoord(edgeCoord);
                adjacentCells[1] = backCell;
            }
            else
            {
                var frontCell = ElementManager_Cell.Instance.GetLeftCellFromTileCoord(edgeCoord);
                adjacentCells[0] = frontCell;
                var backCell = ElementManager_Cell.Instance.GetElement(edgeCoord);
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
