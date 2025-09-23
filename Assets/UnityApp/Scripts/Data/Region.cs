using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class Region : Element
    {
        public string name;
        public RoomType roomType;
        public HashSet<Element_Cell> containedCells = new HashSet<Element_Cell>();
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
            cellElement.region = this;
            CalculateCells();
        }

        public void RemoveCell(Element_Cell cellData)
        {
            cellData.region = null;
            containedCells.Remove(cellData);
            CalculateCells();
        }

        public void AddCells(IEnumerable<Element_Cell> cells)
        {
            foreach (var item in cells)
            {
                item.region = this;
                containedCells.Add(item);
                CalculateCells();
            }
        }

        public void RemoveCells(IEnumerable<Element_Cell> cells)
        {
            foreach (var cell in cells)
            {
                cell.region = null;
                containedCells.Remove(cell);
                CalculateCells();
            }

        }
        public void CalculateCells()
        {
            if (containedCells == null || containedCells.Count == 0)
            {
                ElementManager_Region.Instance.RemoveRegion(this);
                return;
            }

            // 初始化 bounds
            bounds = new Bounds(containedCells.First().worldPosition, Vector3.zero);

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
                item.region = null;
            }
            containedCells.Clear();
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
