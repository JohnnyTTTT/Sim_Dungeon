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
using static UnityEngine.EventSystems.EventTrigger;

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
        public EasyGridBuilderProXZ m_EasyGridBuilderPro_SmallCell;
        public EasyGridBuilderProXZ m_EasyGridBuilderPro_LargeCell;


        [Title("Default BuildableGridObjectSO")]
        public BuildableCornerObjectSO defaultFloor;
        public BuildableEdgeObjectSO defaultWall;
        public BuildableCornerObjectSO defaultPillar;
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
            m_GridManager.OnActiveBuildableSOChanged += OnActiveBuildableSOChanged;
            m_GridManager.OnBuildableObjectPlaced += OnBuildableObjectPlaced;
            m_GridManager.OnGridObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            m_GridManager.OnEdgeObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            m_GridManager.OnEdgeObjectBoxPlacementFinalized += OnEdgeObjectBoxPlacementFinalized;
            //m_GridManager.GetActiveEasyGridBuilderPro().GetActiveGridCellData
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

            //foreach (var item in doors)
            //{
            //    item.CutWall();
            //}
            Debug.Log($"[-----System-----] : Entities UpdateData - Count <{spwanedEntity.Count}>");
        }

        public void UnInit()
        {
            spwanedEntity.Clear();
        }

        private void OnDestroy()
        {
            spwanedEntity.Clear();
            m_CandidateAreaExpandProxies.Clear();
            m_CreatedBuildableEdgeObject.Clear();
        }

        private void OnActiveBuildableSOChanged(EasyGridBuilderPro easyGridBuilderPro, BuildableObjectSO buildableObjectSO)
        {

        }

        private void OnGridObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            if (easyGridBuilderPro.TryGetComponent<Entity_Test>(out var entity))
            {
                //if(entity.randomRotation)
            }
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

            var cells = new HashSet<Element_LargeCell>();
            foreach (var entity in m_CreatedBuildableEdgeObject)
            {
                var adjacentLargeCells = ElementManager_Edge.Instance.GetAdjacentEdges(entity.edgeElement);
                foreach (var cell in adjacentLargeCells)
                {
                    cells.Add(cell);
                }
                //ElementManager_Region.Instance.HandleWallPlacedIncremental(item);
            }
            ElementManager_Region.Instance.HandleWallsPlacedIncremental(cells);
            m_CreatedBuildableEdgeObject.Clear();
            //GridManager.Instance.TryGetBuildableEdgeObjectGhost
        }

        private IEnumerator AreaExpand()
        {
            BindingService.MainGameViewModel.IsLandExpandMode = false;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            var cellList = new List<Element_LargeCell>();
            foreach (var item in m_CandidateAreaExpandProxies)
            {
                var cell = ElementManager_LargeCell.Instance.GetElement(item.transform.position);
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
                var leftEdge = ElementManager_Edge.Instance.GetLeftEdgeFromTileCoord(cell.coord);
                if (!cellList.Contains(leftCell) && leftEdge.GetWallEntity() == null && (leftCell.Data.CellType == FlowTilemapCellType.Custom || leftCell.region == null))
                {
                    leftEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    candidateEdges.Add(leftEdge);
                }

                var upCell = cell.neighbors[1];
                var upEdge = ElementManager_Edge.Instance.GetUpEdgeFromTileCoord(cell.coord);
                if (!cellList.Contains(upCell) && upEdge.GetWallEntity() == null && (upCell.Data.CellType == FlowTilemapCellType.Custom || upCell.region == null))
                {

                    upEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    candidateEdges.Add(upEdge);
                }

                var rightCell = cell.neighbors[2];
                var rightEdge = ElementManager_Edge.Instance.GetRightEdgeFromTileCoord(cell.coord);
                if (!cellList.Contains(rightCell) && rightEdge.GetWallEntity() == null && (rightCell.Data.CellType == FlowTilemapCellType.Custom || rightCell.region == null))
                {
                    rightEdge.Data.EdgeType = FlowTilemapEdgeType.Fence;
                    candidateEdges.Add(rightEdge);
                }

                var downCell = cell.neighbors[3];
                var downEdge = ElementManager_Edge.Instance.GetDownEdgeFromTileCoord(cell.coord);
                if (!cellList.Contains(downCell) && downEdge.GetWallEntity() == null && (downCell.Data.CellType == FlowTilemapCellType.Custom || downCell.region == null))
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

        private void CreateGroundForCellElement(Element_LargeCell cell)
        {
            var postion = CoordUtility.LargeCoordToWorldPosition(cell.coord);
            var rotation = RandomUtility.GetRandomRotation(cell.coord);
            //if (TryInitializeBuildableGridObjectSinglePlacement(postion, rotation, defaultGround, out var obj))
            //{
            //    var entity = obj.GetComponent<Entity_Ground>();
            //    entity.UpdateData();
            //}
        }

        private void CreateWallForEdgeElement(Element_Edge edge)
        {
            var postion = edge.worldPosition;
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

            if (TryInitializeBuildableEdgeObjectSinglePlacement(m_EasyGridBuilderPro_LargeCell, postion, rotation, defaultWall, out var obj))
            {
                var entity = obj.GetComponent<Entity_Wall>();
                entity.UpdateData();
            }
        }

        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            buildableObject.transform.position += new Vector3(0f, -0.1f, 0f);
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

                //if (buildableObject is BuildableCornerObject buildableCornerObject)
                //{
                //    var edgeSO = BindingService.MainGameViewModel.ActiveEasyGridBuilderPro.GetActiveBuildableObjectSO() as BuildableEdgeObjectSO;
                //    if (edgeSO != null && !DirectionUtility.HasCornerConnectRightAngleEdges(buildableCornerObject.transform.position))
                //    {
                //        TryDestroyBuildableCornerObject(buildableCornerObject);
                //    }
                //}
            }
            //Debug.Log("Rooms : " + ElementManager_Room.Instance.roomList.Count);
        }

        public bool TryInitializeBuildableEdgeObjectSinglePlacement(EasyGridBuilderPro easyGridBuilderPro, Vector3 worldPosition, Quaternion rotation, BuildableEdgeObjectSO buildableEdgeObjectSO, out BuildableEdgeObject spawnnedBuildableEdgeObject, BuildableObjectSO.RandomPrefabs radomPrefabs = null)
        {
            //BindingService.MainGameViewModel.GameMode = GameMode.Structure;
            var fourDirectional = DirectionUtility.GetEdgeFourDirectionalRotationForWorld(rotation);
            if (radomPrefabs == null)
            {
                var coord = CoordUtility.WorldPositionToLargeCoord(worldPosition);
                radomPrefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(coord, buildableEdgeObjectSO);
            }

            if (easyGridBuilderPro.TryInitializeBuildableEdgeObjectSinglePlacement(worldPosition, buildableEdgeObjectSO, fourDirectional, false, true, true, 0, true, out spawnnedBuildableEdgeObject, radomPrefabs, null))
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

        public bool TryInitializeBuildableGridObjectSinglePlacement(EasyGridBuilderPro easyGridBuilderPro, Vector3 worldPosition, FourDirectionalRotation dir, BuildableGridObjectSO buildableGridObjectSO, out BuildableGridObject buildableGridObject, BuildableObjectSO.RandomPrefabs radomPrefabs = null)
        {
            if (radomPrefabs == null)
            {
                var coord = CoordUtility.WorldPositionToLargeCoord(worldPosition);
                radomPrefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(coord, buildableGridObjectSO);
            }
            if (easyGridBuilderPro.TryInitializeBuildableGridObjectSinglePlacement(worldPosition, buildableGridObjectSO, dir, true, true, 0, true, out buildableGridObject, radomPrefabs, null))
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

        public bool TryInitializeBuildableGridObjectSinglePlacement(EasyGridBuilderPro easyGridBuilderPro, Vector3 worldPosition, Quaternion rotation, BuildableGridObjectSO buildableGridObjectSO, out BuildableGridObject buildableGridObject, BuildableObjectSO.RandomPrefabs radomPrefabs = null)
        {
            var fourDirectional = DirectionUtility.GetEdgeFourDirectionalRotationForWorld(rotation);
            return TryInitializeBuildableGridObjectSinglePlacement(easyGridBuilderPro, worldPosition, fourDirectional, buildableGridObjectSO, out buildableGridObject);
        }

        public bool TryInitializeBuildableCornerObjectSinglePlacement(EasyGridBuilderPro easyGridBuilderPro, Vector3 worldPosition, Quaternion rotation, BuildableCornerObjectSO buildableCornerObjectSO, out BuildableCornerObject buildableCornerObject, BuildableObjectSO.RandomPrefabs buildableObjectSORandomPrefab = null)
        {
            //BindingService.MainGameViewModel.GameMode = GameMode.Placement;
            var fourDirectional = DirectionUtility.GetEdgeFourDirectionalRotationForWorld(rotation);
            if (easyGridBuilderPro.TryInitializeBuildableCornerObjectSinglePlacement(worldPosition, buildableCornerObjectSO,
                 fourDirectional, EightDirectionalRotation.North, 0f, true, true, 0, true, out buildableCornerObject, buildableObjectSORandomPrefab, null))
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

        public bool TryInitializeBuildableFreeObjectSinglePlacement(EasyGridBuilderPro easyGridBuilderPro, Vector3 worldPosition, Quaternion rotation, BuildableFreeObjectSO buildableFreeObjectSO, out BuildableFreeObject buildableObject, BuildableObjectSO.RandomPrefabs buildableObjectSORandomPrefab = null)
        {
            var fourDirectional = DirectionUtility.GetFreeFourDirectionalRotationForWorld(rotation);
            if (easyGridBuilderPro.TryInitializeBuildableFreeObjectSinglePlacement(worldPosition, buildableFreeObjectSO,
                 fourDirectional, EightDirectionalRotation.North, 0f, Vector3.zero, true, 0, true, out buildableObject, buildableObjectSORandomPrefab, null))
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

        public bool TryDestroyBuildableCornerObject(BuildableCornerObject buildable)
        {
            if (GridManager.Instance.TryGetBuildableObjectDestroyer(out var destroyer))
            {
                if (destroyer.TryDestroyBuildableCornerObject(buildable, true))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
