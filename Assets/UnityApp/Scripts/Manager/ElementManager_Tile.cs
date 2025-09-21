using DungeonArchitect;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Element_Tile : Element
    {
        public IntVector2 coord;
        public Vector3 worldPosition;
        public Element_Cell parentCell;
        public bool isEdge;

        public Element_Tile(IntVector2 vector)
        {
            coord = vector;
            worldPosition = new Vector3(coord.x + 0.5f, 0f, coord.y + 0.5f);
        }
        public void DrawGizmos()
        {
            GizmoUnitily.DrawOneSizeCube(worldPosition, isEdge ? Color.red : Color.blue, true);
        }
    }

    public class ElementManager_Tile : ElementManager<Element_Tile>
    {
        public static ElementManager_Tile Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_Tile>();
                }
                return s_Instance;
            }
        }
        private static ElementManager_Tile s_Instance;

        public void Init(EasyGridBuilderPro easyGridBuilder)
        {
            if (Inited) return;
            for (int x = 0; x < easyGridBuilder.GetGridWidth(); x++)
            {
                for (int z = 0; z < easyGridBuilder.GetGridLength(); z++)
                {
                    var position = new IntVector2(x, z);
                    var worldPosition = new Vector3(position.x + 0.5f, 0f, position.y + 0.5f);
                    var cell = ElementManager_Cell.Instance.GetElement(worldPosition);
                    var newData = new Element_Tile(position);
                    newData.parentCell = cell;
                    cell.tiles.Add(newData);
                    map.Add(newData.coord, newData);
                }
            }
            Inited = true;
            Debug.Log($"[-----System-----] : DataManager_Tile inited , tile count <{map.Count}>");
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
    }
}
