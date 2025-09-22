using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Loxodon.Framework.Binding;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    [System.Serializable]
    public class SpawnRulee
    {
        public RoomType roomType;
        public BiomeSO Biome;
    }

    public class CandidateRoomProxies
    {
        public List<RoomSpawnProxy> candidateRoomProxies = new List<RoomSpawnProxy>();
        public List<IntVector2> candidateCoords = new List<IntVector2>();

        public void AddRoomProxy(RoomSpawnProxy roomSpawnProxy)
        {
            candidateRoomProxies.Add(roomSpawnProxy);
            CalculateCandidateRoomEdges();
        }

        public void RemoveRoomProxy(RoomSpawnProxy roomSpawnProxy)
        {
            candidateRoomProxies.Remove(roomSpawnProxy);
            CalculateCandidateRoomEdges();
        }

        public void CalculateCandidateRoomEdges()
        {
            candidateCoords = candidateRoomProxies
                .Select(x => DungeonController.Instance.WorldPositionToTileCoord(x.transform.position))
                .ToList();
            foreach (var item in candidateRoomProxies)
            {
                item.CalculateEdges(candidateCoords);
            }
        }

        public void Confirm()
        {
            var cells = candidateRoomProxies
                .Select(x => ElementManager_Cell.Instance.GetElement(x.transform.position))
                .ToList();

            var room = ElementManager_Room.Instance.CreateSingleCellRoom(cells[0], candidateRoomProxies[0].roomType);

            foreach (var item in cells)
            {
                room.AddCell(item);
            }

            SpawnManager.Instance.CreateWallForCells(cells, room);

            foreach (var item in candidateRoomProxies)
            {
                var buildable = item.GetComponent<BuildableGridObject>();
                EasyGridBuilderProController.Instance.TryDestroyBuildableGridObject(buildable);
            }

            Clear();
        }

        public void Cancel()
        {
            foreach (var item in candidateRoomProxies)
            {
                var buildable = item.GetComponent<BuildableGridObject>();
                EasyGridBuilderProController.Instance.TryDestroyBuildableGridObject(buildable);
            }
            Clear();
        }

        public void Clear()
        {
            candidateRoomProxies.Clear();
            candidateCoords.Clear();
        }
    }


    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<SpawnManager>();
                }
                return s_Instance;
            }

        }
        private static SpawnManager s_Instance;

        public LayerMask m_GroundMask;

        [Title("Default BuildableGridObjectSOs")]
        public BuildableGridObjectSO defaultArea;
        public BuildableFreeObjectSO defaultGround;
        public BuildableFreeObjectSO defaultWall;
        public BuildableFreeObjectSO defaultCeiling;

        [Title("Default BaseModels")]
        public GameObject groundBase;
        public GameObject CeilingBase;
        public GameObject WallBase;

        [Title("spawnRules")]
        public SpawnRulee[] spawnRules;

        public bool IsLandExpand
        {
            get
            {
                return m_IsLandExpand;
            }
            set
            {
                if (m_IsLandExpand != value)
                {
                    m_IsLandExpand = value;
                    var grid = EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize2;
                    var position = grid.transform.position;
                    if (m_IsLandExpand)
                    {
                        grid.transform.position = new Vector3(position.x, 4.05f, position.y);
                        grid.SetDisplayObjectGrid(true);
                        //BindingService.MainGameViewModel.InputActiveBuildableObjectSO = defaultArea;
                    }
                    else
                    {
                        grid.transform.position = new Vector3(position.x, 0.05f, position.y);
                        grid.SetDisplayObjectGrid(false);
                    }
                    Debug.Log(grid.GetIsDisplayObjectGrid());
                }
            }
        }
        private bool m_IsLandExpand;


        private GridManager m_GridManager;

        private void Start()
        {
            var staticBindingSet = this.CreateBindingSet();
            //staticBindingSet.Bind(this).For(v => v.IsLandExpand).ToExpression(() => BindingService.MainGameViewModel.StructureMode == StructureMode.LandExpand).OneWay();
            staticBindingSet.Build();
            m_GridManager = GridManager.Instance;
           
        }


        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            //easyGridBuilderPro.TryInitializeBuildableEdgeObjectSinglePlacement
            if (DungeonController.Instance.worldDataInited)
            {
              
                    if (buildableObject is BuildableEdgeObject buildableEdgeObject)
                    {
                        var entity = buildableEdgeObject.GetComponent<Entity_Edge>();
                    entity.UpdateData();
                    entity.edgeElement.Data.EdgeType = FlowTilemapEdgeType.Wall;
                    //newWalls.Add(entity);
                    }
            }
            //Debug.Log("Rooms : " + ElementManager_Room.Instance.roomList.Count);
        }

        private void Update()
        {

        }


        //private void OnGridObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        //{
        //    if (m_CandidateRoomProxies.Count > 0)
        //    {
        //        var room = ElementManager_Room.Instance.CreateRoom(m_CandidateRoomProxies[0].roomType);

        //        var roomCells = new List<Element_Cell>();
        //        if (GridManager.Instance.TryGetBuildableGridObjectGhost(out var buildableGridObjectGhost)
        //            && buildableGridObjectGhost.TryGetGhostObjectVisualDictionary(out var ghostObjects))
        //        {
        //            foreach (var buildableObject in ghostObjects)
        //            {
        //                var coord = Vector2IntToIntVector2(buildableObject.Key);
        //                var cellData = ElementManager_Cell.Instance.GetElement(coord);
        //                var oldParent = map[coord];
        //                oldParent.RemoveCell(cellData);
        //                room.AddCell(cellData);
        //                map[coord] = room;
        //                roomCells.Add(cellData);
        //            }
        //            m_CandidateRoomProxies = RoomType.Undefined;
        //            StartCoroutine(AddEdgeForCells(roomCells, room));
        //        }
        //    }
        //}

        public void CreateWallForCells(List<Element_Cell> cells, Room newRoom)
        {
            // 在一连续的单元格集合里找出位于边缘的格子
            var cellCoords = cells.Select(x => x.Data.TileCoord);
            var edgeCells = new List<Element_Cell>();
            foreach (var cell in cells)
            {
                foreach (var dir in DirectionUtility.CardinalDirections)
                {
                    var neighbor = cell.Data.TileCoord + dir;
                    if (!cellCoords.Contains(neighbor))
                    {
                        edgeCells.Add(cell);
                        break;
                    }
                }
            }

            foreach (var cell in edgeCells)
            {
                var coord = cell.Data.TileCoord;

                var edges = new List<Element_Edge>();

                var leftCell = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x - 1, coord.y));
                if (leftCell.room == null || leftCell.room != newRoom)
                {
                    var leftEdge = ElementManager_Cell.Instance.GetElement(coord).verticalEdge;
                    leftEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(leftEdge);
                }

                var upCell = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x, coord.y + 1));
                if (upCell.room == null || upCell.room != newRoom)
                {
                    var upEdge = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x, coord.y + 1)).horizontalEdge;
                    upEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(upEdge);
                }

                var rightCell = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x + 1, coord.y));
                if (rightCell.room == null || rightCell.room != newRoom)
                {
                    var rightEdge = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x + 1, coord.y)).verticalEdge;
                    rightEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(rightEdge);
                }

                var downCell = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x, coord.y - 1));
                if (downCell.room == null || downCell.room != newRoom)
                {
                    var downEdge = ElementManager_Cell.Instance.GetElement(coord).horizontalEdge;
                    downEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(downEdge);
                }
            }
        }




    }
}
