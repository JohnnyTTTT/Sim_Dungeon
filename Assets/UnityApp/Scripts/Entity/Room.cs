using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    [System.Serializable]
    public class Room
    {
        public string name;
        public Vector3 worldCenter;
        public List<Data_Cell> containedCells = new List<Data_Cell>();
        public Color roomColor;

        public void Init(string n)
        {
            name = n;
            roomColor = Random.ColorHSV();
        }

        public void AddCell(Data_Cell cellData)
        {
            containedCells.Add(cellData);
            cellData.parentRoom = this;

            var sum = Vector3.zero;
            foreach (var item in containedCells)
            {
                //Debug.Log(cellData.worldPosition);
                sum += cellData.worldPosition;
            }
            worldCenter = sum / containedCells.Count;
        }

        public void RemoveCell(Data_Cell cellData)
        {
            containedCells.Remove(cellData);
        }

#if UNITY_EDITOR
        public  void DrawGizmos()
        {
            foreach (var item in containedCells)
            {
                GizmoUnitily.DrawFourSizeCube(item.Data.TileCoord, roomColor, true);
            }
            GizmoUnitily.DrawLabel(worldCenter, name);
        }
#endif
    }
}
