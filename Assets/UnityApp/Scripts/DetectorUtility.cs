using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class DetectorUtility
    {
        public static bool FloodFill(Element_Cell start, out HashSet<Element_Cell> cells)
        {
            var visited = new HashSet<Element_Cell>();
            var queue = new Queue<Element_Cell>();

            var mapSize = DungeonController.Instance.tilemapSize;


            queue.Enqueue(start);
            visited.Add(start);

            var reachedBoundary = false;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var coord = current.Data.TileCoord;
                if (coord.x == 0 || coord.y == 0 || coord.x == mapSize.x - 1 || coord.y == mapSize.y - 1)
                {
                    reachedBoundary = true;
                }

                foreach (var dir in DirectionUtility.CardinalDirections)
                {
                    var neighborCoord = current.Data.TileCoord + dir;
                    var neighbor = ElementManager_Cell.Instance.GetElement(neighborCoord);
                    if (neighbor == null || visited.Contains(neighbor) || neighbor.area != null)
                    {
                        continue;
                    }

                    if (HasWallBetween(current, neighbor, dir))
                    {
                        continue;
                    }

                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            if (reachedBoundary)
            {
                return new HashSet<Element_Cell>();
            }
            return visited;
        }


        public static void HandleWallPlacedIncremental(Entity_Edge entity)
        {
            if (ElementManager_Edge.Instance.CountConnectedEdges(entity.edgeElement) < 2)
            {
                Debug.Log("新墙连接少于2，不形成封闭空间。跳过房间检查。");
                return;
            }

            var cellA = entity.edgeElement.primaryCell;
            var cellB = entity.edgeElement.secondaryCell;

            // ========== 步骤1: 找到所有受影响的 cell ==========
            var affectedCells = new HashSet<Element_Cell>();
            if (cellA != null) affectedCells.Add(cellA);
            if (cellB != null) affectedCells.Add(cellB);

            // 同时把相邻 cell 也加入（确保跨房间的情况能覆盖）
            foreach (var c in affectedCells.ToList())
            {
                foreach (var dir in DirectionUtility.CardinalDirections)
                {
                    var neighbor = ElementManager_Cell.Instance.GetElement(c.Data.TileCoord + dir);
                    if (neighbor != null) affectedCells.Add(neighbor);
                }
            }

            // ========== 步骤2: 移除这些 cell 所属的旧房间 ==========
            var oldRooms = affectedCells
                .Where(c => c.area != null)
                .Select(c => c.area)
                .Distinct()
                .ToList();

            foreach (var oldRoom in oldRooms)
            {
                ElementManager_Room.Instance.DestroyArea(oldRoom);
                Debug.Log($"旧房间 {oldRoom.name} 已被销毁（因新墙影响）。");
            }

            // ========== 步骤3: 用 FloodFill 重新划分 ==========
            var processed = new HashSet<Element_Cell>();

            foreach (var c in affectedCells)
            {
                if (c.area != null || processed.Contains(c)) continue;

                var region = FloodFill(c);
                if (region.Count > 0)
                {
                    var newRoom = ElementManager_Room.Instance.CreateRoom(RoomType.EmptyRoom);
                    newRoom.AddCells(region);
                    processed.UnionWith(region);
                    Debug.Log($"新房间（ID: {newRoom.name}）被创建，包含 {region.Count} 个格子。");
                }
            }
        }

        private static bool HasWallBetween(Element_Cell a, Element_Cell b, IntVector2 dir)
        {
            if (dir == DirectionUtility.UP) return a.upEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
            if (dir == DirectionUtility.DOWN) return a.downEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
            if (dir == DirectionUtility.LEFT) return a.leftEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
            if (dir == DirectionUtility.RIGHT) return a.rightEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
            return false;
        }
    }
}
