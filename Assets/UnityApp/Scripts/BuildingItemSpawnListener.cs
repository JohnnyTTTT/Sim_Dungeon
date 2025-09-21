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


        //public bool a = true;
        //public bool b = true;
        //public float test;
        public override void SetMetadata(GameObject dungeonItem, DungeonNodeSpawnData spawnData)
        {
            //if (dungeonItem != null)
            //{
            //    var entity = dungeonItem.GetComponent<Entity>();
            //    if (entity != null)
            //    {
            //        var currentCoord = DungeonController.Instance.WorldPositionToTileCoord(entity.transform.position);
            //        if (entity.lastCoord != currentCoord)
            //        {
            //            entity.lastCoord = currentCoord;
            //            entity.UpdateData();
            //            if (DungeonController.Instance.worldDataInited)
            //            {
            //                entity.ApplyBiomeRule();
            //            }
            //        }
            //    }
            //}
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

    }
}

