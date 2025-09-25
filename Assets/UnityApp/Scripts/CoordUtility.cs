using DungeonArchitect;
using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class CoordUtility
    {
        private static Vector3 Size1GridOriginPosition = new Vector3(-0.5f,0f, -0.5f);

        public static object GetActiveGridCellPosition { get; internal set; }

        public static IntVector2 ToIntVector2(this Vector2Int coord)
        {
            return new IntVector2(coord.x, coord.y);
        }



        public static Vector2Int ToVector2Int(this IntVector2 coord)
        {
            return new Vector2Int(coord.x, coord.y);
        }

        //Small
        public static Vector3 SmallCoordToWorldPosition(Vector2Int coord)
        {
            if (Application.isPlaying)
            {
                return SpawnManager.Instance.m_EasyGridBuilderPro_SmallCell.GetCellWorldPosition(coord, 0);
            }
            else
            {
                return new Vector3(coord.x, 0, coord.y) + Size1GridOriginPosition;
            }
        }

        public static Vector2Int WorldPositionToSmallCoord(Vector3 worldPosition)
        {
            if (Application.isPlaying)
            {
                return SpawnManager.Instance.m_EasyGridBuilderPro_SmallCell.GetActiveGridCellPosition(worldPosition);
            }
            else
            {
                //Debug.Log(new Vector2Int(
                //    Mathf.FloorToInt((worldPosition - Size1GridOriginPosition).x),
                //    Mathf.FloorToInt((worldPosition - Size1GridOriginPosition).z)));
                return new Vector2Int(
                    Mathf.FloorToInt((worldPosition - Size1GridOriginPosition).x),
                    Mathf.FloorToInt((worldPosition - Size1GridOriginPosition).z));
            }
        }


        //Large
        public static Vector3 LargeCoordToWorldPosition(Vector2Int coord)
        {
            return DungeonController.Instance.gridFlowDungeonQuery.TileCoordToWorldCoord(coord.ToIntVector2());
        }

        public static Vector2Int WorldPositionToLargeCoord(Vector3 coord)
        {
            return DungeonController.Instance.dungeonModel.WorldPositionToTilemapCoord(coord).ToVector2Int();
        }
    }
}
