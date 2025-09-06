using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
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

        [SerializeField] private Camera m_Camera;
        [SerializeField] private GameObject m_HighlightPrefab;
        [SerializeField] private LayerMask m_GroundMask;

        private GameObject m_CurrentHighlight;
        private Vector3 m_LastCellPosition;
        private List<IntVector2> m_CustomFloors = new List<IntVector2>();
        private void Start()
        {
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
                if (cell != null && cell.CellType == FlowTilemapCellType.Custom )
                {
                    Debug.Log(cell.CustomCellInfo.name);
                    if (m_CurrentHighlight == null)
                    {
                        m_CurrentHighlight = Instantiate(m_HighlightPrefab, cellPos, Quaternion.identity);
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
            if (Mouse.current.leftButton.wasPressedThisFrame && m_CurrentHighlight != null)
            {
                var cell = dungeonModel.GetTilemapCell(m_LastCellPosition);
                Debug.Log(cell.NodeCoord.ToVector2());
                Debug.Log(cell.TileCoord.ToVector2());
                Debug.Log(cell.CellType);
                //m_LastClickedCell = cell.TileCoord;
                dungeon.Build();
            }
        }




        private Vector3 SnapToGrid(Vector3 value)
        {
            var gridSize = dungeonConfig.gridSize;
            var x = Mathf.FloorToInt(value.x / gridSize.x) * gridSize.x + 2f;
            var z = Mathf.FloorToInt(value.z / gridSize.z) * gridSize.z + 2f;
            return new Vector3(x, 0f, z);
        }


        public override void OnPostDungeonLayoutBuild(Dungeon dungeon, DungeonModel mode)
        {
            //if (m_LastClickedCell != null)
            //{
            //    var cell = dungeonModel.Tilemap.Cells.GetCell(m_LastClickedCell.Value.x, m_LastClickedCell.Value.y);
            //    cell.CellType = FlowTilemapCellType.Floor;
            //}
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
