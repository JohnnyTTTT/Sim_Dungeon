using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Element_Edge : ElementData<FlowTilemapEdge>
    {
        public event Action<Element_Edge> OnWallEntityAdded;

        //public Element_LargeCell[] adjacentLargeCells = new Element_LargeCell[2];

        //public Element_Edge[] Neighbors = new Element_Edge[6];

        //public Element_SmallCell[] containedSmallCells = new Element_SmallCell[3];

        private Entity_Wall m_WallEntity;
        public Entity_Door door;

        public List<Entity_Corner> corners = new List<Entity_Corner>();
        public Vector3 worldPosition;
        public Vector2Int coord;

        public bool isRim;

        public Element_Edge(FlowTilemapEdge data) : base(data)
        {
            coord = new Vector2Int(data.EdgeCoord.x, data.EdgeCoord.y);
            if (data.HorizontalEdge)
            {
                worldPosition = CoordUtility.LargeCoordToWorldPosition(coord) + new Vector3(0f, 0f, -1f);
            }
            else
            {
                worldPosition = CoordUtility.LargeCoordToWorldPosition(coord) + Vector3.left;
            }

        }

        public void SetWallEntity(Entity_Wall wall)
        {
            m_WallEntity = wall;
            //DungeonController.Instance.disablerController_SmallCell.AddDisablerCells(containedSmallCells);
            //foreach (var item in containedSmallCells)
            //{
            //    item.isBuildingValid = false;
            //}
            OnWallEntityAdded?.Invoke(this);
        }

        public Entity_Wall GetWallEntity()
        {
            return m_WallEntity;
        }

        public void DrawGizmos()
        {
            if (Data.EdgeType == FlowTilemapEdgeType.Fence || Data.EdgeType == FlowTilemapEdgeType.Wall)
            {
                GizmoUnitily.DrawWall(worldPosition, Color.red, Data.HorizontalEdge);
            }
            else
            {
                GizmoUnitily.DrawWall(worldPosition, Color.blue, Data.HorizontalEdge);
            }
            //GizmoUnitily.DrawLabel(worldPosition,coord.ToString());
            if (m_WallEntity != null)
            {
                GizmoUnitily.DrawLine(worldPosition, worldPosition + new Vector3(0f, 2f, 0f), Color.yellow);
            }

        }

        public override string ToString()
        {
            return $"<{Data.EdgeCoord.x},{Data.EdgeCoord.y}> , HorizontalEdge : {Data.HorizontalEdge} , Entity : {m_WallEntity}";
        }

    }
    public class ElementManager_Edge : ElementManager
    {
        public static ElementManager_Edge Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_Edge>();
                }
                return s_Instance;
            }

        }
        private static ElementManager_Edge s_Instance;
        public Dictionary<Vector2Int, Element_Edge> horizontalMap = new Dictionary<Vector2Int, Element_Edge>();
        public Dictionary<Vector2Int, Element_Edge> verticalMap = new Dictionary<Vector2Int, Element_Edge>();

        private void OnDestroy()
        {
            horizontalMap.Clear();
            verticalMap.Clear();
        }

        public void Init(FlowTilemapEdgeDatabase edges)
        {
            horizontalMap.Clear();
            verticalMap.Clear();

            foreach (var edge in edges)
            {
                var data = new Element_Edge(edge);
                if (edge.HorizontalEdge)
                {
                    horizontalMap[edge.EdgeCoord.ToVector2Int()] = data;
                }
                else
                {
                    verticalMap[edge.EdgeCoord.ToVector2Int()] = data;
                }
            }
            Debug.Log($"[-----System-----] : DataManager_Edge inited , HorizontalMap count <{horizontalMap.Count}> - VerticalMap <{verticalMap.Count}>");
        }

        public void PostInit()
        {
            foreach (var kvp in horizontalMap)
            {
                var edge = kvp.Value;
                var isWall = edge.Data.EdgeType != FlowTilemapEdgeType.Empty;
                if (isWall)
                {
                    var containedSmallCells = GetContainedSmallCells(edge);
                    foreach (var item in containedSmallCells)
                    {
                        if (item != null)
                        {
                            item.cellType = FlowTilemapSmallCellType.Wall;
                        }
                    }
                }
            }

            foreach (var kvp in verticalMap)
            {
                var edge = kvp.Value;
                var isWall = edge.Data.EdgeType != FlowTilemapEdgeType.Empty;
                if (isWall)
                {
                    var containedSmallCells = GetContainedSmallCells(edge);
                    foreach (var item in containedSmallCells)
                    {
                        if (item != null)
                        {
                            item.cellType = FlowTilemapSmallCellType.Wall;
                        }
                    }
                }
            }
        }

        public IEnumerable<Element_Edge> GetAllElements()
        {
            return horizontalMap.Values.Concat(verticalMap.Values);
        }

        public Element_Edge GetHorizontal(Vector2Int coord)
        {
            if (horizontalMap.TryGetValue(coord, out var data))
            {
                return data;
            }
            return null;
        }

        public Element_Edge GetHorizontal(Vector3 worldPosition)
        {
            var coord = CoordUtility.WorldPositionToLargeCoord(worldPosition);
            return GetHorizontal(coord);
        }

        public Element_Edge GetVertical(Vector2Int coord)
        {
            if (verticalMap.TryGetValue(coord, out var data))
            {
                return data;
            }
            return null;
        }

        public Element_Edge GetVertical(Vector3 worldPosition)
        {
            var coord = CoordUtility.WorldPositionToLargeCoord(worldPosition);
            return GetVertical(coord);
        }

        public Element_LargeCell[] GetAdjacentEdges(Element_Edge edge)
        {
            var reslut = new Element_LargeCell[2];
            var edgeCoord = edge.coord;
            if (edge.Data.HorizontalEdge)
            {
                var frontCell = ElementManager_LargeCell.Instance.GetElement(edgeCoord);
                reslut[0] = frontCell;
                var backCell = ElementManager_LargeCell.Instance.GetDownCellFromCoord(edgeCoord);
                reslut[1] = backCell;
            }
            else
            {
                var frontCell = ElementManager_LargeCell.Instance.GetLeftCellFromCoord(edgeCoord);
                reslut[0] = frontCell;
                var backCell = ElementManager_LargeCell.Instance.GetElement(edgeCoord);
                reslut[1] = backCell;
            }

            return reslut;
        }

        public Element_Edge[] GetNeighborEdges(Element_Edge edge)
        {
            var reslut = new Element_Edge[6];
            var edgeCoord = edge.coord;
            if (edge.Data.HorizontalEdge)
            {
                var upLeftHorizontal = GetHorizontal(edgeCoord);
                reslut[0] = upLeftHorizontal;

                var upVertical = GetVertical(edgeCoord + DirectionUtility.UP);
                reslut[1] = upVertical;

                var upRightHorizontal = GetHorizontal(edgeCoord + DirectionUtility.UP);
                reslut[2] = upRightHorizontal;

                var downRightHorizontal = GetHorizontal(edgeCoord + DirectionUtility.LEFT + DirectionUtility.UP);
                reslut[3] = downRightHorizontal;

                var downVertical = GetVertical(edgeCoord + DirectionUtility.DOWN);
                reslut[4] = downVertical;

                var downLeftHorizontal = GetHorizontal(edgeCoord + DirectionUtility.LEFT);
                reslut[5] = downLeftHorizontal;
            }
            else
            {
                var leftHorizontal = GetHorizontal(edgeCoord + DirectionUtility.LEFT);
                reslut[0] = leftHorizontal;

                var upLeftVertical = GetVertical(edgeCoord);
                reslut[1] = upLeftVertical;

                var upRightVertical = GetVertical(edgeCoord + DirectionUtility.RIGHT);
                reslut[2] = upRightVertical;

                var rightHorizontal = GetHorizontal(edgeCoord + DirectionUtility.RIGHT);
                reslut[3] = rightHorizontal;

                var downRightVertical = GetVertical(edgeCoord + DirectionUtility.DOWN + DirectionUtility.RIGHT);
                reslut[4] = downRightVertical;

                var downLeftVertical = GetVertical(edgeCoord + DirectionUtility.DOWN);
                reslut[5] = downLeftVertical;
            }
            return reslut;
        }

        public Element_SmallCell[] GetContainedSmallCells(Element_Edge edge)
        {
            var reslut = new Element_SmallCell[3];
            var edgeCoord = edge.coord;
            if (edge.Data.HorizontalEdge)
            {
                var cellMid = ElementManager_SmallCell.Instance.GetElement(edgeCoord);
                reslut[1] = cellMid;

                var cellLeft = ElementManager_SmallCell.Instance.GetLeftCellFromCoord(edgeCoord);
                reslut[0] = cellLeft;

                var cellRight = ElementManager_SmallCell.Instance.GetRightCellFromCoord(edgeCoord);
                reslut[2] = cellRight;
            }
            else
            {
                var cellMid = ElementManager_SmallCell.Instance.GetElement(edgeCoord);
                reslut[1] = cellMid;

                var cellUp = ElementManager_SmallCell.Instance.GetUpCellFromCoord(edgeCoord); ;
                reslut[0] = cellUp;

                var cellDown = ElementManager_SmallCell.Instance.GetDownCellFromCoord(edgeCoord);
                reslut[2] = cellDown;
            }

            return reslut;
        }


        public Element_Edge GetLeftEdgeFromTileCoord(Vector2Int coord)
        {
            return GetVertical(coord);
        }

        public Element_Edge GetUpEdgeFromTileCoord(Vector2Int coord)
        {
            return GetHorizontal(coord + DirectionUtility.UP);
        }

        public Element_Edge GetRightEdgeFromTileCoord(Vector2Int coord)
        {
            return GetVertical(coord + DirectionUtility.RIGHT);
        }

        public Element_Edge GetDownEdgeFromTileCoord(Vector2Int coord)
        {
            return GetHorizontal(coord);
        }



        public void UnInit()
        {
            horizontalMap.Clear();
            verticalMap.Clear();
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in horizontalMap)
                {

                    // if (item.Value.Data.EdgeType == FlowTilemapEdgeType.Wall || item.Value.Data.EdgeType == FlowTilemapEdgeType.Fence)
                    {
                        item.Value.DrawGizmos();
                    }

                }
                foreach (var item in verticalMap)
                {
                    //    if (item.Value.Data.EdgeType == FlowTilemapEdgeType.Wall || item.Value.Data.EdgeType == FlowTilemapEdgeType.Fence)
                    {
                        item.Value.DrawGizmos();
                    }
                }
            }
        }

    }
}
