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


        public static HashSet<Element_Cell> FloodFill(Element_Cell start)
        {
            var visited = new HashSet<Element_Cell>();
            var queue = new Queue<Element_Cell>();

            var size = ElementManager_Cell.Instance.tilemapSize;
            var mapSize = new IntVector2(size, size);


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
                    if (neighbor == null || visited.Contains(neighbor) || neighbor.room != null)
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


            if (cellA.room != null && cellA.room == cellB.room)
            {
                return;
            }


            var newRoomCellsA = FloodFill(cellA);
            if (newRoomCellsA.Count > 0)
            {
                var newRoom = ElementManager_Room.Instance.CreateRoom(RoomType.EmptyRoom);
                newRoom.AddCells(newRoomCellsA);
                Debug.Log($"新房间（ID: {newRoom.name}）被创建，包含 {newRoomCellsA.Count} 个格子。");
            }

            var newRoomCellsB = FloodFill(cellB);
            if (newRoomCellsB.Count > 0 && !newRoomCellsB.Overlaps(newRoomCellsA))
            {
                var newRoom = ElementManager_Room.Instance.CreateRoom(RoomType.EmptyRoom);
                newRoom.AddCells(newRoomCellsB);
                Debug.Log($"新房间（ID: {newRoom.name}）被创建，包含 {newRoomCellsB.Count} 个格子。");
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
