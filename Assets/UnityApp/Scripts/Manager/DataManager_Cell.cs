using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class DataManager_Cell : EntityManager<FlowTilemapCell, Data_Cell>
    {
        public static DataManager_Cell Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DataManager_Cell>();
                }
                return s_Instance;
            }

        }
        private static DataManager_Cell s_Instance;

        [Title("Titles and Headers")]
        public bool drawGizmos;

        public void Init(FlowTilemapCellDatabase cells)
        {
            map.Clear();
            foreach (var cell in cells)
            {
                if (cell.CellType == FlowTilemapCellType.Floor)
                {
                    var data = new Data_Cell(cell);
                    map.Add(cell, data);
                }
            }
        }

        public Data_Cell GetData(FlowTilemapCell cell)
        {
            if (map.TryGetValue(cell, out var entitly))
            {
                return entitly;
            }
            return null;
        }

        public Data_Cell GetData(IntVector2 coord)
        {
            var cell = DungeonController.Instance.GetCellFromTileCoord(coord);
            return GetData(cell);
        }

        public Data_Cell GetData(Vector3 worldPosition)
        {
            var cell = DungeonController.Instance.GetCellFromWorldPosition(worldPosition);
            return GetData(cell);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in map)
                {
                    item.Value.DrawGizmos();
                }

                //var origin = transform.position - new Vector3(2, 0, 2);
                //for (int x = 0; x < 4; x++)
                //{
                //    for (int y = 0; y < 4; y++)
                //    {
                //        var subCenter = origin + new Vector3(x + 0.5f, 0.125f / 2f, y + 0.5f);
                //        var subSize = new Vector3(1f, 0.125f, 1f);
                //        Gizmos.color = Color.blue;
                //        Gizmos.DrawWireCube(subCenter, subSize);
                //        
                //    }
                //}
            }

        }

    }
}