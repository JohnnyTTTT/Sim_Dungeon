using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Johnny.SimDungeon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class SimDungeonCellInfo
    {
        public BuildingPart floor;
        public List<BuildingPart> walls = new List<BuildingPart>();
    }
    public class BuildingItemSpawnListener : DungeonItemSpawnListener
    {
        [SerializeField] private DungeonController m_DungeonController;
        [SerializeField] private Transform m_CellEntityParent;
        // private Dictionary<IntVector, GameObject> cells = new Dictionary<IntVector, GameObject>();
        private Dictionary<FlowTilemapCell, CellEntity> cells = new Dictionary<FlowTilemapCell, CellEntity>();


        private void Start()
        {
            cells.Clear();
        }

        private CellEntity TryGetValue(FlowTilemapCell cell)
        {
            if (!cells.TryGetValue(cell, out var info))
            {
                var name = $"{cell.TileCoord.x},{cell.TileCoord.y}";
                var entity = new GameObject(name);
                entity.transform.position = m_DungeonController.gridFlowDungeonQuery.TileCoordToWorldCoord(cell.TileCoord);
                entity.transform.parent = m_CellEntityParent;
                info = entity.AddComponent<CellEntity>();
                //info = new SimDungeonCellInfo();
                cells[cell] = info;
            }
            return info;
        }

        public  void DestroyCellEntites()
        {
            foreach (var item in cells)
            {
                if (Application.isPlaying)
                {
                    Destroy(item.Value.gameObject);
                }
                else
                {
                    DestroyImmediate(item.Value.gameObject);
                }
            }
        }

        public override void SetMetadata(GameObject dungeonItem, DungeonNodeSpawnData spawnData)
        {
            if (dungeonItem != null)
            {
                var marker = spawnData.socket;
                //var gridPosition = marker.gridPosition;
                //if (!cells.TryGetValue(gridPosition, out var cell))
                //{
                //    cell = new GameObject(gridPosition.ToString());

                //    cell.transform.parent = m_DungeonController.pooledDungeonSceneProvider.itemParent.transform;
                //    cells[gridPosition] = cell;
                //}
                //dungeonItem.transform.parent = cell.transform;
                //Debug.Log(marker.gridPosition);
                var buildingParts = dungeonItem.GetComponentsInChildren<BuildingPart>();
                foreach (var item in buildingParts)
                {
                    var position = new Vector3(item.transform.position.x, 0f, item.transform.position.z);
                    var info = TryGetValue(m_DungeonController.dungeonModel.GetTilemapCell(position));
                    switch (item.type)
                    {
                        case FlowTilemapCellType.Empty:
                            break;
                        case FlowTilemapCellType.Floor:
                            info.floor = item;
                            break;
                        case FlowTilemapCellType.Wall:
                            info.walls.Add(item);
                            break;
                        case FlowTilemapCellType.Door:
                            break;
                        case FlowTilemapCellType.Custom:
                            break;
                        default:
                            break;
                    }
                    item.parent = info;
                }

                //dungeonItem.transform.parent = parent.transform;
            }
        }

        public void LogInfo(FlowTilemapCell cell)
        {
            var info = cells[cell];
            Debug.Log($"Cell : <{info.name}>", info);
            Debug.Log($"Floor : <{info.floor.name}>", cells[cell].floor);
            for (int i = 0; i < info.walls.Count; i++)
            {
                Debug.Log($"Wall[{i}] : <{info.walls[i].name}>", info.walls[i]);
            }
        }

        public CellEntity GetInfo(FlowTilemapCell item)
        {
            if (cells.TryGetValue(item, out var reslut))
            {
                return reslut;
            }
            else
            {
                Debug.LogError($"No CellInfo on <{item.TileCoord},{item.TileCoord.y}>");
            }
            return null;
        }
    }
}
