using DungeonArchitect;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class GizmoUnitily
    {

        public static Vector3 TwoSize = new Vector3(2f, 0.01f, 2f);
        public static Vector3 OneSize = new Vector3(1f, 0.01f, 1f);

        public static void DrawLine(Vector3 from, Vector3 to, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(from, to);
        }


        public static void DrawLabel(Vector3 center, string label)
        {
#if UNITY_EDITOR
            Handles.Label(center + new Vector3(0, 1f, 0), label);
#endif
        }

        public static void DrawLabel(IntVector2 center, string label)
        {
#if UNITY_EDITOR
            var worldCenter = DungeonController.Instance.TileCoordToWorldPosition(center);
            DrawLabel(worldCenter, label);
#endif
        }

        public static void DrawOneSizeCube(Vector3 center, Color color, bool isWire)
        {
            Gizmos.color = color;
            if (isWire)
            {
                Gizmos.DrawWireCube(center + new Vector3(0, 0.01f, 0), OneSize);
            }
            else
            {
                Gizmos.DrawCube(center + new Vector3(0, 0.01f, 0), OneSize);
            }
        }

        public static void DrawWall(Vector3 center, Color color, bool isHorizontalEdge)
        {
            Gizmos.color = color;
            var offset = isHorizontalEdge ?  new Vector3(0f, 0f, -1f) : new Vector3(-1f, 0f, 0f) ;
            if (isHorizontalEdge)
            {
    
                Gizmos.DrawWireCube(center + offset, new Vector3(2f, 0.01f, 0.3f));

            }
            else
            {
                Gizmos.DrawWireCube(center + offset, new Vector3(0.3f, 0.01f, 2f));
            }
        }
        public static void DrawWall(IntVector2 center, Color color, bool isHorizontalEdge)
        {
            var position = DungeonController.Instance.TileCoordToWorldPosition(center);
            DrawWall(position, color, isHorizontalEdge);
        }
        public static void DrawTwoSizeCube(Vector3 center, Color color, bool isWire)
        {
            Gizmos.color = color;
            if (isWire)
            {
                Gizmos.DrawWireCube(center + new Vector3(0, 0.01f, 0), TwoSize);
            }
            else
            {
                Gizmos.DrawCube(center + new Vector3(0, 0.01f, 0), TwoSize);
            }
        }

        public static void DrawTwoSizeCube(IntVector2 center, Color color, bool isWire)
        {
            var worldCenter = DungeonController.Instance.TileCoordToWorldPosition(center);
            DrawTwoSizeCube(worldCenter, color, isWire);
        }
    }
}
