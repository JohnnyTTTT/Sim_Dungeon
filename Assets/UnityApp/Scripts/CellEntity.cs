using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect.Flow.Domains.Tilemap;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Johnny.SimDungeon
{
    public class CellEntity : MonoBehaviour
    {
        public FlowTilemapCell cell;
        public Vector2Int cellTileCoord;
        public Vector2Int[,] subCellCoords = new Vector2Int[4, 4];
        public BuildingPart floor;
        public BuildingPart walleast;
        public List<BuildingPart> walls = new List<BuildingPart>();
        public bool m_ShowGizmo;
        public bool canBuildOn => floor != null;
        public void Init(FlowTilemapCell flowTilemapCell, Transform parentTransform,bool showGizmo)
        {
            cell = flowTilemapCell;
            m_ShowGizmo = showGizmo;
            var tileCoord = new Vector2Int(cell.TileCoord.x, cell.TileCoord.y);
            cellTileCoord = tileCoord;
            name = cellTileCoord.ToString();
            transform.position = DungeonController.Instance.gridFlowDungeonQuery.TileCoordToWorldCoord(cell.TileCoord);
            transform.parent = parentTransform;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    subCellCoords[x, y] = new Vector2Int(cellTileCoord.x * 4 + x, cellTileCoord.y * 4 + y);
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_ShowGizmo)
            {
                var center = transform.position + new Vector3(0f, 0.25f, 0f);
                var size = new Vector3(4f, 0.5f, 4f);
                Gizmos.color = canBuildOn ? Color.green : Color.red;
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
