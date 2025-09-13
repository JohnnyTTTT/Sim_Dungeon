using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public enum StructureMode
    {
        None,
        CreateSpace,

    }


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

        [SerializeField] private Camera m_Camera;
        [SerializeField] private GameObject m_HighlightPrefab;
        [SerializeField] private LayerMask m_GroundMask;
        [SerializeField] private Transform m_SelectionRoot;

        public GameObject testPrefab;
        public List<GameObject> tests = new List<GameObject>();

        private GameObject m_CurrentHighlight;
        private BuildingPart m_LastHitBuildingPart;
        private HashSet<IntVector2> m_CustomFloors = new HashSet<IntVector2>();
        private List<GameObject> m_CurrentAreaHighlights = new List<GameObject>();


        //Structure
        public StructureMode structureMode = StructureMode.None;
        private List<FlowTilemapCell> m_WillCreateSpaces = new List<FlowTilemapCell>();

        private void Start()
        {
            dungeon.Build();
            GridManager.Instance.OnActiveGridModeChanged += OnActiveGridModeChanged;
            GridManager.Instance.OnActiveEasyGridBuilderProChanged += OnActiveEasyGridBuilderProChanged;
            //m_CustomFloors.Clear();
            //

            //m_GridFlowDungeonBuilder.BuildDungeon(m_DungeonConfig, m_DungeonModel);
        }

        public FlowTilemapCell WorldPositionToCell(Vector3 worldPosition)
        {
            return gridFlowDungeonQuery.WorldCoordToTile(worldPosition);
        }



        private void OnActiveEasyGridBuilderProChanged(EasyGridBuilderPro arg0)
        {

            //Debug.Log("[System] : EasyGridBuilderPro Changed");
            //dungeon.Build();
            //Debug.Log("[System] : Dungeon Builded");
        }

        private void Update()
        {


            if (structureMode == StructureMode.CreateSpace)
            {
                StructureModeTest();

            }
            //UpdateHighlight();
            //CheckClick();
        }

        public void SetCellsTo(List<FlowTilemapCell> cells, FlowTilemapCellType cellType)
        {
            foreach (var item in cells)
            {

            }
        }


        public static readonly IntVector2[] CardinalDirections ={
            new IntVector2(1, 0),
            new IntVector2(-1, 0),
            new IntVector2(0, 1),
            new IntVector2(0, -1)};

        public static List<CellEntity> FindEdgeCells(List<CellEntity> cells)
        {
            var cellCoords = cells.Select(x => x.cell.TileCoord);
            var edgeCells = new List<CellEntity>();

            foreach (var cell in cells)
            {
                foreach (var dir in CardinalDirections)
                {
                    var neighbor = cell.cell.TileCoord + dir;
                    if (!cellCoords.Contains(neighbor))
                    {
                        edgeCells.Add(cell);
                        break;
                    }
                }
            }
            return edgeCells;
        }

        public void AddEdgeForCells(List<CellEntity> cellEntitlies, RoomEntitly roomEntitly)
        {
            var edgeCellEntitlies = FindEdgeCells(cellEntitlies);
            var cellEntitiyManager = CellEntitiyManager.Instance;
            foreach (var cellEntitly in edgeCellEntitlies)
            {
                var neighbourData = GetNeighbourData(cellEntitly.cell);
                for (int i = 0; i < 4; i++)
                {
                    //left
                    var left = neighbourData[0].cell;
                    var edgeLeft = neighbourData[0].edge;
                    if (left.CellType == FlowTilemapCellType.Custom || (left.CellType == FlowTilemapCellType.Floor && cellEntitiyManager.GetCellEntitly(left).room != roomEntitly))
                    {
                        edgeLeft.EdgeType = FlowTilemapEdgeType.Fence;
                    }

                    //up
                    var up = neighbourData[1].cell;
                    var edgeUp = neighbourData[1].edge;
                    if (up.CellType == FlowTilemapCellType.Custom || (up.CellType == FlowTilemapCellType.Floor && cellEntitiyManager.GetCellEntitly(up).room != roomEntitly))
                    {
                        edgeUp.EdgeType = FlowTilemapEdgeType.Fence;
                    }


                    //right
                    var right = neighbourData[2].cell;
                    var edgeRight = neighbourData[2].edge;
                    if (right.CellType == FlowTilemapCellType.Custom || (right.CellType == FlowTilemapCellType.Floor && cellEntitiyManager.GetCellEntitly(right).room != roomEntitly))
                    {
                        edgeRight.EdgeType = FlowTilemapEdgeType.Fence;
                    }

                    //down
                    var down = neighbourData[3].cell;
                    var edgeDown = neighbourData[3].edge;
                    if (down.CellType == FlowTilemapCellType.Custom || (down.CellType == FlowTilemapCellType.Floor && cellEntitiyManager.GetCellEntitly(down).room != roomEntitly))
                    {
                        edgeDown.EdgeType = FlowTilemapEdgeType.Fence;
                    }
                    Debug.Log($"左 : <{left.CellType}> , 上 : <{up.CellType}> , 右 : <{right.CellType}> , 下 : <{down.CellType}>");
                }
            }
            dungeon.ApplyTheme(new RuntimeDungeonSceneObjectInstantiator());
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

                    var neighbourData = GetNeighbourData(cell);
                    for (int i = 0; i < 4; i++)
                    {
                        //left
                        var left = neighbourData[0].cell;
                        var edgeLeft = neighbourData[0].edge;
                        if (left.CellType == FlowTilemapCellType.Custom)
                        {
                            edgeLeft.EdgeType = FlowTilemapEdgeType.Fence;
                        }
                        else if (left.CellType == FlowTilemapCellType.Floor)
                        {
                            edgeLeft.EdgeType = FlowTilemapEdgeType.Empty;
                        }

                        //up
                        var up = neighbourData[1].cell;
                        var edgeUp = neighbourData[1].edge;
                        if (up.CellType == FlowTilemapCellType.Custom)
                        {
                            edgeUp.EdgeType = FlowTilemapEdgeType.Fence;
                        }
                        else if (up.CellType == FlowTilemapCellType.Floor)
                        {
                            edgeUp.EdgeType = FlowTilemapEdgeType.Empty;
                        }

                        //right
                        var right = neighbourData[2].cell;
                        var edgeRight = neighbourData[2].edge;
                        if (right.CellType == FlowTilemapCellType.Custom)
                        {
                            edgeRight.EdgeType = FlowTilemapEdgeType.Fence;
                        }
                        else if (right.CellType == FlowTilemapCellType.Floor)
                        {
                            edgeRight.EdgeType = FlowTilemapEdgeType.Empty;
                        }

                        //down
                        var down = neighbourData[3].cell;
                        var edgeDown = neighbourData[3].edge;
                        if (down.CellType == FlowTilemapCellType.Custom)
                        {
                            edgeDown.EdgeType = FlowTilemapEdgeType.Fence;
                        }
                        else if (down.CellType == FlowTilemapCellType.Floor)
                        {
                            edgeDown.EdgeType = FlowTilemapEdgeType.Empty;
                        }
                        Debug.Log($"左 : <{left.CellType}> , 上 : <{up.CellType}> , 右 : <{right.CellType}> , 下 : <{down.CellType}>");
                    }

                    dungeon.ApplyTheme(new RuntimeDungeonSceneObjectInstantiator());
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



        private NeighborData[] GetNeighbourData(FlowTilemapCell cell)
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

        private void UpdateHighlight()
        {
            var mousePos = Mouse.current.position.ReadValue();
            var ray = m_Camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_GroundMask))
            {
                m_LastHitBuildingPart = hit.transform.GetComponent<BuildingPart>();
                if (m_LastHitBuildingPart != null)
                {
                    Debug.Log(m_LastHitBuildingPart.type);
                }
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
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_GroundMask))
                {
                    var buildingPart = hit.transform.GetComponent<BuildingPart>();
                    if (buildingPart != null && buildingPart != m_LastHitBuildingPart)
                    {
                        m_LastHitBuildingPart = buildingPart;
                    }
                }



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
                if (m_LastHitBuildingPart != null)
                {
                    var position = m_LastHitBuildingPart.transform.position;
                    position = new Vector3(position.x, 0f, position.z);
                    var cell = gridFlowDungeonQuery.WorldCoordToTile(position);
                    var roomCells = gridFlowDungeonQuery.GetLayoutNodeTile(cell.NodeCoord, false);
                    //foreach (var item in roomCells)
                    //{
                    //    var info = buildingItemSpawnListener.GetInfo(item);

                    //    switch (m_LastHitBuildingPart.type)
                    //    {
                    //        case FlowTilemapCellType.Empty:
                    //            break;
                    //        case FlowTilemapCellType.Floor:
                    //            var test = Instantiate(testPrefab);
                    //            test.transform.position = info.floor.transform.position;
                    //            tests.Add(test);
                    //            break;
                    //        case FlowTilemapCellType.Wall:
                    //            foreach (var wall in info.walls)
                    //            {
                    //                var testWall = Instantiate(testPrefab);
                    //                testWall.transform.position = wall.transform.position;
                    //                tests.Add(testWall);
                    //            }
                    //            break;
                    //        case FlowTilemapCellType.Door:
                    //            break;
                    //        case FlowTilemapCellType.Custom:
                    //            break;
                    //        default:
                    //            break;
                    //    }

                    //}

                }
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

                if (m_LastHitBuildingPart != null)
                {
                    var position = m_LastHitBuildingPart.transform.position;
                    position = new Vector3(position.x, 0f, position.z);
                    var cell = dungeonModel.GetTilemapCell(position);
                    var coord = cell.TileCoord;
                    var test1 = Instantiate(testPrefab);
                    var cellPosition = gridFlowDungeonQuery.TileCoordToWorldCoord(coord);
                    test1.transform.position = cellPosition;
                    tests.Add(test1);



                    var neighbourData = GetNeighbourData(cell);
                    for (int i = 0; i < neighbourData.Length; i++)
                    {
                        if (neighbourData[i].edge.EdgeType != FlowTilemapEdgeType.Empty)
                        {
                            var test = Instantiate(testPrefab);
                            switch (i)
                            {
                                case 0:
                                    test.name = "Left";
                                    break;
                                case 1:
                                    test.name = "Up";
                                    break;
                                case 2:
                                    test.name = "Right";
                                    break;
                                case 3:
                                    test.name = "Down";
                                    break;
                            }

                            var edgePosition = gridFlowDungeonQuery.TileCoordToWorldCoord(neighbourData[i].edge.EdgeCoord);
                            test.transform.position = edgePosition;
                            tests.Add(test);
                        }
                    }


                }
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

        private struct NeighborData
        {
            public FlowTilemapCell cell;
            public FlowTilemapEdge edge;
        }





        public FlowTilemapCell[] GetLayoutNodeTile(Vector3 position)
        {
            var cell = dungeonModel.GetTilemapCell(position);
            var cells = gridFlowDungeonQuery.GetLayoutNodeTile(cell.NodeCoord, false);
            return cells;
        }

        public override void OnDungeonDestroyed(Dungeon dungeon)
        {
            CellEntitiyManager.Instance.DestroyCellEntites();
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
            CellEntitiyManager.Instance.Init(dungeonModel.Tilemap.Cells);
            //dungeonModel.Tilemap.ed
            if (m_WillCreateSpaces.Any())
            {
                foreach (var item in m_CustomFloors)
                {
                    var cell = dungeonModel.Tilemap.Cells.GetCell(item.x, item.y);
                    cell.CellType = FlowTilemapCellType.Floor;
                }
            }
            gridFlowMinimap.Initialize();
        }



        public override void OnPostDungeonBuild(Dungeon dungeon, DungeonModel model)
        {
            //CellEntitiesData.Instance.CheckCellEntites();



            //Debug.Log("Dungeon build complete");

            var m = model as GridFlowDungeonModel;
            //if (m_LastClickedCell != null)
            //{
            //    var cell = dungeonModel.Tilemap.Cells.GetCell(m_LastClickedCell.Value.x, m_LastClickedCell.Value.y);
            //    Debug.Log(cell.CellType);
            //}

            //Debug.Log(m);
            //Debug.Log(m.Tilemap);
        }


    }
}
