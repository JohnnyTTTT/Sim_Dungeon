using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum Direction
    {
        Left,
        Up,
        Right,
        Down
    }

    public static class DirectionUtility
    {
        public static readonly Vector2Int LEFT = new Vector2Int(-1, 0);
        public static readonly Vector2Int UP = new Vector2Int(0, 1);
        public static readonly Vector2Int RIGHT = new Vector2Int(1, 0);
        public static readonly Vector2Int DOWN = new Vector2Int(0, -1);

        public static readonly Vector2Int[] CardinalDirections ={
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1)};

        public static Vector3 dirLeft = Vector3.left;
        public static Vector3 dirUp = Vector3.forward;
        public static Vector3 dirRight = Vector3.right;
        public static Vector3 dirDown = Vector3.back;     
       
        public static Direction ToDirection(FourDirectionalRotation pluginDir)
        {
            return pluginDir switch
            {
                FourDirectionalRotation.North => Direction.Left,   // 插件 North(270°) → 我的 Left(270°)
                FourDirectionalRotation.East => Direction.Up,     // 插件 East(0°)   → 我的 Up(0°)
                FourDirectionalRotation.South => Direction.Right,  // 插件 South(90°) → 我的 Right(90°)
                FourDirectionalRotation.West => Direction.Down,   // 插件 West(180°) → 我的 Down(180°)
            };
        }

        public static FourDirectionalRotation ToEdgeFourDirectionalRotation(Direction myDir)
        {
            return myDir switch
            {
                Direction.Left => FourDirectionalRotation.North,  // 我的 Left(270°)→ 插件 North(270°)
                Direction.Up => FourDirectionalRotation.East,   // 我的 Up(0°)    → 插件 East(0°)
                Direction.Right => FourDirectionalRotation.South,  // 我的 Right(90°)→ 插件 South(90°)
                Direction.Down => FourDirectionalRotation.West,   // 我的 Down(180°)→ 插件 West(180°)
            };
        }

        public static Direction GetDirectionForWorld(Quaternion rotation)
        {

            var forward = rotation * Vector3.forward;

            forward.y = 0;
            forward.Normalize();

            var dotUp = Vector3.Dot(forward, dirUp);
            var dotDown = Vector3.Dot(forward, dirDown);
            var dotRight = Vector3.Dot(forward, dirRight);
            var dotLeft = Vector3.Dot(forward, dirLeft);

            float maxDot = Mathf.Max(dotUp, dotDown, dotRight, dotLeft);

            if (maxDot == dotUp)
                return Direction.Up;
            else if (maxDot == dotDown)
                return Direction.Down;
            else if (maxDot == dotRight)
                return Direction.Right;
            else
                return Direction.Left;
        }

        public static FourDirectionalRotation GetFreeFourDirectionalRotationForWorld(Quaternion rotation)
        {
            // 获取旋转后的 forward 方向，只在水平面上考虑
            var forward = rotation * Vector3.forward;
            forward.y = 0;
            forward.Normalize();

            // atan2 得到角度（范围 -180 ~ 180）
            var angle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;

            // 转为 0~360
            if (angle < 0) angle += 360f;

            // 每 90° 一个方向，四舍五入到最近的整数
            var index = Mathf.RoundToInt(angle / 90f) % 4;

            return (FourDirectionalRotation)index;
        }

        public static FourDirectionalRotation GetEdgeFourDirectionalRotationForWorld(Quaternion rotation)
        {

            var forward = rotation * Vector3.forward;

            forward.y = 0;
            forward.Normalize();

            var dotUp = Vector3.Dot(forward, dirUp);
            var dotDown = Vector3.Dot(forward, dirDown);
            var dotRight = Vector3.Dot(forward, dirRight);
            var dotLeft = Vector3.Dot(forward, dirLeft);

            float maxDot = Mathf.Max(dotUp, dotDown, dotRight, dotLeft);

            if (maxDot == dotUp)
                return FourDirectionalRotation.East;
            else if (maxDot == dotDown)
                return FourDirectionalRotation.West;
            else if (maxDot == dotRight)
                return FourDirectionalRotation.South;
            else
                return FourDirectionalRotation.North;
        }

        public static Direction GetDirection(Vector3 A, Vector3 B)
        {
            var diff = A - B;
            diff.y = 0;

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.z))
            {
                return diff.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return diff.z > 0 ? Direction.Up : Direction.Down;
            }
        }

        //public static Orientation GetOrientation(Transform t)
        //{
        //    var forward = t.forward;
        //    var dir = new Vector2(forward.x, forward.z).normalized;

        //    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        //    {
        //        return Orientation.Vertical;
        //    }
        //    else
        //    {
        //        return Orientation.Horizontal;
        //    }
        //}
    }
}
