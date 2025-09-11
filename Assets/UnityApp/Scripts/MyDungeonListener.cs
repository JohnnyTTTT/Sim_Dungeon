using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class MyDungeonListener : DungeonEventListener
    {
        public Dungeon dungeon;
        public GridFlowDungeonConfig dungeonConfig;
        public GridFlowDungeonModel dungeonModel;
        public GridFlowDungeonBuilder gridFlowDungeonBuilder;
        public GridFlowDungeonQuery gridFlowDungeonQuery;
        public GridFlowMinimap gridFlowMinimap;

        [SerializeField] private Camera m_Camera;
        [SerializeField] private GameObject m_HighlightPrefab;
        [SerializeField] private LayerMask m_GroundMask;
        [SerializeField] private Transform m_SelectionRoot;



        private GameObject m_CurrentHighlight;
        private Vector3 m_LastCellPosition;
        private HashSet<IntVector2> m_CustomFloors = new HashSet<IntVector2>();
        private List<GameObject> m_CurrentAreaHighlights = new List<GameObject>();
        private void Start()
        {
            m_CustomFloors.Clear();
            dungeon.Build();
            //m_GridFlowDungeonBuilder.BuildDungeon(m_DungeonConfig, m_DungeonModel);
        }

        private void Update()
        {
            UpdateHighlight();
            CheckClick();
        }

        private void UpdateHighlight()
        {

            var mousePos = Mouse.current.position.ReadValue();
            var ray = m_Camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_GroundMask))
            {
                var cellPos = SnapToGrid(hit.point);
                var cell = dungeonModel.GetTilemapCell(cellPos);
                if (cell != null && (cell.CellType == FlowTilemapCellType.Custom || cell.CellType == FlowTilemapCellType.Wall))
                {
                    cellPos = new Vector3(cellPos.x, 4.01f, cellPos.z);
                }
                Debug.Log(cell.CellType + " " + cell.NodeCoord.x + "-" + cell.NodeCoord.y);
                if (m_CurrentHighlight == null)
                {
                    m_CurrentHighlight = Instantiate(m_HighlightPrefab, cellPos, Quaternion.identity, m_SelectionRoot);
                    m_LastCellPosition = cellPos;
                }
                else
                {
                    if (cellPos != m_LastCellPosition)
                    {
                        m_CurrentHighlight.transform.position = cellPos;
                        m_LastCellPosition = cellPos;
                    }
                }
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
            if (m_CurrentHighlight != null)
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    var cell = dungeonModel.GetTilemapCell(m_LastCellPosition);
                    Debug.Log(cell.NodeCoord.ToVector2());
                    Debug.Log(cell.TileCoord.ToVector2());
                    Debug.Log(cell.CellType);
                    m_CustomFloors.Add(cell.TileCoord);
                    //m_LastClickedCell = cell.TileCoord;
                    dungeon.Build();
                }
                else if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    if (m_CurrentAreaHighlights.Any())
                    {
                        for (int i = m_CurrentAreaHighlights.Count - 1; i >= 0; i--)
                        {
                            Destroy(m_CurrentAreaHighlights[i]);
                        }
                        m_CurrentAreaHighlights.Clear();
                    }

                    var cell = dungeonModel.GetTilemapCell(m_LastCellPosition);
                    var cells = gridFlowDungeonQuery.GetLayoutNodeTile(cell.NodeCoord, false);
                    foreach (var item in cells)
                    {
                        var position = gridFlowDungeonQuery.TileCoordToWorldCoord(item.TileCoord);
                        position += new Vector3(0f, 0.01f, 0f);
                        var current = Instantiate(m_HighlightPrefab, position, Quaternion.identity, m_SelectionRoot);
                        m_CurrentAreaHighlights.Add(current);
                    }
                }
        }




        private Vector3 SnapToGrid(Vector3 value)
        {
            var gridSize = dungeonConfig.gridSize;
            var x = Mathf.FloorToInt(value.x / gridSize.x) * gridSize.x + 2f;
            var z = Mathf.FloorToInt(value.z / gridSize.z) * gridSize.z + 2f;
            return new Vector3(x, 0.01f, z);
        }


        public override void OnPostDungeonLayoutBuild(Dungeon dungeon, DungeonModel mode)
        {
            if (m_CustomFloors != null && m_CustomFloors.Any())
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
