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
    public class Element_LargeCell : ElementData<FlowTilemapCell>
    {
        public Element_LargeCell[] neighbors = new Element_LargeCell[4];
        public Element_Edge[] edges = new Element_Edge[4];
        public Region region;
        public Entity_Ground ground;
        public Entity_Ceiling ceiling;

        public Element_Edge horizontalEdge;
        public Element_Edge verticalEdge;

        public Entity_SubEdge horizontalSubEdge;
        public Entity_SubEdge verticalSubEdge;

        public Vector3 worldPosition;
        public Vector2Int coord;


        public Element_LargeCell(FlowTilemapCell data) : base(data)
        {
            coord = new Vector2Int(data.TileCoord.x, data.TileCoord.y);
            worldPosition = CoordUtility.LargeCellCoordToWorldPosition(coord);
        }

        private FourDirectionalRotation GetEdgeDirection(Vector3 edge)
        {
            var dir = new Vector2(edge.x - worldPosition.x, edge.z - worldPosition.z);

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // X 方向差距更大
            {
                return dir.x > 0 ? FourDirectionalRotation.East : FourDirectionalRotation.West;
            }
            else
            {
                return dir.y > 0 ? FourDirectionalRotation.North : FourDirectionalRotation.South;
            }
        }

        public void DrawGizmos()
        {
            GizmoUnitily.DrawTwoSizeCube(worldPosition, Color.green, true);

            //var origin = worldPosition - new Vector3(2, -0.1f, 2);
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
            GizmoUnitily.DrawLabel(coord, $"{new Vector2Int(Data.TileCoord.x, Data.TileCoord.y)} - {Data.CellType} - {region}");
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

        public override string ToString()
        {
            return $"<{Data.TileCoord.x},{Data.TileCoord.y}> - {Data.CellType}";
        }

    }

    public class ElementManager_LargeCell : ElementManager<Element_LargeCell>
    {
        public static ElementManager_LargeCell Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_LargeCell>();
                }
                return s_Instance;
            }

        }
        private static ElementManager_LargeCell s_Instance;


        public Vector2Int drawGizmosCoord;

        public bool drawAll;

        public void Init(FlowTilemapCellDatabase cells)
        {
            if (Inited) return;
            map.Clear();

            foreach (var cell in cells)
            {
                var element = new Element_LargeCell(cell);
                map[cell.TileCoord.ToVector2Int()] = element;
            }

            Inited = true;
            Debug.Log($"[-----System-----] : DataManager Cell inited , Cell count <{map.Count}>");
        }

        public void PostInit()
        {
            var edgeManager = ElementManager_Edge.Instance;
            foreach (var element in map.Values)
            {
                var coord = element.coord;

                element.edges[0] = edgeManager.GetLeftEdgeFromTileCoord(coord);
                element.edges[1] = edgeManager.GetUpEdgeFromTileCoord(coord);
                element.edges[2] = edgeManager.GetRightEdgeFromTileCoord(coord);
                element.edges[3] = edgeManager.GetDownEdgeFromTileCoord(coord);

                element.neighbors[0] = GetLeftCellFromTileCoord(coord);
                element.neighbors[1] = GetUpCellFromTileCoord(coord);
                element.neighbors[2] = GetRightCellFromTileCoord(coord);
                element.neighbors[3] = GetDownCellFromTileCoord(coord);
            }
        }

        public List<Element_LargeCell> GetAllCells()
        {
            return new List<Element_LargeCell>(map.Values);
        }

        public void UnInit()
        {
            map.Clear();
            Inited = false;
        }

        public Element_LargeCell GetLeftCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.LEFT);
        }

        public Element_LargeCell GetUpCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.UP);
        }

        public Element_LargeCell GetRightCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.RIGHT);
        }

        public Element_LargeCell GetDownCellFromTileCoord(Vector2Int coord)
        {
            return GetElement(coord + DirectionUtility.DOWN);
        }


        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in map)
                {
                    if (drawAll || item.Value.Data.CellType == FlowTilemapCellType.Floor)
                    {
                        item.Value.DrawGizmos();
                    }
                }
            }
        }


    }
}