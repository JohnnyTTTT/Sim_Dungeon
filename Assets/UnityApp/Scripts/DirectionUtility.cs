using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public static class DirectionUtility
    {
        private static Vector3 dirUp = Vector3.forward;    // 世界前
        private static Vector3 dirDown = Vector3.back;     // 世界后
        private static Vector3 dirRight = Vector3.right;   // 世界右
        private static Vector3 dirLeft = Vector3.left;     // 世界左


        public static FourDirectionalRotation GetDirectionForWorld(Quaternion rotation)
        {
            var forward = rotation * Vector3.forward;

            forward.y = 0;
            forward.Normalize();

            var dotUp = Vector3.Dot(forward, dirUp);
            var dotDown = Vector3.Dot(forward, dirDown);
            var dotRight = Vector3.Dot(forward, dirRight);
            var dotLeft = Vector3.Dot(forward, dirLeft);

            float maxDot = Mathf.Max(dotUp, dotDown, dotRight, dotLeft);

            if (maxDot == dotUp) return FourDirectionalRotation.North;
            if (maxDot == dotDown) return FourDirectionalRotation.North;
            if (maxDot == dotRight) return FourDirectionalRotation.West;
            return FourDirectionalRotation.East;
        }

        public static FourDirectionalRotation GetDirection(Vector3 A, Vector3 B)
        {
            var diff = A - B;
            diff.y = 0; 

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
            {
                return diff.x > 0 ? FourDirectionalRotation.East : FourDirectionalRotation.West;
            }
            else
            {
                return diff.z > 0 ? FourDirectionalRotation.North : FourDirectionalRotation.South;
            }
        }

        public static Orientation GetOrientation(Transform t)
        {
            var forward = t.forward;
            var dir = new Vector2(forward.x, forward.z).normalized;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                return Orientation.Vertical;
            }
            else
            {
                return Orientation.Horizontal;
            }
        }
    }
}
