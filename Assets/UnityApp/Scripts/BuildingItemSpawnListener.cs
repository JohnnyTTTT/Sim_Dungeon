using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Johnny.SimDungeon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{


    public class BuildingItemSpawnListener : DungeonItemSpawnListener
    {
        private static readonly float[] Angles = { 0f, 90f, 180f, 270f };
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

        private float GetRandomRotation()
        {
            var index = UnityEngine.Random.Range(0, Angles.Length);
            return Angles[index];
        }
        public bool a = true;
        public bool b = true;
        public float test;
        public override void SetMetadata(GameObject dungeonItem, DungeonNodeSpawnData spawnData)
        {
            if (dungeonItem != null)
            {
                var marker = spawnData.socket;
                var gridcoord = new IntVector2(marker.gridPosition.x, marker.gridPosition.z);
                var cell = DungeonController.Instance.GetCellFromTileCoord(gridcoord);
                var building = dungeonItem.GetComponent<BuildingEntity>();
                if (building is CellEntity cellEntity)
                {
                    if (cellEntity.randomAngle)
                    {
                        var rotation = Quaternion.Euler(0, GetRandomRotation(), 0);
                        cellEntity.transform.rotation = rotation;
                    }
                    cellEntity.Init(cell);
                    EntitiyManager_Cell.Instance.Regist(cellEntity);
                }
                else if (building is EdgeEntity edgeEntitly)
                {
                    //EdgeEntity
                    var y = Mathf.FloorToInt(spawnData.transform.rotation.eulerAngles.y);
                    FlowTilemapEdge edge = null;
                    if (y == 0)
                    {
                        edge = DungeonController.Instance.GetDownEdgeFromTileCoord(gridcoord);
                    }
                    else if (y == 90)
                    {
                        edge = DungeonController.Instance.GetLeftEdgeFromTileCoord(gridcoord);

                    }
                    else if (y == 180)
                    {
                        edge = DungeonController.Instance.GetUpEdgeFromTileCoord(gridcoord);
                    }
                    else if (y == 270)
                    {
                        edge = DungeonController.Instance.GetRightEdgeFromTileCoord(gridcoord);
                    }
                    edgeEntitly.Init(edge);
                    EntitylManager_Edge.Instance.Regist(edgeEntitly);

                    //SubEdgeEntity
                    //foreach (var subEdgeEntity in edgeEntitly.subEdges)
                    //{
                    //    var realCellEntity = EntitiyManager_Cell.Instance.GetCellEntitly(subEdgeEntity.transform.position);
                    //    var dir = GetDirectionForCell(subEdgeEntity.transform.position, realCellEntity.transform.position);
                    //    switch (dir)
                    //    {
                    //        case Direction.Left:
                    //            realCellEntity.edges[0] = subEdgeEntity;
                    //            break;
                    //        case Direction.Up:
                    //            realCellEntity.edges[1] = subEdgeEntity;
                    //            break;
                    //        case Direction.Right:
                    //            realCellEntity.edges[2] = subEdgeEntity;
                    //            break;
                    //        case Direction.Down:
                    //            realCellEntity.edges[3] = subEdgeEntity;
                    //            break;
                    //    }
                    //}
                    //Debug.Log("aaa   " + marker.gridPosition, edgeEntitly);
                    //Debug.Log(EntitiyManager_Cell.Instance.GetCellEntitly(cell), EntitiyManager_Cell.Instance.GetCellEntitly(cell));
                    //Debug.Log(marker.SocketType);


                    //foreach (var item in edgeEntitly.edges)
                    //{
                    //    if (item.replaceableObjectSO == null)
                    //    {
                    //        edgeEntitly.SetReplaceableObjectSO(item, BuildableAssets.Instance.stoneWall);
                    //    }
                    //    var cell = DungeonController.Instance.GetCellFromTileCoord(gridcoord);
                    //    var entitly = CellEntitiyManager.Instance.GetCellEntitly(cell);
                    //    edgeEntitly.Init(cell,);



                    //var directionForCell = GetDirectionForCell(building_Edge.transform.position, entitly.transform.position);
                    //var edgeEntitly = directionForCell switch
                    //{
                    //    Direction.Left => entitly.edges[0],
                    //    Direction.Up => entitly.edges[1],
                    //    Direction.Right => entitly.edges[2],
                    //    Direction.Down => entitly.edges[3],
                    //};
                    //edgeEntitly.buildingPart = building_Edge;
                    //edgeEntitly.cellEntity =
                    //item.cellEntitly = entitly;
                }
                //var directionForWorld = GetDirectionForWorld(dungeonItem.transform.rotation);
                //building_Edge.SetDirection(directionForWorld);
            }

        }
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

