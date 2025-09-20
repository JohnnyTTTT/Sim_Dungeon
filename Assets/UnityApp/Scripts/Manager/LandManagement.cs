using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Johnny.SimDungeon
{
    public class LandManagement : MonoBehaviour
    {
        public LayerMask m_GroundMask;

        private List<IntVector2> expandCoords = new List<IntVector2>();

        private void Update()
        {
            if (BindingService.MainPanelViewModel.StructureMode != StructureMode.LandExpand) return;
            if (PhysicsUtility.MouseRaycastHit(m_GroundMask, out var raycastHit))
            {
                var position = raycastHit.point;
                position.y = 0f;
                var cellData = DataManager_Cell.Instance.GetData(position);
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    cellData.Data.CellType = FlowTilemapCellType.Floor;


                    var neighbourData = DungeonController.Instance.GetNeighbourData(cellData.Data);


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
                    Debug.Log($"墙 - 左 : <{edgeLeft.EdgeType}> , 上 : <{edgeRight.EdgeType}> , 右 : <{edgeUp.EdgeType}> , 下 : <{edgeDown.EdgeType}>");

                    DataManager_Room.Instance.CreateSingleCellRoom(cellData, RoomType.EmptyRoom);

                    DungeonController.Instance.ApplyTheme();
                    //DungeonController.Instance.BuildDungeon();
                }

            }
        }
    }
}
