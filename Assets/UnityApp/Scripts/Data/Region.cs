using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.UI.Image;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class Region : Element
    {
        public string name;
        public RoomType roomType;
        public HashSet<Element_LargeCell> containedLargeCells = new HashSet<Element_LargeCell>();
        public HashSet<Element_SmallCell> containedSmallCells = new HashSet<Element_SmallCell>();
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

        public void AddSamllCell(Element_SmallCell cell)
        {
            cell.region = this;
            containedSmallCells.Add(cell);
        }

        public void RemoveSmallCell(Element_SmallCell cell)
        {
            cell.region = null;
            containedSmallCells.Remove(cell);
        }

        public void AddLargeCell(Element_LargeCell cell)
        {
            cell.region = this;
            containedLargeCells.Add(cell);
        }

        public void RemoveLargeCell(Element_LargeCell cell)
        {
            cell.region = null;
            containedLargeCells.Remove(cell);
        }


        public void CalculateBounds()
        {
            // 初始化 bounds
            bounds = new Bounds(containedLargeCells.First().worldPosition, Vector3.zero);

            // 包含所有格子
            foreach (var cell in containedLargeCells)
            {
                bounds.Encapsulate(cell.worldPosition);
            }

            // Y轴可以忽略或保持为0
            center = new Vector3(bounds.center.x, 0f, bounds.center.z);
        }

        public void Clear()
        {
            foreach (var item in containedLargeCells)
            {
                item.region = null;
            }
            containedLargeCells.Clear();
        }



        public override string ToString()
        {
            return name;
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            foreach (var item in containedLargeCells)
            {
                GizmoUnitily.DrawTwoSizeCube(item.worldPosition, roomColor, true);
}
            Color.RGBToHSV(roomColor, out float h, out float s, out float v);
            v *= 0.3f; 
            var darker = Color.HSVToRGB(h, s, v);
            foreach (var item in containedSmallCells)
            {
                GizmoUnitily.DrawOneSizeCube(item.worldPosition, darker, true);
            }
            GizmoUnitily.DrawLabel(center, name);
        }






#endif
    }
}
