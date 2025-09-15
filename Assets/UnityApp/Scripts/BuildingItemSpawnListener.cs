using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Johnny.SimDungeon;
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


        /// <summary>
        /// 判断 a 在 cell 的上下左右哪个方向
        /// </summary>
        public static Direction GetDirectionForCell(Vector3 a, Vector3 cell)
        {
            var dir = new Vector2(a.x - cell.x, a.z - cell.z);

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // X 方向差距更大
            {
                return dir.x > 0 ? Direction.Right : Direction.Left;
            }
            else // Z 方向差距更大
            {
                return dir.y > 0 ? Direction.Up : Direction.Down;
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
                    if (currentCoord != entity.lastCoord)
                    {
                        entity.lastCoord = currentCoord;
                        if (entity is Entity_Edge edgeEntity)
                        {
                            if (edgeEntity.parentCellData != null)
                            {
                                edgeEntity.parentCellData.edgeDatas.Remove(edgeEntity);
                            }
                            var cellData = DataManager_Cell.Instance.GetData(edgeEntity.transform.position);
                            if (cellData != null)
                            {
                                edgeEntity.parentCellData = cellData;
                                cellData.edgeDatas.Add(edgeEntity);
                            }
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

