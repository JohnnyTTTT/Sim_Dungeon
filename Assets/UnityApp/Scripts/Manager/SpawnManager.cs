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

        [Title("Global Settings")]
        public Transform m_SpawnRoot;
        public List<Entity> spwanedEntity = new List<Entity>();

        [Title("Easy GridBuilder Pro Settings")]
        public EasyGridBuilderProXZ m_EasyGridBuilderProSize1;
        public EasyGridBuilderProXZ m_EasyGridBuilderProSize2;


        [Title("Default BuildableGridObjectSO")]
        public BuildableGridObjectSO defaultAreaExpand;
        public BuildableGridObjectSO defaultGround;
        public BuildableEdgeObjectSO defaultWall;
        public BuildableCornerObjectSO defaultCorner;
        public BuildableFreeObjectSO defaultDoor;
        public Material defaultSectionMaterial;

        [Title("Default BaseModels")]
        public GameObject groundBase;
        public GameObject CeilingBase;
        public GameObject WallBase;

        [Title("spawnRules")]
        public SpawnRulee[] spawnRules;

        private GridManager m_GridManager;

        [SerializeField] private List<BuildableGridObject> m_CandidateAreaExpandProxies = new List<BuildableGridObject>();
        [SerializeField] private List<Entity_Wall> m_CreatedBuildableEdgeObject = new List<Entity_Wall>();


        private void Start()
        {
            var staticBindingSet = this.CreateBindingSet();

            staticBindingSet.Build();
            m_GridManager = GridManager.Instance;
            m_GridManager.OnBuildableObjectPlaced += OnBuildableObjectPlaced;
            m_GridManager.OnGridObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            m_GridManager.OnEdgeObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            m_GridManager.OnEdgeObjectBoxPlacementFinalized += OnEdgeObjectBoxPlacementFinalized;
        }

        public void Init()
        {
            var doors = new List<Entity_Door>();
            foreach (var item in spwanedEntity)
            {
                if (item is Entity_Door door)
                {
                    doors.Add(door);
                }
                item.UpdateData();
            }

            foreach (var item in doors)
            {
                item.CutWall();
            }
            Debug.Log($"[-----System-----] : Entities UpdateData - Count <{spwanedEntity.Count}>");
        }

        public void UnInit()
        {
            spwanedEntity.Clear();
        }

        private void OnGridObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            //StartCoroutine(AreaExpand());
        }
        private void OnEdgeObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            StartCoroutine(OnPostEdgeObjectBoxPlacementFinalized());
        }

        private IEnumerator OnPostEdgeObjectBoxPlacementFinalized()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            foreach (var item in m_CreatedBuildableEdgeObject)
            {
                ElementManager_Region.Instance.HandleWallPlacedIncremental(item);
            }
           
            m_CreatedBuildableEdgeObject.Clear();
            //GridManager.Instance.TryGetBuildableEdgeObjectGhost
        }

        private IEnumerator AreaExpand()
        {
            BindingService.MainGameViewModel.IsLandExpandMode = false;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            var cellList = new List<Element_Cell>();
            foreach (var item in m_CandidateAreaExpandProxies)
            {
                var cell = ElementManager_Cell.Instance.GetElement(item.transform.position);
                cellList.Add(cell);
            }

            foreach (var cell in cellList)
            {
                cell.Data.CellType = FlowTilemapCellType.Floor;
                CreateGroundForCellElement(cell);
            }

            var candidateEdges = new List<Element_Edge>();
            foreach (var cell in cellList)
            {
                var leftCell = cell.neighbors[0];
                var leftEdge = cell.leftEdge;
                if (!cellList.Contains(leftCell) && leftEdge.wall == null && (leftCell.Data.CellType == FlowTilemapCellType.Custom || leftCell.region == null))
                {
                    leftEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    candidateEdges.Add(leftEdge);
                }

                var upCell = cell.neighbors[1];
                var upEdge = cell.upEdge;
                if (!cellList.Contains(upCell) && upEdge.wall == null && (upCell.Data.CellType == FlowTilemapCellType.Custom || upCell.region == null))
                {

                    upEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    candidateEdges.Add(upEdge);
                }

                var rightCell = cell.neighbors[2];
                var rightEdge = cell.rightEdge;
                if (!cellList.Contains(rightCell) && rightEdge.wall == null && (rightCell.Data.CellType == FlowTilemapCellType.Custom || rightCell.region == null))
                {
                    rightEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    candidateEdges.Add(rightEdge);
                }

                var downCell = cell.neighbors[3];
                var downEdge = cell.downEdge;
                if (!cellList.Contains(downCell) && downEdge.wall == null && (downCell.Data.CellType == FlowTilemapCellType.Custom || downCell.region == null))
                {
                    downEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    candidateEdges.Add(downEdge);
                }
            }

            foreach (var item in candidateEdges)
            {
                CreateWallForEdgeElement(item);
            }


            for (int i = m_CandidateAreaExpandProxies.Count - 1; i >= 0; i--)
            {
                TryDestroyBuildableGridObject(m_CandidateAreaExpandProxies[i]);
            }

            m_CandidateAreaExpandProxies.Clear();

            InvalidAreaManager.Instance.UpdateMesh();
        }



        private void CreateGroundForCellElement(Element_Cell cell)
        {
            var postion = CoordUtility.TileCoordToWorldPosition(cell.Data.TileCoord);
            var rotation = RandomUtility.GetRandomRotation(cell.Data.TileCoord);
            if (TryInitializeBuildableGridObjectSinglePlacement(postion, rotation, defaultGround, out var obj))
            {
                var entity = obj.GetComponent<Entity_Ground>();
                entity.UpdateData();
            }
        }

        private void CreateWallForEdgeElement(Element_Edge edge)
        {
            var postion = CoordUtility.TileCoordToWorldPosition(edge.Data.EdgeCoord);
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

            if (TryInitializeBuildableEdgeObjectSinglePlacement(postion, rotation, defaultWall, out var obj))
            {
                var entity = obj.GetComponent<Entity_Wall>();
                entity.UpdateData();
            }
        }

        public void CreateWallForCells(List<Element_Cell> cells, Region newRoom)
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
                if (leftCell.region == null || leftCell.region != newRoom)
                {
                    var leftEdge = ElementManager_Cell.Instance.GetElement(coord).verticalEdge;
                    leftEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(leftEdge);
                }

                var upCell = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x, coord.y + 1));
                if (upCell.region == null || upCell.region != newRoom)
                {
                    var upEdge = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x, coord.y + 1)).horizontalEdge;
                    upEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(upEdge);
                }

                var rightCell = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x + 1, coord.y));
                if (rightCell.region == null || rightCell.region != newRoom)
                {
                    var rightEdge = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x + 1, coord.y)).verticalEdge;
                    rightEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(rightEdge);
                }

                var downCell = ElementManager_Cell.Instance.GetElement(new IntVector2(coord.x, coord.y - 1));
                if (downCell.region == null || downCell.region != newRoom)
                {
                    var downEdge = ElementManager_Cell.Instance.GetElement(coord).horizontalEdge;
                    downEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    edges.Add(downEdge);
                }
            }
        }

        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            //easyGridBuilderPro.TryInitializeBuildableEdgeObjectSinglePlacement
            if (DungeonController.Instance.worldDataInited)
            {
                if (buildableObject is BuildableGridObject buildableGridObject)
                {
                    if (buildableObject.TryGetComponent<DevelopToolPanel>(out var areaExpandProxy))
                    {
                        m_CandidateAreaExpandProxies.Add(buildableGridObject);
                    }
                }

                if (buildableObject is BuildableEdgeObject buildableEdgeObject)
                {
                    var entity = buildableEdgeObject.GetComponent<Entity_Wall>();
                    entity.UpdateData();
                    entity.edgeElement.Data.EdgeType = FlowTilemapEdgeType.Wall;
                    m_CreatedBuildableEdgeObject.Add(entity);
                }
            }
            //Debug.Log("Rooms : " + ElementManager_Room.Instance.roomList.Count);
        }



        private void Update()
        {

        }


        public bool TryInitializeBuildableEdgeObjectSinglePlacement(Vector3 worldPosition, Quaternion rotation, BuildableEdgeObjectSO buildableEdgeObjectSO, out BuildableEdgeObject spawnnedBuildableEdgeObject, BuildableObjectSO.RandomPrefabs radomPrefabs = null)
        {
            BindingService.MainGameViewModel.GameMode = GameMode.Structure;
            var fourDirectional = DirectionUtility.GetEdgeFourDirectionalRotationForWorld(rotation);
            if (radomPrefabs == null)
            {
                var coord = CoordUtility.WorldPositionToTileCoord(worldPosition);
                radomPrefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(coord, buildableEdgeObjectSO);
            }

            if (BindingService.MainGameViewModel.ActiveEasyGridBuilderPro.TryInitializeBuildableEdgeObjectSinglePlacement(worldPosition, buildableEdgeObjectSO, fourDirectional, false, true, true, 0, true, out spawnnedBuildableEdgeObject, radomPrefabs, null))
            {
                spawnnedBuildableEdgeObject.transform.parent = m_SpawnRoot;
                return true;
            }
            else
            {
                Debug.LogError($"Try Initialize Edge Error - ObjectOS : <{buildableEdgeObjectSO.objectName}> , Position <{worldPosition}>");
            }

            return false;
        }

        public bool TryInitializeBuildableGridObjectSinglePlacement(Vector3 worldPosition, Quaternion rotation, BuildableGridObjectSO buildableGridObjectSO, out BuildableGridObject buildableGridObject, BuildableObjectSO.RandomPrefabs radomPrefabs = null)
        {
            BindingService.MainGameViewModel.GameMode = GameMode.Structure;
            var fourDirectional = DirectionUtility.GetEdgeFourDirectionalRotationForWorld(rotation);
            if (radomPrefabs == null)
            {
                var coord = CoordUtility.WorldPositionToTileCoord(worldPosition);
                radomPrefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(coord, buildableGridObjectSO);
            }
            if (m_EasyGridBuilderProSize2.TryInitializeBuildableGridObjectSinglePlacement(worldPosition, buildableGridObjectSO,
                fourDirectional, true, true, 0, true, out buildableGridObject, radomPrefabs, null))
            {
                buildableGridObject.transform.parent = m_SpawnRoot;
                return true;
            }
            else
            {
                Debug.LogError($"Place Grid Error - <>");
            }

            return false;
        }

        public bool TryInitializeBuildableCornerObjectSinglePlacement(Vector3 worldPosition, BuildableCornerObjectSO buildableCornerObjectSO, out BuildableCornerObject buildableCornerObject, BuildableObjectSO.RandomPrefabs buildableObjectSORandomPrefab = null)
        {
            if (BindingService.MainGameViewModel.ActiveEasyGridBuilderPro.TryInitializeBuildableCornerObjectSinglePlacement(worldPosition, buildableCornerObjectSO,
                 FourDirectionalRotation.North, EightDirectionalRotation.North, 0f, true, true, 0, true, out buildableCornerObject, buildableObjectSORandomPrefab, null))
            {
                buildableCornerObject.transform.parent = m_SpawnRoot;
                return true;
            }
            else
            {
                Debug.LogError($"Place Corner Error - <>");
            }

            return false;
        }
        public bool TryInitializeBuildableFreeObjectSinglePlacement(Vector3 worldPosition, Quaternion rotation, BuildableFreeObjectSO buildableFreeObjectSO, out BuildableFreeObject buildableObject, BuildableObjectSO.RandomPrefabs buildableObjectSORandomPrefab = null)
        {
            var fourDirectional = DirectionUtility.GetFreeFourDirectionalRotationForWorld(rotation);
            if (BindingService.MainGameViewModel.ActiveEasyGridBuilderPro.TryInitializeBuildableFreeObjectSinglePlacement(worldPosition, buildableFreeObjectSO,
                 fourDirectional, EightDirectionalRotation.North, 0f, Vector3.zero,true, 0, true, out buildableObject, buildableObjectSORandomPrefab, null))
            {
                buildableObject.transform.parent = m_SpawnRoot;
                return true;
            }
            else
            {
                Debug.LogError($"Place Free Error - <>");
            }

            return false;
        }
        public bool TryDestroyBuildableGridObject(BuildableGridObject buildable)
        {
            if (GridManager.Instance.TryGetBuildableObjectDestroyer(out var destroyer))
            {
                if (destroyer.TryDestroyBuildableGridObject(buildable, true))
                {
                    return true;
                }
                else
                {
                    Debug.LogError($"TryDestroyGridObject faild : {buildable}");
                }
            }
            return false;
        }

        public bool TryDestroyBuildableFreeObject(BuildableFreeObject buildable)
        {
            if (GridManager.Instance.TryGetBuildableObjectDestroyer(out var destroyer))
            {
                if (destroyer.TryDestroyBuildableFreeObject(buildable, true))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
