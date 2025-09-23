using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class Room : Element
    {
        public string name;
        public RoomType roomType;
        public List<Element_Cell> containedCells = new List<Element_Cell>();
        public bool isClosed;

        public BiomeSO biome;
        public Color roomColor;
        public Bounds bounds;
        public Vector3 center;

        public void Init(string n, RoomType type)
        {
            name = n;
            roomType = type;
            roomColor = Random.ColorHSV();
        }

        public void AddCell(Element_Cell cellElement)
        {
            containedCells.Add(cellElement);
            cellElement.area = this;
            CalculateBounds();
        }
        public void AddCells(IEnumerable<Element_Cell> cells)
        {
            foreach (var item in cells)
            {
                containedCells.Add(item);
                item.area = this;
                CalculateBounds();
            }
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

        public void Clear()
        {
            foreach (var item in containedCells)
            {
                item.area = null;
            }
            containedCells.Clear();
        }

        public void RemoveCell(Element_Cell cellData)
        {
            containedCells.Remove(cellData);
            CalculateBounds();
        }

        public override string ToString()
        {
            return name;
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            foreach (var item in containedCells)
            {
                //item.DrawGizmos();
                GizmoUnitily.DrawTwoSizeCube(item.Data.TileCoord, roomColor, true);
            }
            GizmoUnitily.DrawLabel(center, name);
        }


#endif
    }
}
