using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect.Flow.Domains.Tilemap;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Johnny.SimDungeon
{
    public class CellEntity : BuildingEntity<FlowTilemapCell>
    {
        public bool randomAngle;
        public RoomEntitly room;
        public Vector2Int[,] subCellCoords = new Vector2Int[4, 4];

        //вСиосроб
        public SubEdgeEntity[] edges;

        public bool m_ShowGizmo;


        public override void Init(FlowTilemapCell flowTilemapCell)
        {
            base.Init(flowTilemapCell);
            var tileCoord = new Vector2Int(Data.TileCoord.x, Data.TileCoord.y);
            name = tileCoord.ToString();
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    subCellCoords[x, y] = new Vector2Int(tileCoord.x * 4 + x, tileCoord.y * 4 + y);
                }
            }

            //var leftEdge = DungeonController.Instance.GetLeftEdgeFromTileCoord(cell.TileCoord);
            //var leftEdgeEntitly = new EdgeEntitly(leftEdge);
            //var upEdge = DungeonController.Instance.GetUpEdgeFromTileCoord(cell.TileCoord);
            //var upEdgetEntitly = new EdgeEntitly(upEdge);
            //var rightEdge = DungeonController.Instance.GetRightEdgeFromTileCoord(cell.TileCoord);
            //var rightEdgetEntitly = new EdgeEntitly(rightEdge);
            //var downEdge = DungeonController.Instance.GetDownEdgeFromTileCoord(cell.TileCoord);
            //var downEdgetEntitly = new EdgeEntitly(downEdge);

            //edges = new EdgeEntitly[] { leftEdgeEntitly, upEdgetEntitly, rightEdgetEntitly, downEdgetEntitly };

        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_ShowGizmo)
            {
                var center = transform.position + new Vector3(0f, 0.25f, 0f);
                var size = new Vector3(4f, 0.5f, 4f);
                Gizmos.color =Color.green;
                Gizmos.DrawWireCube(center, size);

                var origin = transform.position - new Vector3(2, 0, 2);
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        var subCenter = origin + new Vector3(x + 0.5f, 0.125f / 2f, y + 0.5f);
                        var subSize = new Vector3(1f, 0.125f, 1f);
                        Gizmos.color = Color.blue;
                        Gizmos.DrawWireCube(subCenter, subSize);
                        //Handles.Label(subCenter, subCellCoord[x, y].ToString());
                    }
                }
            }

        }
#endif
    }
}
