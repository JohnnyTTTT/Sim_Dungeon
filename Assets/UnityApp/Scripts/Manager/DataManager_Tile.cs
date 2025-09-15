using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Data_Tile
    {
        public Vector2Int coord;
        public Vector3 worldPosition;
        public Data_Cell parentCell;
        public bool isEdge;

        public Data_Tile(Vector2Int vector)
        {
            coord = vector;
            worldPosition = new Vector3(coord.x + 0.5f, 0f, coord.y + 0.5f);
        }
        public void DrawGizmos()
        {
            GizmoUnitily.DrawOneSizeCube(worldPosition, isEdge ? Color.red : Color.blue, true);
        }
    }
    public class DataManager_Tile : EntityManager<Vector2Int, Data_Tile>
    {
        public static DataManager_Tile Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DataManager_Tile>();
                }
                return s_Instance;
            }
        }
        private static DataManager_Tile s_Instance;
        [Title("Titles and Headers")]
        public bool drawGizmos;
        public void Init(EasyGridBuilderPro easyGridBuilder)
        {
            if (Inited) return;
            for (int x = 0; x < easyGridBuilder.GetGridWidth(); x++)
            {
                for (int z = 0; z < easyGridBuilder.GetGridLength(); z++)
                {
                    var position = new Vector2Int(x, z);
                    var newData = new Data_Tile(position);
                    newData.parentCell = DataManager_Cell.Instance.GetData(newData.worldPosition);
                    map.Add(newData.coord, newData);
                }
            }
        }

        public void UnInit()
        {
            map.Clear();
            Inited = false;
        }
        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in map)
                {
                    item.Value.DrawGizmos();
                }
            }
        }

        public Data_Tile GetData(Vector2Int coord)
        {
            if (map.TryGetValue(coord, out var data))
            {
                return data;
            }
            return null;
        }
    }
}
