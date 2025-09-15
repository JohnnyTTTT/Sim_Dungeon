using DungeonArchitect;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class GizmoUnitily
    {

        public static Vector3 FourSize = new Vector3(4f, 0.01f, 4f);

        public static void DrawLine(Vector3 from, Vector3 to, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(from, to);
        }


        public static void DrawLabel(Vector3 center, string label)
        {
#if UNITY_EDITOR
            Handles.Label(center + new Vector3(0, 0.01f, 0), label);
#endif
        }

        public static void DrawLabel(IntVector2 center, string label)
        {
#if UNITY_EDITOR
            var worldCenter = DungeonController.Instance.TileCoordToWorldPosition(center);
            DrawLabel(worldCenter, label);
#endif
        }

        public static void DrawFourSizeCube(Vector3 center, Color color, bool isWire)
        {
            Gizmos.color = color;
            if (isWire)
            {
                Gizmos.DrawWireCube(center + new Vector3(0, 0.01f, 0), FourSize);
            }
            else
            {
                Gizmos.DrawCube(center + new Vector3(0, 0.01f, 0), FourSize);
            }
        }

        public static void DrawFourSizeCube(IntVector2 center, Color color, bool isWire)
        {
            var worldCenter = DungeonController.Instance.TileCoordToWorldPosition(center);
            DrawFourSizeCube(worldCenter, color, isWire);
        }
    }
}
