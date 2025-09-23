using DungeonArchitect;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public static class CoordUtility 
    {
        public static Vector2Int IntVector2ToVector2Int(IntVector2 coord)
        {
            return new Vector2Int(coord.x, coord.y);
        }

        public static IntVector2 WorldPositionToTileCoord(Vector3 coord)
        {

            return DungeonController.Instance.dungeonModel.WorldPositionToTilemapCoord(coord);
        }

        public static Vector3 TileCoordToWorldPosition(IntVector2 coord)
        {
            return DungeonController.Instance.gridFlowDungeonQuery.TileCoordToWorldCoord(coord);
        }

    }
}
