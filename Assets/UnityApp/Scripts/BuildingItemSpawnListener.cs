using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Johnny.SimDungeon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum RelativeDirection
    {
        Left,
        Up,
        Right,
        Down
    }

    public class BuildingItemSpawnListener : DungeonItemSpawnListener
    {


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

        /// <summary>
        /// 判断 a 在 b 的上下左右哪个方向
        /// </summary>
        public static RelativeDirection GetDirection(Vector3 a, Vector3 b)
        {
            var dir = new Vector2(a.x - b.x, a.z - b.z);

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // X 方向差距更大
            {
                return dir.x > 0 ? RelativeDirection.Right : RelativeDirection.Left;
            }
            else // Z 方向差距更大
            {
                return dir.y > 0 ? RelativeDirection.Up : RelativeDirection.Down;
            }
        }

        public override void SetMetadata(GameObject dungeonItem, DungeonNodeSpawnData spawnData)
        {
            if (dungeonItem != null)
            {
                var buildingParts = dungeonItem.GetComponentsInChildren<BuildingPart>();
                foreach (var item in buildingParts)
                {
                    if (item is Building_Edge building_Edge)
                    {
                        var cell = DungeonController.Instance.GetCellFromWorldPosition(building_Edge.transform.position);
                        var entitly = CellEntitiyManager.Instance.GetCellEntitly(cell);
                        var direction = GetDirection(building_Edge.transform.position, entitly.transform.position);
                        switch (direction)
                        {
                            case RelativeDirection.Left:
                                entitly.edges[0].buildingPart = building_Edge;
                                break;
                            case RelativeDirection.Up:
                                entitly.edges[1].buildingPart = building_Edge;
                                break;
                            case RelativeDirection.Right:
                                entitly.edges[2].buildingPart = building_Edge;
                                break;
                            case RelativeDirection.Down:
                                entitly.edges[3].buildingPart = building_Edge;
                                break;
                            default:
                                break;
                        }
                        item.parent = entitly;
                    }
                }
                //if (!cells.TryGetValue(gridPosition, out var cell))
                //{
                //    cell = new GameObject(gridPosition.ToString());

                //    cell.transform.parent = m_DungeonController.pooledDungeonSceneProvider.itemParent.transform;
                //    cells[gridPosition] = cell;
                //}
                //dungeonItem.transform.parent = cell.transform;
                //Debug.Log(marker.gridPosition);
                //var buildingParts = dungeonItem.GetComponentsInChildren<BuildingPart>();
                //foreach (var item in buildingParts)
                //{
                //    var position = new Vector3(item.transform.position.x, 0f, item.transform.position.z);
                //    var info = TryGetValue(m_DungeonController.dungeonModel.GetTilemapCell(position));
                //    //Debug.Log(spawnData.socket.gridPosition);
                //    //Debug.Log(m_DungeonController.dungeonModel.GetTilemapCell(position).TileCoord.x+" "+ m_DungeonController.dungeonModel.GetTilemapCell(position).TileCoord.y);
                //    switch (item.type)
                //    {
                //        case FlowTilemapCellType.Empty:
                //            break;
                //        case FlowTilemapCellType.Floor:
                //            info.floor = item;
                //            break;
                //        case FlowTilemapCellType.Wall:
                //            Vector3 direction = transform.position - position;
                //            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                //            // 判断方向（角度区间）
                //            if (angle > -45 && angle <= 45)
                //            {
                //                Debug.Log("物体在右侧");
                //            }
                //            else if (angle > 45 && angle <= 135)
                //            {
                //                Debug.Log("物体在上方");
                //            }
                //            else if (angle > 135 || angle <= -135)
                //            {
                //                Debug.Log("物体在左侧");
                //            }
                //            else
                //            {
                //                Debug.Log("物体在下方");
                //            }


                //            //m_DungeonController.dungeonModel.Tilemap.Edges.GetVertical
                //            info.walls.Add(item);
                //            break;
                //        case FlowTilemapCellType.Door:
                //            break;
                //        case FlowTilemapCellType.Custom:
                //            break;
                //        default:
                //            break;
                //    }
                //    item.parent = info;
                //}

                //dungeonItem.transform.parent = parent.transform;
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
}
