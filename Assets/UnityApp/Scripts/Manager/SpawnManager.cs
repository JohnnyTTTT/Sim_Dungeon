using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Loxodon.Framework.Binding;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
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
        public Dictionary<RoomType, SpawnRulee> spawnRulesDic = new Dictionary<RoomType, SpawnRulee>();

        private AreaSpawnProxy m_CurrentAreaSpawnProxy;

        public CandidateRoomProxies m_CandidateRoomGhostProxies = new CandidateRoomProxies();
        public CandidateRoomProxies m_CandidateRoomProxies = new CandidateRoomProxies();

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
  

        private void Start()
        {
            var staticBindingSet = this.CreateBindingSet();
            staticBindingSet.Bind(this).For(v => v.IsLandExpand).ToExpression(() => BindingService.MainGameViewModel.StructureMode == StructureMode.LandExpand).OneWay();
            staticBindingSet.Build();

            foreach (var item in spawnRules)
            {
                spawnRulesDic[item.roomType] = item;
            }
            GridManager.Instance.OnGridObjectBoxPlacementUpdated += OnGridObjectBoxPlacementUpdated;
            GridManager.Instance.OnBuildableObjectPlaced += OnBuildableObjectPlaced;
        }

        private void OnGridObjectBoxPlacementUpdated(EasyGridBuilderPro easyGridBuilderPro, Vector3 boxPlacementEndPosition)
        {

        }

        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            if (buildableObject.TryGetComponent<RoomSpawnProxy>(out var roomSpawnProxy))
            {

                if (!ConfirmPanel.Instance.isActive)
                {
                    ConfirmPanel.Instance.isActive = true;
                    ConfirmPanel.Instance.onConfirm += ConfirmRoomBuild;
                    ConfirmPanel.Instance.onCancel += CancelRoomBuild;
                }

                m_CandidateRoomProxies.AddRoomProxy(roomSpawnProxy);
            }
        }

        private void ConfirmRoomBuild()
        {
            m_CandidateRoomProxies.Confirm();
            m_CandidateRoomGhostProxies.Clear();
            GridManager.Instance.OnGridObjectBoxPlacementUpdated -= OnGridObjectBoxPlacementUpdated;
            GridManager.Instance.OnBuildableObjectPlaced -= OnBuildableObjectPlaced;
        }

        private void CancelRoomBuild()
        {
            m_CandidateRoomGhostProxies.Cancel();
            m_CandidateRoomProxies.Cancel();
            GridManager.Instance.OnGridObjectBoxPlacementUpdated -= OnGridObjectBoxPlacementUpdated;
            GridManager.Instance.OnBuildableObjectPlaced -= OnBuildableObjectPlaced;
        }

        private void Update()
        {
            if (m_CandidateRoomGhostProxies.candidateRoomProxies.Count > 0)
            {
                m_CandidateRoomGhostProxies.CalculateCandidateRoomEdges();
            }

            if (BindingService.MainGameViewModel.StructureMode != StructureMode.LandExpand) return;
            if (PhysicsUtility.MouseRaycastHit(m_GroundMask, out var raycastHit))
            {
                var position = raycastHit.point;
                position.y = 0f;
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    var cellElement = ElementManager_Cell.Instance.GetElement(position);
                    cellElement.Data.CellType = FlowTilemapCellType.Floor;
                    UpdateOrCreateGroundEntity(cellElement);

                    var newRoom = ElementManager_Room.Instance.CreateSingleCellRoom(cellElement, RoomType.EmptyRoom);
                    CreateWallForCells(new List<Element_Cell>() { cellElement }, newRoom);
                }
            }
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

                foreach (var edge in edges)
                {
                    UpdateOrCreateEdgeEntity(edge);
                }

            }
        }

        public void UpdateOrCreateGroundEntity(Element_Cell cell)
        {
            var runtimeSimSceneObjectInstantiator = DungeonController.Instance.runtimeSimSceneObjectInstantiator;
            var postion = DungeonController.Instance.TileCoordToWorldPosition(cell.Data.TileCoord);
            var rotation = Quaternion.identity;
            runtimeSimSceneObjectInstantiator.Instantiate(groundBase, postion, rotation, Vector3.one, null);
            if (cell.ceiling != null)
            {
                cell.ceiling.TryDestroy();
            }
        }

        public void UpdateOrCreateEdgeEntity(Element_Edge edge)
        {
            if (edge.wall != null)
            {
                if (edge.Data.HorizontalEdge)
                {
                    edge.wall.CreateOrUpdateModel();
                }
            }
            else
            {
                var runtimeSimSceneObjectInstantiator = DungeonController.Instance.runtimeSimSceneObjectInstantiator;
                var postion = DungeonController.Instance.TileCoordToWorldPosition(edge.Data.EdgeCoord);
                Quaternion rotation;
                if (edge.Data.HorizontalEdge)
                {
                    postion += new Vector3(0f, 0f, -1f);
                    rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                }
                else
                {
                    postion += new Vector3(-1f, 0f, 0f);
                    rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                runtimeSimSceneObjectInstantiator.Instantiate(WallBase, postion, rotation, Vector3.one, null);
            }
        }

        public void DestroyCeilingEntity(Element_Cell cell)
        {
            cell.ceiling.TryDestroy();
        }

        public void DestroyEdgeEntity(Element_Edge edge)
        {
            edge.wall.TryDestroy();
        }

        public void SetCurrentAreaProxy(AreaSpawnProxy areaSpawnProxy)
        {
            if (m_CurrentAreaSpawnProxy != areaSpawnProxy)
            {
                m_CurrentAreaSpawnProxy = areaSpawnProxy;
            }
        }

        public void CancelCurrentAreaProxy(AreaSpawnProxy areaSpawnProxy)
        {
            if (m_CurrentAreaSpawnProxy == areaSpawnProxy)
            {
                m_CurrentAreaSpawnProxy = null;
            }
        }



        public void CancelCurrentRoomProxy(RoomSpawnProxy roomSpawnProxy)
        {
            //if (m_CandidateRoomProxies == roomSpawnProxy.roomType)
            //{
            //    m_CandidateRoomProxies = RoomType.Undefined;
            //}
        }


    }
}
