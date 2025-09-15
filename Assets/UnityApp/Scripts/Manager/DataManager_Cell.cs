using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;
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
        public List<Data_Tile> tiles = new List<Data_Tile>();
        public Vector3 worldPosition;
        //左上右下
        private Entity_Edge[] edgeDatas = new Entity_Edge[4];

        public bool m_ShowGizmo;

        public Data_Cell(FlowTilemapCell data) : base(data)
        {
            worldPosition = DungeonController.Instance.TileCoordToWorldPosition(data.TileCoord);
        }

        public void AddEdge(Entity_Edge entity)
        {
            var direction = GetEdgeDirection(entity.transform.position);
            switch (direction)
            {
                case Direction.Left:
                    edgeDatas[0] = entity;
                    break;
                case Direction.Up:
                    edgeDatas[1] = entity;
                    break;
                case Direction.Right:
                    edgeDatas[2] = entity;
                    break;
                case Direction.Down:
                    edgeDatas[3] = entity;
                    break;
            }
            CalculateSubCells();
        }

        public void RomoveEdge(Entity_Edge entity)
        {
            var direction = GetEdgeDirection(entity.transform.position);
            switch (direction)
            {
                case Direction.Left:
                    edgeDatas[0] = null;
                    break;
                case Direction.Up:
                    edgeDatas[1] = null;
                    break;
                case Direction.Right:
                    edgeDatas[2] = null;
                    break;
                case Direction.Down:
                    edgeDatas[3] = null;
                    break;
            }
            CalculateSubCells();
        }

        private void CalculateSubCells()
        {

            //foreach (var item in subCells)
            //{
            //    item.isEdge = edgeDatas[0] != null && item.direction == Direction.Left;
            //    item.isEdge = edgeDatas[1] != null && item.direction == Direction.Up;
            //    item.isEdge = edgeDatas[2] != null && item.direction == Direction.Right;
            //    item.isEdge = edgeDatas[3] != null && item.direction == Direction.Down;
            //    Debug.Log(item.isEdge);
            //}

        }

        public Entity_Edge[] GetEdgeEntities()
        {
            return edgeDatas;
        }


        private Direction GetEdgeDirection(Vector3 edge)
        {
            var dir = new Vector2(edge.x - worldPosition.x, edge.z - worldPosition.z);

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // X 方向差距更大
            {
                return dir.x > 0 ? Direction.Right : Direction.Left;
            }
            else
            {
                return dir.y > 0 ? Direction.Up : Direction.Down;
            }
        }

        public void DrawGizmos()
        {
            GizmoUnitily.DrawFourSizeCube(worldPosition, Color.green, true);
            foreach (var item in edgeDatas)
            {
                if (item != null)
                {
                    GizmoUnitily.DrawLine(worldPosition, item.transform.position + new Vector3(0f, 1.5f, 0f), Color.gold);
                }

            }
            var origin = worldPosition - new Vector3(2, -0.1f, 2);
            //foreach (var item in subCells)
            //{
            //    var color = Color.gray;
            //    if (item.direction == Direction.Left && item.isEdge)
            //    {
            //        color = Color.red;
            //    }
            //    else if (item.direction == Direction.Up && item.isEdge)
            //    {
            //        color = Color.green;
            //    }
            //    else if (item.direction == Direction.Right && item.isEdge)
            //    {
            //        color = Color.darkRed;
            //    }
            //    else if (item.direction == Direction.Down && item.isEdge)
            //    {
            //        color = Color.darkGreen;
            //    }
            //    var position = new Vector3(item.position.x + 0.5f, 0f, item.position.y + 0.5f);
            //    GizmoUnitily.DrawOneSizeCube(position, item.GizmoColor, true);
            //}
            GizmoUnitily.DrawLabel(Data.TileCoord, new Vector2Int(Data.TileCoord.x, Data.TileCoord.y).ToString());
        }
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
    public class DataManager_Cell : EntityManager<FlowTilemapCell, Data_Cell>
    {
        public static DataManager_Cell Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DataManager_Cell>();
                }
                return s_Instance;
            }

        }
        private static DataManager_Cell s_Instance;

        [Title("Titles and Headers")]
        public bool drawGizmos;
        public Vector2Int drawGizmosCoord;

        public void Init(FlowTilemapCellDatabase cells)
        {
            if (Inited) return;
            map.Clear();
            foreach (var cell in cells)
            {
                if (cell.CellType == FlowTilemapCellType.Floor)
                {
                    var data = new Data_Cell(cell);
                    map.Add(cell, data);
                }
            }
            Inited = true;
        }
        public void UnInit()
        {
            map.Clear();
            Inited = false;
        }
        public Data_Cell GetData(FlowTilemapCell cell)
        {
            if (map.TryGetValue(cell, out var entitly))
            {
                return entitly;
            }
            return null;
        }

        public Data_Cell GetData(IntVector2 coord)
        {
            var cell = DungeonController.Instance.GetCellFromTileCoord(coord);
            return GetData(cell);
        }

        public Data_Cell GetData(Vector3 worldPosition)
        {
            var cell = DungeonController.Instance.GetCellFromWorldPosition(worldPosition);
            return GetData(cell);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                if (drawGizmosCoord.x > -1 && drawGizmosCoord.y > -1)
                {
                    GetData(new IntVector2(drawGizmosCoord.x, drawGizmosCoord.y)).DrawGizmos();
                }
                else
                {
                    foreach (var item in map)
                    {
                        item.Value.DrawGizmos();
                    }
                }

            }
        }


    }
}