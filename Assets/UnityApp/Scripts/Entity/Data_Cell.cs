using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect.Flow.Domains.Tilemap;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Johnny.SimDungeon
{

    public class Data_Cell : ElementData<FlowTilemapCell>
    {
        private static readonly float[] Angles = { 0f, 90f, 180f, 270f };
        public bool randomAngle;
        public Room parentRoom;
        public Vector2Int[,] subCellCoords = new Vector2Int[4, 4];
        public Vector3 worldPosition;
        //вСиосроб
        public List<Entity_Edge> edgeDatas = new List<Entity_Edge>();

        public bool m_ShowGizmo;

        public Data_Cell(FlowTilemapCell data) : base(data)
        {
            worldPosition = DungeonController.Instance.TileCoordToWorldPosition(data.TileCoord);
        }
#if UNITY_EDITOR
        public void DrawGizmos()
        {
            GizmoUnitily.DrawFourSizeCube(worldPosition, Color.green, true);
            foreach (var item in edgeDatas)
            {
                GizmoUnitily.DrawLine(worldPosition, item.transform.position + new Vector3(0f, 1.5f, 0f), Color.gold);
            }
            GizmoUnitily.DrawLabel(Data.TileCoord, new Vector2Int(Data.TileCoord.x, Data.TileCoord.y).ToString());
        }
#endif
        //public override void Init(FlowTilemapCell flowTilemapCell)
        //{
        //    base.Init(flowTilemapCell);
        //    var tileCoord = new Vector2Int(Data.TileCoord.x, Data.TileCoord.y);
        //    name = tileCoord.ToString();
        //    if (randomAngle)
        //    {
        //        var rotation = Quaternion.Euler(0, GetRandomRotation(), 0);
        //        transform.rotation = rotation;
        //    }
        //    for (int x = 0; x < 4; x++)
        //    {
        //        for (int y = 0; y < 4; y++)
        //        {
        //            subCellCoords[x, y] = new Vector2Int(tileCoord.x * 4 + x, tileCoord.y * 4 + y);
        //        }
        //    }
        //    EntityManager_Cell.Instance.Register(this);
        //    registered = true;
        //    //var leftEdge = DungeonController.Instance.GetLeftEdgeFromTileCoord(cell.TileCoord);
        //    //var leftEdgeEntitly = new EdgeEntitly(leftEdge);
        //    //var upEdge = DungeonController.Instance.GetUpEdgeFromTileCoord(cell.TileCoord);
        //    //var upEdgetEntitly = new EdgeEntitly(upEdge);
        //    //var rightEdge = DungeonController.Instance.GetRightEdgeFromTileCoord(cell.TileCoord);
        //    //var rightEdgetEntitly = new EdgeEntitly(rightEdge);
        //    //var downEdge = DungeonController.Instance.GetDownEdgeFromTileCoord(cell.TileCoord);
        //    //var downEdgetEntitly = new EdgeEntitly(downEdge);

        //    //edges = new EdgeEntitly[] { leftEdgeEntitly, upEdgetEntitly, rightEdgetEntitly, downEdgetEntitly };

        //}

        private float GetRandomRotation()
        {
            var index = UnityEngine.Random.Range(0, Angles.Length);
            return Angles[index];
        }


    }
}
