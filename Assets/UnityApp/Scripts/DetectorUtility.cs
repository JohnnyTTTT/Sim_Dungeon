//using DungeonArchitect;
//using DungeonArchitect.Flow.Domains.Tilemap;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace Johnny.SimDungeon
//{
//    public class DetectorUtility
//    {
//        // 洪水填充算法：可以生成新房间，也可以拆分旧房间
//        public static HashSet<Element_Cell> FloodFill(Element_Cell start)
//        {
//            var visited = new HashSet<Element_Cell>();
//            var queue = new Queue<Element_Cell>();
//            var mapSize = DungeonController.Instance.tilemapSize;

//            queue.Enqueue(start);
//            visited.Add(start);

//            var reachedBoundary = false;

//            while (queue.Count > 0)
//            {
//                var current = queue.Dequeue();
//                var coord = current.Data.TileCoord;

//                // 检查是否到达地图边界
//                if (coord.x == 0 || coord.y == 0 || coord.x == mapSize.x - 1 || coord.y == mapSize.y - 1)
//                {
//                    reachedBoundary = true;
//                }

//                for (int i = 0; i < 4; i++)
//                {
//                    var neighbor = current.neighbors[i];
//                    var dir = DirectionUtility.CardinalDirections[i];

//                    if (neighbor == null || visited.Contains(neighbor)) continue;
//                    if (HasWallBetween(current, neighbor, dir)) continue;

//                    visited.Add(neighbor);
//                    queue.Enqueue(neighbor);
//                }
//            }

//            // 如果到达边界，说明区域未封闭
//            if (reachedBoundary)
//            {
//                return new HashSet<Element_Cell>();
//            }

//            return visited;
//        }


//        // 放置新墙时触发的增量式房间检测（支持拆分旧房间）
//        public static void HandleWallPlacedIncremental(Entity_Edge entity)
//        {
//            if (ElementManager_Edge.Instance.CountConnectedEdges(entity.edgeElement) < 2)
//            {
//                Debug.Log("新墙连接少于2，不形成封闭空间。跳过房间检查。");
//                return;
//            }

//            var cellA = entity.edgeElement.primaryCell;
//            var cellB = entity.edgeElement.secondaryCell;

//            if (cellA == null || cellB == null) return;

//            // ========== 步骤1: 收集受影响 cell ==========
//            var affectedCells = new HashSet<Element_Cell> { cellA, cellB };

//            // 收集受影响的旧房间 cell（邻居属于旧房间）
//            foreach (var c in affectedCells.ToList())
//            {
//                for (int i = 0; i < 4; i++)
//                {
//                    var neighbor = c.neighbors[i];
//                    if (neighbor != null)
//                        affectedCells.Add(neighbor);
//                }
//            }

//            // ========== 步骤2: FloodFill 生成区域 ==========
//            var processed = new HashSet<Element_Cell>();
//            foreach (var c in affectedCells)
//            {
//                if (processed.Contains(c)) continue;

//                var newRegionCells = DetectorUtility.FloodFill(c);
//                if (newRegionCells.Count == 0) continue; // 无效区域，跳过

//                // 从旧房间中移除这些 cell
//                foreach (var cell in newRegionCells)
//                {
//                    if (cell.region != null)
//                    {
//                        cell.region.RemoveCell(cell);
//                    }
//                }

//                // 创建新房间
//                var newRoom = ElementManager_Region.Instance.CreateRegion(RoomType.EmptyRoom);
//                newRoom.AddCells(newRegionCells);
//                //Debug.Log($"新房间（ID: {newRoom.name}）被创建，包含 {newRegionCells.Count} 个格子。房间总数 {ElementManager_Region.Instance.regionList.Count}");

//                processed.UnionWith(newRegionCells);
//            }

//        }

//        // 辅助方法：检查两个相邻单元格之间是否有墙
//        private static bool HasWallBetween(Element_Cell a, Element_Cell b, IntVector2 dir)
//        {
//            if (dir == DirectionUtility.UP) return a.upEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
//            if (dir == DirectionUtility.DOWN) return a.downEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
//            if (dir == DirectionUtility.LEFT) return a.leftEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
//            if (dir == DirectionUtility.RIGHT) return a.rightEdge.Data.EdgeType > FlowTilemapEdgeType.Empty;
//            return false;
//        }
//    }
//}
