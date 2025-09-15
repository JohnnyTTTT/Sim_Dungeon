using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Johnny.SimDungeon;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class BuildingItemSpawnListener : DungeonItemSpawnListener
    {
        private static Vector3 dirUp = Vector3.forward;    // 世界前
        private static Vector3 dirDown = Vector3.back;     // 世界后
        private static Vector3 dirRight = Vector3.right;   // 世界右
        private static Vector3 dirLeft = Vector3.left;     // 世界左
        private void Start()
        {
            //cells.Clear();
        }

        //private CellEntity TryGetValue(FlowTilemapCell cell)
        //{
        //    var entitly = CellEntitiyManager.Instance.GetCellEntitly(cell);

        //    entity.transform.position = m_DungeonController.gridFlowDungeonQuery.TileCoordToWorldCoord(cell.TileCoord);
        //    entity.transform.parent = m_CellEntityParent;
        //    info = entity.AddComponent<CellEntity>();
        //    var nodeCoord = new Vector2Int(cell.NodeCoord.x, cell.NodeCoord.y);
        //    //info.Init(tileCoord);
        //    //info = new SimDungeonCellInfo();
        //    cells[cell] = info;

        //    return info;
        //}

        private Direction GetDirectionForWorld(Quaternion rotation)
        {
            var forward = rotation * Vector3.forward;

            forward.y = 0;
            forward.Normalize();

            // 定义四个方向向量


            // 计算点积
            float dotUp = Vector3.Dot(forward, dirUp);
            float dotDown = Vector3.Dot(forward, dirDown);
            float dotRight = Vector3.Dot(forward, dirRight);
            float dotLeft = Vector3.Dot(forward, dirLeft);

            // 找最大值对应的方向
            float maxDot = Mathf.Max(dotUp, dotDown, dotRight, dotLeft);

            if (maxDot == dotUp) return Direction.Down;
            if (maxDot == dotDown) return Direction.Up;
            if (maxDot == dotRight) return Direction.Left;
            return Direction.Right;
        }
        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        public static Orientation GetOrientation(Transform t)
        {
            var forward = t.forward;
            var dir = new Vector2(forward.x, forward.z).normalized;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                return Orientation.Horizontal;
            }
            else
            {
                return Orientation.Vertical;
            }
        }

        //public bool a = true;
        //public bool b = true;
        //public float test;
        public override void SetMetadata(GameObject dungeonItem, DungeonNodeSpawnData spawnData)
        {
            if (dungeonItem != null)
            {
                var entity = dungeonItem.GetComponent<Entity>();
                if (entity != null)
                {
                    var currentCoord = DungeonController.Instance.WorldPositionToTileCoord(entity.transform.position);
                    if (entity.lastCoord != currentCoord)
                    {
                        entity.lastCoord = currentCoord;
                        if (entity is Entity_Edge edgeEntity)
                        {
                            var position = dungeonItem.transform.position;
                            var x = Mathf.FloorToInt(position.x);
                            var z = Mathf.FloorToInt(position.z);
                            var startCoord = new Vector2Int(x, z);
                            var data = DataManager_Tile.Instance.GetData(startCoord);
                            if (data != null)
                            {
                                data.isEdge = true;
                                var orientation = GetOrientation(dungeonItem.transform);
                                switch (orientation)
                                {
                                    case Orientation.Horizontal:
                                        var data1 = DataManager_Tile.Instance.GetData(new Vector2Int(startCoord.x, startCoord.y + 1));
                                        data1.isEdge = true;
                                        var data2 = DataManager_Tile.Instance.GetData(new Vector2Int(startCoord.x, startCoord.y - 1));
                                        data2.isEdge = true;
                                        var data3 = DataManager_Tile.Instance.GetData(new Vector2Int(startCoord.x, startCoord.y - 2));
                                        data3.isEdge = true;
                                        break;
                                    case Orientation.Vertical:
                                        var data4 = DataManager_Tile.Instance.GetData(new Vector2Int(startCoord.x + 1, startCoord.y));
                                        data4.isEdge = true;
                                        var data5 = DataManager_Tile.Instance.GetData(new Vector2Int(startCoord.x - 1, startCoord.y));
                                        data5.isEdge = true;
                                        var data6 = DataManager_Tile.Instance.GetData(new Vector2Int(startCoord.x - 2, startCoord.y));
                                        data6.isEdge = true;
                                        break;
                                    default:
                                        break;
                                }
                            }

                            //if (edgeEntity.parentCellData != null)
                            //{
                            //    edgeEntity.parentCellData.RomoveEdge(edgeEntity);
                            //}
                            //var cellData = DataManager_Cell.Instance.GetData(edgeEntity.transform.position);
                            //if (cellData != null)
                            //{
                            //    edgeEntity.parentCellData = cellData;
                            //    cellData.AddEdge(edgeEntity);
                            //}
                        }
                    }

                }
            }
        }
        //{
        //    var cell = DungeonController.Instance.GetCellFromWorldPosition(dungeonItem.transform.position);
        //    Debug.Log(cell.NodeCoord.ToVector2());
        //}
        //{
        //    
        //    {
        //        
        //        if (building == null || building.registered) return;
        //        var marker = spawnData.socket;
        //        var gridcoord = new IntVector2(marker.gridPosition.x, marker.gridPosition.z);
        //        //var cell = DungeonController.Instance.GetCellFromTileCoord(gridcoord);
        //        var cell = DungeonController.Instance.GetCellFromWorldPosition(dungeonItem.transform.position);
        //        if (building is Entity_Cell cellEntity)
        //        {
        //            cellEntity.Init(cell);
        //        }
        //        //else if (building is Entity_Edge edgeEntitly)
        //        //{
        //        //    var tileCoord = cell.TileCoord;

        //        //    FlowTilemapEdge edge = null;
        //        //    var y = Mathf.FloorToInt(transform.rotation.eulerAngles.y);
        //        //    if (y == 0)
        //        //    {
        //        //        edge = DungeonController.Instance.GetDownEdgeFromTileCoord(tileCoord);
        //        //    }
        //        //    else if (y == 90)
        //        //    {
        //        //        edge = DungeonController.Instance.GetLeftEdgeFromTileCoord(tileCoord);

        //        //    }
        //        //    else if (y == 180)
        //        //    {
        //        //        var newCoord = new IntVector2(tileCoord.x, tileCoord.y - 1);
        //        //        edge = DungeonController.Instance.GetUpEdgeFromTileCoord(newCoord);
        //        //    }
        //        //    else if (y == 270)
        //        //    {
        //        //        var newCoord = new IntVector2(tileCoord.x + 1, tileCoord.y);
        //        //        edge = DungeonController.Instance.GetRightEdgeFromTileCoord(newCoord);
        //        //    }
        //        //    edgeEntitly.Init(edge);
        //        //}
        //    }
        //}
        //{
        //    if (dungeonItem != null)
        //    {
        //        Debug.Log(dungeonItem, dungeonItem);

        //        var cell = DungeonController.Instance.GetCellFromTileCoord(gridcoord);
        //        var building = dungeonItem.GetComponent<BuildingEntity>();
        //        if (building is Entity_Cell cellEntity)
        //        {
        //            if (cellEntity.randomAngle)
        //            {
        //                var rotation = Quaternion.Euler(0, GetRandomRotation(), 0);
        //                cellEntity.transform.rotation = rotation;
        //            }
        //            cellEntity.Init(cell);
        //            EntitiyManager_Cell.Instance.Regist(cellEntity);
        //        }

        //        //var directionForWorld = GetDirectionForWorld(dungeonItem.transform.rotation);
        //        //building_Edge.SetDirection(directionForWorld);
        //    }

        //}
    }



    //public void LogInfo(FlowTilemapCell cell)
    //{
    //    var info = cells[cell];
    //    Debug.Log($"Cell : <{info.name}>", info);
    //    Debug.Log($"Floor : <{info.floor.name}>", cells[cell].floor);
    //    for (int i = 0; i < info.walls.Count; i++)
    //    {
    //        Debug.Log($"Wall[{i}] : <{info.walls[i].name}>", info.walls[i]);
    //    }
    //}

    //public CellEntity GetInfo(FlowTilemapCell item)
    //{
    //    if (cells.TryGetValue(item, out var reslut))
    //    {
    //        return reslut;
    //    }
    //    else
    //    {
    //        Debug.LogError($"No CellInfo on <{item.TileCoord},{item.TileCoord.y}>");
    //    }
    //    return null;
    //}


}

