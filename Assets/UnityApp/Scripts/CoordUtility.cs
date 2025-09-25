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

        public static Vector3 GetSmallCellWorldPosition(Vector2Int coord, int v)
        {
            if (Application.isPlaying)
            {
                return SpawnManager.Instance.m_EasyGridBuilderProSize1.GetCellWorldPosition(coord, 0);
            }
            else
            {
                return new Vector3(coord.x, 0, coord.y) + Size1GridOriginPosition;
            }
        }

        public static Vector2Int GetSmallCellCorrd(Vector3 worldPosition)
        {
            if (Application.isPlaying)
            {
                return SpawnManager.Instance.m_EasyGridBuilderProSize1.GetActiveGridCellPosition(worldPosition);
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


        public static Vector2Int WorldPositionToTileCoord(Vector3 coord)
        {
            return DungeonController.Instance.dungeonModel.WorldPositionToTilemapCoord(coord).ToVector2Int();
        }

        public static Vector3 LargeCellCoordToWorldPosition(Vector2Int coord)
        {
            return DungeonController.Instance.gridFlowDungeonQuery.TileCoordToWorldCoord(coord.ToIntVector2());
        }


    }
}
