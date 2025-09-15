using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class Room
    {
        public string name;
        public Vector3 worldCenter;
        private List<Data_Cell> m_ContainedCells = new List<Data_Cell>();
        public Color roomColor;

        public void Init(string n)
        {
            name = n;
            roomColor = Random.ColorHSV();
        }

        public void AddCell(Data_Cell cellData)
        {
            m_ContainedCells.Add(cellData);
            cellData.parentRoom = this;

            var sum = Vector3.zero;
            foreach (var item in m_ContainedCells)
            {
                sum += DungeonController.Instance.TileCoordToWorldPosition(cellData.Data.TileCoord);
            }
            worldCenter = sum / m_ContainedCells.Count;
        }

        public void RemoveCell(Data_Cell cellData)
        {
            m_ContainedCells.Remove(cellData);
        }

#if UNITY_EDITOR
        public  void DrawGizmos()
        {
            foreach (var item in m_ContainedCells)
            {
                GizmoUnitily.DrawFourSizeCube(item.Data.TileCoord, roomColor, false);
            }
            GizmoUnitily.DrawLabel(worldCenter, name);
        }
#endif
    }
}
