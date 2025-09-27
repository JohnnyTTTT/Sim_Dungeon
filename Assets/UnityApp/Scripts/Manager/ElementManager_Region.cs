using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum RoomType
    {
        Undefined,
        OriginaCave,
        EmptyRoom,
        Tavern,
        Hotel,
        HotelRoom,
    }

    public class ElementManager_Region : MonoBehaviour
    {
        public static ElementManager_Region Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_Region>();
                }
                return s_Instance;
            }
        }
        private static ElementManager_Region s_Instance;

        private static int s_RegionID;

        public List<Region> regionList = new List<Region>();

        [ShowInInspector]
        public int RegionCount
        {
            get
            {
                return regionList.Count;
            }
        }

        public bool drawGizmos;

        public Region CreateRegion(RoomType roomType)
        {
            var region = new Region();
            region.Init($"{roomType} - {s_RegionID}", roomType);
            regionList.Add(region);
            s_RegionID++;
            return region;
        }

        public void RemoveRegion(Region region)
        {
            regionList.Remove(region);
        }

        public void DestroyRegion(Region room)
        {
            room.Clear();
            regionList.Remove(room);
        }

        public void Init()
        {
            regionList.Clear();

        }

        public void PostInit()
        {
            foreach (var cell in ElementManager_LargeCell.Instance.GetAllElements())
            {
                if (cell.Data.CellType != FlowTilemapCellType.Floor) continue;
                if (cell.region != null) continue;

                var regionCells = FloodFill(cell);
                if (regionCells != null && regionCells.Count > 0)
                {
                    var newRegion = CreateRegion(RoomType.EmptyRoom);
                    foreach (var item in regionCells)
                    {
                        newRegion.AddLargeCell(item);
                    }
                    newRegion.CalculateBounds();
                    //Debug.Log($"新区域（ID: {newRegion.name}）被创建，包含 {regionCells.Count} 个格子。区域总数 {Instance.regionList.Count}");
                }
            }

            foreach (var region in regionList)
            {
                CollectSmallCells(region);
            }
            //foreach (var item in ElementManager_SmallCell.Instance.GetAllElements())
            //{
            //    if (item.cellType ==  FlowTilemapSmallCellType.Floor) continue;
            //    if (item.region != null) continue;
            //    var regionCells = FloodFill(cell);
            //}


            foreach (var item in regionList)
            {

            }
        }

        public void UnInit()
        {
            regionList.Clear();
        }

        private void OnDestroy()
        {
            regionList.Clear();
        }


        public void CalculateExist(Region region)
        {
            if (region.containedLargeCells == null || region.containedLargeCells.Count == 0)
            {
                RemoveRegion(region);
                Debug.Log($"区域（ID: {name}）被移除。区域总数 {ElementManager_Region.Instance.regionList.Count}");
            }
        }

        private void CollectSmallCells(Region region)
        {
            var position = region.containedLargeCells.First().worldPosition;
            var firstSmall = ElementManager_SmallCell.Instance.GetElement(position);
            var regionCells = FloodFill(firstSmall);
            if (regionCells != null && regionCells.Count > 0)
            {
                foreach (var cell in regionCells)
                {
                    var oldRegion = cell.region;
                    if (oldRegion != null)
                    {
                        oldRegion.RemoveSmallCell(cell);
                    }
                    region.AddSamllCell(cell);
                    cell.cellType = FlowTilemapSmallCellType.Floor;
                }
            }
        }

        public HashSet<Element_SmallCell> FloodFill(Element_SmallCell start, int maxRange = 300)
        {
            var visited = new HashSet<Element_SmallCell>();
            var queue = new Queue<Element_SmallCell>();
            var mapSize = DungeonController.Instance.smallTilemapSize;

            var origin = start.coord;
            int halfRange = maxRange / 2;

            queue.Enqueue(start);
            visited.Add(start);

            var reachedBoundary = false;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var coord = current.coord;

                if (coord.x == 0 || coord.y == 0 || coord.x == mapSize.x - 1 || coord.y == mapSize.y - 1)
                {
                    reachedBoundary = true;
                }

                for (int i = 0; i < 4; i++)
                {
                    var neighbor = current.neighbors[i];

                    if (neighbor == null || visited.Contains(neighbor)) continue;
                    if (neighbor.cellType == FlowTilemapSmallCellType.Wall) continue;

                    var nCoord = neighbor.coord;
                    if (Mathf.Abs(nCoord.x - origin.x) > halfRange ||
                        Mathf.Abs(nCoord.y - origin.y) > halfRange)
                    {
                        Debug.Log($"FloodFill SmallCell , 超出范围，跳过");
                        continue; // 超出范围，跳过
                    }

                    // 不跳过旧房间格子
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            // 到边界的区域依然返回空
            if (reachedBoundary) return new HashSet<Element_SmallCell>();

            return visited;
        }

        public HashSet<Element_LargeCell> FloodFill(Element_LargeCell start, int maxRange = 150)
        {
            var visited = new HashSet<Element_LargeCell>();
            var queue = new Queue<Element_LargeCell>();
            var mapSize = DungeonController.Instance.smallTilemapSize;

            var origin = start.coord;
            int halfRange = maxRange / 2;

            queue.Enqueue(start);
            visited.Add(start);

            var reachedBoundary = false;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var coord = current.coord;

                if (coord.x == 0 || coord.y == 0 || coord.x == mapSize.x - 1 || coord.y == mapSize.y - 1)
                {
                    reachedBoundary = true;
                }

                for (int i = 0; i < 4; i++)
                {
                    var neighbor = current.neighbors[i];
                    var dir = DirectionUtility.CardinalDirections[i];

                    if (neighbor == null || visited.Contains(neighbor)) continue;
                    if (DirectionUtility.HasEdgeBetween(current, neighbor, dir)) continue;

                    var nCoord = neighbor.coord;
                    if (Mathf.Abs(nCoord.x - origin.x) > halfRange ||
                        Mathf.Abs(nCoord.y - origin.y) > halfRange)
                    {
                        Debug.Log($"FloodFill LargeCell , 超出范围，跳过");
                        continue; // 超出范围，跳过
                    }

                    // 不跳过旧房间格子
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            // 到边界的区域依然返回空
            if (reachedBoundary) return new HashSet<Element_LargeCell>();

            return visited;
        }

        public void HandleWallsPlacedIncremental(HashSet<Element_LargeCell> allAdjacentCells)
        {
            // 已访问的格子集合，避免重复计算
            var visited = new HashSet<Element_LargeCell>();

            foreach (var cell in allAdjacentCells)
            {
                if (visited.Contains(cell)) continue;

                // 对每个未访问的格子执行 FloodFill
                var regionCells = FloodFill(cell, 150); // 或传入 maxRange
                if (regionCells.Count == 0) continue; // 遇到边界，忽略

                // 标记这些格子已访问
                foreach (var c in regionCells)
                    visited.Add(c);

                var newRegion = CreateRegion(RoomType.EmptyRoom);
                foreach (var c in regionCells)
                {
                    var oldRegion = c.region;
                    if (oldRegion != null)
                    {
                        oldRegion.RemoveLargeCell(c);
                        CalculateExist(oldRegion);
                    }
                    newRegion.AddLargeCell(c);
                    newRegion.CalculateBounds();
                }
                CollectSmallCells(newRegion);
                Debug.Log($"新区域（ID: {newRegion.name}）被创建，包含 {regionCells.Count} 个格子。区域总数 {Instance.regionList.Count}");
            }
        }

        //public void HandleWallPlacedIncremental(Entity_Wall entity)
        //{
        //    if (DirectionUtility.GetEdgeConnectedEdgesCount(entity.edgeElement) < 2)
        //    {
        //        //Debug.Log("新墙连接少于2，不形成封闭空间。跳过房间检查。");
        //        return;
        //    }

        //    var cellA = entity.edgeElement.adjacentLargeCells[0];
        //    var cellB = entity.edgeElement.adjacentLargeCells[1];

        //    if (cellA == null || cellB == null) return;

        //    // ========== 步骤1: 收集受影响 cell ==========
        //    var affectedCells = new HashSet<Element_LargeCell> { cellA, cellB };

        //    // 收集受影响的旧房间 cell（邻居属于旧房间）
        //    foreach (var c in affectedCells.ToList())
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            var neighbor = c.neighbors[i];
        //            if (neighbor != null)
        //                affectedCells.Add(neighbor);
        //        }
        //    }

        //    // ========== 步骤2: FloodFill 生成区域 ==========
        //    var processed = new HashSet<Element_LargeCell>();
        //    foreach (var c in affectedCells)
        //    {
        //        if (processed.Contains(c)) continue;

        //        var newRegionCells = FloodFill(c);
        //        if (newRegionCells.Count == 0) continue; // 无效区域，跳过

        //        // 从旧房间中移除这些 cell
        //        foreach (var cell in newRegionCells)
        //        {
        //            if (cell.region != null)
        //            {
        //                cell.region.RemoveLargeCell(cell);
        //            }
        //        }

        //        // 创建新房间
        //        var newRegion = CreateRegion(RoomType.EmptyRoom);
        //        newRegion.AddLargeCells(newRegionCells);
        //        Debug.Log($"新区域（ID: {newRegion.name}）被创建，包含 {newRegionCells.Count} 个格子。区域总数 {Instance.regionList.Count}");

        //        processed.UnionWith(newRegionCells);
        //    }

        //}



        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in regionList)
                {
                    item.DrawGizmos();
                }
            }

        }


    }
}
