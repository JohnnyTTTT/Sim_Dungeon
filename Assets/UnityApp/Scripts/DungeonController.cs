using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
#if UNITY_EDITOR
using DungeonArchitect.Editors;
#endif
using DungeonArchitect.Flow.Domains.Tilemap;
using DungeonArchitect.Flow.Impl.GridFlow;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class DungeonController : DungeonEventListener
    {
        public static DungeonController Instance
        {
            get
            {
                if (s_Instances == null)
                {
                    s_Instances = FindFirstObjectByType<DungeonController>();
                }
                return s_Instances;
            }

        }
        private static DungeonController s_Instances;
        public Dungeon dungeon;
        public GridFlowDungeonConfig dungeonConfig;
        public GridFlowDungeonModel dungeonModel;
        public GridFlowDungeonBuilder gridFlowDungeonBuilder;
        public PooledDungeonSceneProvider pooledDungeonSceneProvider;
        public GridFlowDungeonQuery gridFlowDungeonQuery;
        public GridFlowMinimap gridFlowMinimap;
        public BuildingItemSpawnListener buildingItemSpawnListener;
        public EasyGridBuilderProController easyGridBuilderProController;

        public List<Entity> entities = new List<Entity>();

        public Camera m_Camera;
        [SerializeField] private GameObject m_HighlightPrefab;
        public LayerMask m_GroundMask;
        [SerializeField] private Transform m_SelectionRoot;

        public GameObject testPrefab;
        public List<GameObject> tests = new List<GameObject>();

        private GameObject m_CurrentHighlight;
        //private BuildingPart m_LastHitBuildingPart;
        private HashSet<IntVector2> m_CustomFloors = new HashSet<IntVector2>();
        private List<GameObject> m_CurrentAreaHighlights = new List<GameObject>();

        private RuntimeSimSceneObjectInstantiator m_RuntimeSimSceneObjectInstantiator;

        //Structure
        public StructureMode structureMode = StructureMode.None;
        private List<FlowTilemapCell> m_WillCreateSpaces = new List<FlowTilemapCell>();
        public float wallDotThreshold;
        public bool worldDataInited;
        public RuntimeSimSceneObjectInstantiator runtimeSimSceneObjectInstantiator;

        private void Start()
        {
            m_RuntimeSimSceneObjectInstantiator = new RuntimeSimSceneObjectInstantiator();

            StartCoroutine(PostStart());

            //m_CustomFloors.Clear();
            //

            //m_GridFlowDungeonBuilder.BuildDungeon(m_DungeonConfig, m_DungeonModel);
        }

        private IEnumerator PostStart()
        {
            yield return new WaitForEndOfFrame();


            GridManager.Instance.OnActiveGridModeChanged += OnActiveGridModeChanged;
            GridManager.Instance.OnActiveEasyGridBuilderProChanged += OnActiveEasyGridBuilderProChanged;

            Debug.Log("[-----System-----] : Dungeon Build Start");
            BindingService.MainGameViewModel.GameMode = GameMode.Loading;
            DestroyDungeon();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            BindingService.MainGameViewModel.GridType = GridType.SizeTwo;
            yield return new WaitForEndOfFrame();
            BuildDungeon();
            yield return new WaitForEndOfFrame();
            worldDataInited = true;
            RandomUtility.SetSeed((int)DungeonController.Instance.dungeon.Config.Seed);
            yield return new WaitForEndOfFrame();
            BindingService.MainGameViewModel.GameMode = GameMode.Default;
            //foreach (var item in entities)
            //{
            //    item.UpdateData();
            //}
            //foreach (var item in entities)
            //{
            //    item.CreateOrUpdateModel();
            //}

            //SpawnManager.Instance.SpawnWorld();
        }

        #region GetCell

        public FlowTilemapCell GetCellFromWorldPosition(Vector3 worldPosition)
        {
            return gridFlowDungeonQuery.WorldCoordToTile(worldPosition);
        }
        public FlowTilemapCell GetCellFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Cells.GetCell(coord.x, coord.y);
        }

        public FlowTilemapCell GetLeftCellFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Cells.GetCell(coord.x - 1, coord.y);
        }
        public FlowTilemapCell GetUpCellFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Cells.GetCell(coord.x, coord.y + 1);
        }
        public FlowTilemapCell GetRightCellFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Cells.GetCell(coord.x + 1, coord.y);
        }
        public FlowTilemapCell GetDownCellFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Cells.GetCell(coord.x, coord.y - 1);
        }
        #endregion

        #region TransformConvert
        public IntVector2 WorldPositionToTileCoord(Vector3 coord)
        {

            return dungeonModel.WorldPositionToTilemapCoord(coord);
        }
        public Vector3 TileCoordToWorldPosition(IntVector2 coord)
        {
            return gridFlowDungeonQuery.TileCoordToWorldCoord(coord);
        }
        #endregion

        public NeighborData[] GetNeighbourData(FlowTilemapCell cell)
        {
            var tilemap = dungeonModel.Tilemap;
            var coord = cell.TileCoord;
            var left = new NeighborData
            {
                cell = tilemap.Cells.GetCell(coord.x - 1, coord.y),
                edge = tilemap.Edges.GetVertical(coord.x, coord.y)
            };

            var right = new NeighborData
            {
                cell = tilemap.Cells.GetCell(coord.x + 1, coord.y),
                edge = tilemap.Edges.GetVertical(coord.x + 1, coord.y)
            };

            var down = new NeighborData
            {
                cell = tilemap.Cells.GetCell(coord.x, coord.y - 1),
                edge = tilemap.Edges.GetHorizontal(coord.x, coord.y)
            };

            var up = new NeighborData
            {
                cell = tilemap.Cells.GetCell(coord.x, coord.y + 1),
                edge = tilemap.Edges.GetHorizontal(coord.x, coord.y + 1)
            };

            return new[] { left, up, right, down };
        }

        public GridFlowLayoutNodeRoomType GetRoomType(IntVector2 coord)
        {
            var layoutNode = gridFlowDungeonQuery.GetLayoutNode(coord);
            var roomType = GridFlowLayoutNodeRoomType.Unknown;

            if (layoutNode != null)
            {
                var domainData = layoutNode.GetDomainData<GridFlowTilemapDomainData>();
                if (domainData != null)
                {
                    roomType = domainData.RoomType;
                }
            }
            return roomType;
        }


        #region GetEdge
        public FlowTilemapEdge GetLeftEdgeFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Edges.GetVertical(coord.x, coord.y);
        }

        public FlowTilemapEdge GetUpEdgeFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Edges.GetHorizontal(coord.x, coord.y + 1);
        }

        public FlowTilemapEdge GetRightEdgeFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Edges.GetVertical(coord.x + 1, coord.y);
        }

        public FlowTilemapEdge GetDownEdgeFromTileCoord(IntVector2 coord)
        {
            return dungeonModel.Tilemap.Edges.GetHorizontal(coord.x, coord.y);
        }
        #endregion

        private void OnActiveEasyGridBuilderProChanged(EasyGridBuilderPro arg0)
        {

            //Debug.Log("[System] : EasyGridBuilderPro Changed");
            //dungeon.Build();
            //Debug.Log("[System] : Dungeon Builded");
        }

        private void Update()
        {


            //if (structureMode == StructureMode.CreateSpace)
            //{
            //    StructureModeTest();

            //}
            //UpdateHighlight();
            //CheckClick();
        }

        public void SetCellsTo(List<FlowTilemapCell> cells, FlowTilemapCellType cellType)
        {
            foreach (var item in cells)
            {

            }
        }




 



        private void StructureModeTest()
        {
            var mousePos = Mouse.current.position.ReadValue();
            var ray = m_Camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_GroundMask))
            {
                var position = new Vector3(hit.point.x, 0f, hit.point.z);

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    var cell = gridFlowDungeonQuery.WorldCoordToTile(position);
                    cell.CellType = FlowTilemapCellType.Floor;

                   
                }
                if (Mouse.current.rightButton.wasReleasedThisFrame)
                {
                    if (tests.Any())
                    {
                        for (int i = tests.Count - 1; i >= 0; i--)
                        {
                            Destroy(tests[i]);
                        }
                        tests.Clear();
                    }
                    var cell = gridFlowDungeonQuery.WorldCoordToTile(position);
                    //var coord = cell.TileCoord;
                    var cells = gridFlowDungeonQuery.GetLayoutNodeTile(cell.NodeCoord, false);
                    foreach (var item in cells)
                    {
                        var test1 = Instantiate(testPrefab);
                        var cellPosition = gridFlowDungeonQuery.TileCoordToWorldCoord(item.TileCoord);
                        test1.transform.position = cellPosition;
                        tests.Add(test1);
                    }

                }
            }
        }

        private void UpdateHighlight()
        {
            var mousePos = Mouse.current.position.ReadValue();
            var ray = m_Camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_GroundMask))
            {
                //m_LastHitBuildingPart = hit.transform.GetComponent<BuildingPart>();
                //if (m_LastHitBuildingPart != null)
                //{
                //    //Debug.Log(m_LastHitBuildingPart.type);
                //}
                //var cellPos = SnapToGrid(hit.point);
                //var cell = dungeonModel.GetTilemapCell(cellPos);
                //if (cell != null && (cell.CellType == FlowTilemapCellType.Custom || cell.CellType == FlowTilemapCellType.Wall))
                //{
                //    cellPos = new Vector3(cellPos.x, 4.01f, cellPos.z);
                //}
                //Debug.Log(cell.CellType + " " + cell.NodeCoord.x + "-" + cell.NodeCoord.y);
                //if (m_CurrentHighlight == null)
                //{
                //    m_CurrentHighlight = Instantiate(m_HighlightPrefab, cellPos, Quaternion.identity, m_SelectionRoot);
                //    m_LastCellPosition = cellPos;
                //}
                //else
                //{
                //    if (cellPos != m_LastCellPosition)
                //    {
                //        m_CurrentHighlight.transform.position = cellPos;
                //        m_LastCellPosition = cellPos;
                //    }
                //}
            }
            else
            {
                if (m_CurrentHighlight != null)
                {
                    Destroy(m_CurrentHighlight);
                    m_CurrentHighlight = null;
                }
            }
        }

        private void CheckClick()
        {
            if (easyGridBuilderProController)
            {
                var mousePos = Mouse.current.position.ReadValue();
                var ray = m_Camera.ScreenPointToRay(mousePos);
                //if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_GroundMask))
                //{
                //    var buildingPart = hit.transform.GetComponent<BuildingPart>();
                //    if (buildingPart != null && buildingPart != m_LastHitBuildingPart)
                //    {
                //        m_LastHitBuildingPart = buildingPart;
                //    }
                //}



            }
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (tests.Any())
                {
                    for (int i = tests.Count - 1; i >= 0; i--)
                    {
                        Destroy(tests[i]);
                    }
                    tests.Clear();
                }
                //if (m_LastHitBuildingPart != null)
                //{
                //    var position = m_LastHitBuildingPart.transform.position;
                //    position = new Vector3(position.x, 0f, position.z);
                //    var cell = gridFlowDungeonQuery.WorldCoordToTile(position);
                //    var roomCells = gridFlowDungeonQuery.GetLayoutNodeTile(cell.NodeCoord, false);
                //    //foreach (var item in roomCells)
                //    //{
                //    //    var info = buildingItemSpawnListener.GetInfo(item);

                //    //    switch (m_LastHitBuildingPart.type)
                //    //    {
                //    //        case FlowTilemapCellType.Empty:
                //    //            break;
                //    //        case FlowTilemapCellType.Floor:
                //    //            var test = Instantiate(testPrefab);
                //    //            test.transform.position = info.floor.transform.position;
                //    //            tests.Add(test);
                //    //            break;
                //    //        case FlowTilemapCellType.Wall:
                //    //            foreach (var wall in info.walls)
                //    //            {
                //    //                var testWall = Instantiate(testPrefab);
                //    //                testWall.transform.position = wall.transform.position;
                //    //                tests.Add(testWall);
                //    //            }
                //    //            break;
                //    //        case FlowTilemapCellType.Door:
                //    //            break;
                //    //        case FlowTilemapCellType.Custom:
                //    //            break;
                //    //        default:
                //    //            break;
                //    //    }

                //    //}

                //}
                //buildingItemSpawnListener.LogInfo(cell);


                //m_CustomFloors.Add(cell.TileCoord);
                //m_LastClickedCell = cell.TileCoord;
                //dungeon.Build();
            }
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (tests.Any())
                {
                    for (int i = tests.Count - 1; i >= 0; i--)
                    {
                        Destroy(tests[i]);
                    }
                    tests.Clear();
                }

                //if (m_LastHitBuildingPart != null)
                //{
                //    var position = m_LastHitBuildingPart.transform.position;
                //    position = new Vector3(position.x, 0f, position.z);
                //    var cell = dungeonModel.GetTilemapCell(position);
                //    var coord = cell.TileCoord;
                //    var test1 = Instantiate(testPrefab);
                //    var cellPosition = gridFlowDungeonQuery.TileCoordToWorldCoord(coord);
                //    test1.transform.position = cellPosition;
                //    tests.Add(test1);



                //    var neighbourData = GetNeighbourData(cell);
                //    for (int i = 0; i < neighbourData.Length; i++)
                //    {
                //        if (neighbourData[i].edge.EdgeType != FlowTilemapEdgeType.Empty)
                //        {
                //            var test = Instantiate(testPrefab);
                //            switch (i)
                //            {
                //                case 0:
                //                    test.name = "Left";
                //                    break;
                //                case 1:
                //                    test.name = "Up";
                //                    break;
                //                case 2:
                //                    test.name = "Right";
                //                    break;
                //                case 3:
                //                    test.name = "Down";
                //                    break;
                //            }

                //            var edgePosition = gridFlowDungeonQuery.TileCoordToWorldCoord(neighbourData[i].edge.EdgeCoord);
                //            test.transform.position = edgePosition;
                //            tests.Add(test);
                //        }
                //    }


                //}
                //if (m_CurrentAreaHighlights.Any())
                //{
                //    for (int i = m_CurrentAreaHighlights.Count - 1; i >= 0; i--)
                //    {
                //        Destroy(m_CurrentAreaHighlights[i]);
                //    }
                //    m_CurrentAreaHighlights.Clear();
                //}

                //var cell = dungeonModel.GetTilemapCell(m_LastCellPosition);
                //var cells = gridFlowDungeonQuery.GetLayoutNodeTile(cell.NodeCoord, false);
                //foreach (var item in cells)
                //{
                //    var position = gridFlowDungeonQuery.TileCoordToWorldCoord(item.TileCoord);
                //    position += new Vector3(0f, 0.01f, 0f);
                //    var current = Instantiate(m_HighlightPrefab, position, Quaternion.identity, m_SelectionRoot);
                //    m_CurrentAreaHighlights.Add(current);
                //}
            }

        }

        public struct NeighborData
        {
            public FlowTilemapCell cell;
            public FlowTilemapEdge edge;
        }


        public void BuildDungeon()
        {
            runtimeSimSceneObjectInstantiator = new RuntimeSimSceneObjectInstantiator();
            dungeon.Build(runtimeSimSceneObjectInstantiator);
            //if (Application.isPlaying)
            //{

            //}
            //else
            //{
            //    dungeon.Build(new EditorDungeonSceneObjectInstantiator());
            //}

        }

        public void DestroyDungeon()
        {
            dungeon.DestroyDungeon();
        }


        public void ApplyTheme()
        {
            dungeon.ApplyTheme(new RuntimeSimSceneObjectInstantiator());
        }

        public FlowTilemapCell[] GetLayoutNodeTile(Vector3 position)
        {
            var cell = dungeonModel.GetTilemapCell(position);
            var cells = gridFlowDungeonQuery.GetLayoutNodeTile(cell.NodeCoord, false);
            return cells;
        }



        private Vector3 SnapToGrid(Vector3 value)
        {
            var gridSize = dungeonConfig.gridSize;
            var x = Mathf.FloorToInt(value.x / gridSize.x) * gridSize.x + 2f;
            var z = Mathf.FloorToInt(value.z / gridSize.z) * gridSize.z + 2f;
            return new Vector3(x, 0.01f, z);
        }

        private void OnActiveGridModeChanged(EasyGridBuilderPro easyGridBuilderPro, GridMode gridMode)
        {
            if (gridMode != GridMode.None)
            {
                //easyGridBuilderProController.Temp_UpdateGrid(dungeonCellDatas.subCellsMap);
            }
        }

        public override void OnPostDungeonLayoutBuild(Dungeon dungeon, DungeonModel model)
        {
            var dungeonModel = model as GridFlowDungeonModel;
            //CellEntitiyManager.Instance.Init(dungeonModel.Tilemap.Cells);
            //dungeonModel.Tilemap.ed
            //gridFlowMinimap.Initialize();
        }

        public override void OnDungeonMarkersEmitted(Dungeon dungeon, DungeonModel model, LevelMarkerList markers)
        {
            var gridFlowDungeonModel = model as GridFlowDungeonModel;

            ElementManager_Edge.Instance.Init(gridFlowDungeonModel.Tilemap.Edges);
            ElementManager_Cell.Instance.Init(gridFlowDungeonModel.Tilemap.Cells);
            ElementManager_Room.Instance.Init(gridFlowDungeonModel.Tilemap.Cells);
            ElementManager_Tile.Instance.Init(EasyGridBuilderProController.Instance.m_EasyGridBuilderProSize1);
            Debug.Log("[-----System-----] : OnDungeonMarkersEmitted");
        }

        public override void OnPostDungeonBuild(Dungeon dungeon, DungeonModel model)
        {
            if (!Application.isPlaying)
            {
                var entities = FindObjectsByType<Entity_Edge>(FindObjectsSortMode.InstanceID);
                foreach (var item in entities)
                {
                    item.UpdateData();
                }
            }

            Debug.Log("[-----System-----] : OnPostDungeonBuild");
        }
        public Transform root;
        public override void OnDungeonDestroyed(Dungeon dungeon)
        {
            ElementManager_Cell.Instance.UnInit();
            //ElementManager_Edge.Instance.UnInit();
            ElementManager_Room.Instance.UnInit();
            ElementManager_Tile.Instance.UnInit();
            //for (int i = root.childCount - 1; i >= 0; i--)
            //{
            //    Destroy(root.GetChild(i));
            //}

        }

        //public override void OnSpawnedManagedObjects(Dungeon dungeon, GameObject[] spawnedManagedObjects, DungeonModel activeModel)
        //{
        //    Debug.Log(spawnedManagedObjects.Count());
        //    Debug.Log("[System] : OnSpawnedManagedObjects");
        //}


    }
}
