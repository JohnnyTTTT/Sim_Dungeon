using DungeonArchitect;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

namespace Johnny.SimDungeon
{
    public enum FlowTilemapSmallCellType
    {
        Empty,
        Floor,
        Wall,
        Door,
    }
    public class Element_SmallCell : Element
    {
        public Element_SmallCell[] neighbors = new Element_SmallCell[4];
        public Vector2Int coord;
        public Vector3 worldPosition;
        public FlowTilemapSmallCellType cellType;
        public Region region;
        //public Element_Edge wall;
        //public Element_LargeCell parentCell;

        public Element_SmallCell(Vector2Int vector)
        {
            coord = vector;
            worldPosition = CoordUtility.SmallCoordToWorldPosition(coord) + new Vector3(0.5f, 0f, 0.5f);
            //parentCell = ElementManager_LargeCell.Instance.GetElement(worldPosition);

        }

        public void DrawGizmos()
        {
            var color = (cellType) switch
            {
                FlowTilemapSmallCellType.Empty => Color.black,
                FlowTilemapSmallCellType.Floor => Color.blue,
                FlowTilemapSmallCellType.Wall => Color.red,
                FlowTilemapSmallCellType.Door => Color.green,
            };
            GizmoUnitily.DrawOneSizeCube(worldPosition + new Vector3(0.5f, 0f, 0.5f), color, true);
        }
    }

    public class ElementManager_SmallCell : ElementManager<Element_SmallCell>
    {
        public static ElementManager_SmallCell Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_SmallCell>();
                }
                return s_Instance;
            }
        }
        private static ElementManager_SmallCell s_Instance;

        public void Init(EasyGridBuilderPro easyGridBuilder)
        {
            for (int x = 0; x < easyGridBuilder.GetGridWidth(); x++)
            {
                for (int z = 0; z < easyGridBuilder.GetGridLength(); z++)
                {
                    var position = new Vector2Int(x, z);
                    var newData = new Element_SmallCell(position);
                    map.Add(newData.coord, newData);
                }
            }
            Debug.Log($"[-----System-----] : DataManager_Tile inited , tile count <{map.Count}>");
        }
        public void PostInit()
        {
            foreach (var element in map.Values)
            {
                var coord = element.coord;

                element.neighbors[0] = GetLeftCellFromCoord(coord);
                element.neighbors[1] = GetUpCellFromCoord(coord);
                element.neighbors[2] = GetRightCellFromCoord(coord);
                element.neighbors[3] = GetDownCellFromCoord(coord);
            }
        }

        public override Element_SmallCell GetElement(Vector3 worldPosition)
        {
            var coord = CoordUtility.WorldPositionToSmallCoord(worldPosition);
            return GetElement(coord);
        }

        public Element_SmallCell GetLeftCellFromCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.LEFT);
        }

        public Element_SmallCell GetUpCellFromCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.UP);
        }

        public Element_SmallCell GetRightCellFromCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.RIGHT);
        }

        public Element_SmallCell GetDownCellFromCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.DOWN);
        }

        public void UnInit()
        {
            map.Clear();
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
    }
}
