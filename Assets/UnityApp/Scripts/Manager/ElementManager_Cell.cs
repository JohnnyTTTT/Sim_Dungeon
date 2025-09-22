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
    [System.Serializable]
    public class Element_Cell : ElementData<FlowTilemapCell>
    {
        public Element_Edge leftEdge;
        public Element_Edge upEdge;
        public Element_Edge rightEdge;
        public Element_Edge downEdge;
        public Room room;


        public Entity_Ground ground;
        public Entity_Ceiling ceiling;




        public Element_Edge horizontalEdge;
        public Element_Edge verticalEdge;

        public Entity_SubEdge horizontalSubEdge;
        public Entity_SubEdge verticalSubEdge;

        public List<Element_Tile> tiles = new List<Element_Tile>();
        public Vector3 worldPosition;

    

        public Element_Cell(FlowTilemapCell data) : base(data)
        {
            worldPosition = DungeonController.Instance.TileCoordToWorldPosition(data.TileCoord);
            var coord = data.TileCoord;
            leftEdge = ElementManager_Edge.Instance.GetLeftEdgeFromTileCoord(coord);
            upEdge = ElementManager_Edge.Instance.GetUpEdgeFromTileCoord(coord);
            rightEdge = ElementManager_Edge.Instance.GetRightEdgeFromTileCoord(coord);
            downEdge = ElementManager_Edge.Instance.GetDownEdgeFromTileCoord(coord);
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
            if (leftEdge.wall != null)
            {
                GizmoUnitily.DrawLine(worldPosition, leftEdge.wall.transform.GetChild(0).transform.position + new Vector3(0f, 1.5f, 0f), Color.gold);
            }
            if (upEdge.wall != null)
            {
                GizmoUnitily.DrawLine(worldPosition, upEdge.wall.transform.GetChild(0).transform.position + new Vector3(0f, 1.5f, 0f), Color.red);
            }
            if (rightEdge.wall != null)
            {
                GizmoUnitily.DrawLine(worldPosition, rightEdge.wall.transform.GetChild(0).transform.position + new Vector3(0f, 1.5f, 0f), Color.yellowGreen);
            }
            if (downEdge.wall != null)
            {
                GizmoUnitily.DrawLine(worldPosition, downEdge.wall.transform.GetChild(0).transform.position + new Vector3(0f, 1.5f, 0f), Color.violetRed);
            }
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
            GizmoUnitily.DrawLabel(Data.TileCoord, new Vector2Int(Data.TileCoord.x, Data.TileCoord.y).ToString()+" "+Data.CellType);
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

    public class ElementManager_Cell : ElementManager<Element_Cell>
    {
        public static ElementManager_Cell Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_Cell>();
                }
                return s_Instance;
            }

        }
        private static ElementManager_Cell s_Instance;
        public int tilemapSize;

        public Vector2Int drawGizmosCoord;

        public bool drawAll;

        public void Init(FlowTilemapCellDatabase cells)
        {
            if (Inited) return;

            tilemapSize = DungeonController.Instance.dungeonModel.Tilemap.Width;
            map.Clear();

            foreach (var cell in cells)
            {
                var data = new Element_Cell(cell);
                map[cell.TileCoord] = data;
            }
            Inited = true;
            Debug.Log($"[-----System-----] : DataManager Cell inited , Cell count <{map.Count}>");
        }

        public void UnInit()
        {
            map.Clear();
            Inited = false;
        }


        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                if (drawGizmosCoord.x > -1 && drawGizmosCoord.y > -1)
                {
                    var data = GetElement(new IntVector2(drawGizmosCoord.x, drawGizmosCoord.y));
                    data.DrawGizmos();
                }
                else
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
}