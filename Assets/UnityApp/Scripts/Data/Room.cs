using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class Room : ElementData
    {
        public string name;
        public List<Data_Cell> containedCells = new List<Data_Cell>();
        public RoomType roomType
        {
            get
            {
                return m_RoomType;
            }
        }
        private RoomType m_RoomType;

        public IntVector2 spawnNodeCoord;
        public Color roomColor;
        public Bounds bounds;
        public Vector3 center;
        public void Init(string n, RoomType type)
        {
            name = n;
            m_RoomType = type;


            roomColor = Random.ColorHSV();
        }

        public void AddCell(Data_Cell cellData)
        {
            containedCells.Add(cellData);
            cellData.parentRoom = this;
            CalculateBounds();
        }
        public void CalculateBounds()
        {
            if (containedCells == null || containedCells.Count == 0)
            {
                bounds = new Bounds(Vector3.zero, Vector3.zero);
                center = Vector3.zero;
                return;
            }

            // 初始化 bounds
            bounds = new Bounds(containedCells[0].worldPosition, Vector3.zero);

            // 包含所有格子
            foreach (var cell in containedCells)
            {
                bounds.Encapsulate(cell.worldPosition);
            }

            // Y轴可以忽略或保持为0
            center = new Vector3(bounds.center.x, 0f, bounds.center.z);
        }

        public void RemoveCell(Data_Cell cellData)
        {
            containedCells.Remove(cellData);
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            foreach (var item in containedCells)
            {
                GizmoUnitily.DrawTwoSizeCube(item.Data.TileCoord, roomColor, true);
            }
            GizmoUnitily.DrawLabel(center, name);
        }
#endif
    }
}
