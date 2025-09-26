using DungeonArchitect;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class GizmoUnitily
    {

        public static Vector3 TwoSize = new Vector3(1.9f, 0.01f, 1.9f);
        public static Vector3 OneSize = new Vector3(0.9f, 0.01f, 0.9f);

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

        public static void DrawLabel(Vector2Int center, string label)
        {
#if UNITY_EDITOR
            var worldCenter = CoordUtility.LargeCoordToWorldPosition(center);
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

        public static void DrawWall(Vector3 position, Color color, bool isHorizontalEdge)
        {
            Gizmos.color = color;
            if (isHorizontalEdge)
            {
                Gizmos.DrawWireCube(position, new Vector3(2f, 0.01f, 0.3f));

            }
            else
            {
                Gizmos.DrawWireCube(position, new Vector3(0.3f, 0.01f, 2f));
            }
        }


        internal static void DrawLine(Vector3 vector3, object worldPosition, Color yellow)
        {
            throw new NotImplementedException();
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

        public static void DrawTwoSizeCube(Vector2Int center, Color color, bool isWire)
        {
            var worldCenter = CoordUtility.LargeCoordToWorldPosition(center);
            DrawTwoSizeCube(worldCenter, color, isWire);
        }
    }
}
