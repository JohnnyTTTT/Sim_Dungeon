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
        public static readonly IntVector2 LEFT = new IntVector2(-1, 0);
        public static readonly IntVector2 UP = new IntVector2(0, 1);
        public static readonly IntVector2 RIGHT = new IntVector2(1, 0);
        public static readonly IntVector2 DOWN = new IntVector2(0, -1);

        public static readonly IntVector2[] CardinalDirections ={
            new IntVector2(1, 0),
            new IntVector2(-1, 0),
            new IntVector2(0, 1),
            new IntVector2(0, -1)};


        public static Vector3 dirUp = Vector3.forward;    // 世界前
        public static Vector3 dirDown = Vector3.back;     // 世界后
        public static Vector3 dirRight = Vector3.right;   // 世界右
        public static Vector3 dirLeft = Vector3.left;     // 世界左
       
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

        public static FourDirectionalRotation ToFourDirectionalRotation(Direction myDir)
        {
            return myDir switch
            {
                Direction.Up => FourDirectionalRotation.East,   // 我的 Up(0°)    → 插件 East(0°)
                Direction.Right => FourDirectionalRotation.South,  // 我的 Right(90°)→ 插件 South(90°)
                Direction.Down => FourDirectionalRotation.West,   // 我的 Down(180°)→ 插件 West(180°)
                Direction.Left => FourDirectionalRotation.North,  // 我的 Left(270°)→ 插件 North(270°)
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
