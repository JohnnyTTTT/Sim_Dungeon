using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum RoomType
    {
        Undefined,
        OriginaCave,
        EmptyRoom,
        Hotel,
        HotelRoom,
    }

    public class DataManager_Room : EntityManager<Room>
    {
        public static DataManager_Room Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DataManager_Room>();
                }
                return s_Instance;
            }
        }
        private static DataManager_Room s_Instance;
        public List<Room> roomList = new List<Room>();
        private RoomType m_CurrentRoomType;

        private void Start()
        {
            GridManager.Instance.OnGridObjectBoxPlacementStarted += OnGridObjectBoxPlacementStarted;
            GridManager.Instance.OnGridObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            GridManager.Instance.OnGridObjectBoxPlacementUpdated += OnGridObjectBoxPlacementUpdated;
        }

        public void RegistRoomType(RoomType roomType)
        {
            m_CurrentRoomType = roomType;
        }

        private void OnGridObjectBoxPlacementStarted(EasyGridBuilderPro easyGridBuilderPro, Vector3 boxPlacementStartPosition, GridObjectPlacementType placementType)
        {

        }

        private void OnGridObjectBoxPlacementUpdated(EasyGridBuilderPro easyGridBuilderPro, Vector3 boxPlacementEndPosition)
        {
            if (GridManager.Instance.TryGetBuildableGridObjectGhost(out var a))
            {
                if (a.TryGetGhostObjectVisualDictionary(out var aaa))
                {
                    foreach (var item in aaa)
                    {
                        Debug.Log(item.Key, item.Value);
                    }



                }
            }
        }

        private void OnGridObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            if (m_CurrentRoomType != RoomType.Undefined)
            {
                var room = MakeRoom(m_CurrentRoomType.ToString(), m_CurrentRoomType);

                var roomCells = new List<Data_Cell>();
                if (GridManager.Instance.TryGetBuildableGridObjectGhost(out var buildableGridObjectGhost)
                    && buildableGridObjectGhost.TryGetGhostObjectVisualDictionary(out var ghostObjects))
                {
                    foreach (var buildableObject in ghostObjects)
                    {
                        var buildableRoom = buildableObject.Value.GetComponent<BuildableRoom>();
                        var cellData = DataManager_Cell.Instance.GetData(buildableObject.Value.transform.position);

                        var coord = cellData.Data.TileCoord;
                        var oldParent = map[coord];
                        oldParent.RemoveCell(cellData);
                        room.AddCell(cellData);
                        map[coord] = room;
                        roomCells.Add(cellData);
                        buildableRoom.Hide();
                    }
                    Debug.Log(roomCells.Count);
                    DungeonController.Instance.AddEdgeForCells(roomCells, room);

                    m_CurrentRoomType = RoomType.Undefined;
                }
            }
        }


        private Room MakeRoom(string roomName, RoomType roomType, IntVector2? spawnNodeCoord = null)
        {
            var room = new Room();
            room.Init(roomName, roomType, spawnNodeCoord);
            roomList.Add(room);
            return room;
        }

        public void Init(FlowTilemapCellDatabase cells)
        {
            if (Inited) return;
            map.Clear();
            roomList.Clear();
            foreach (var cell in cells)
            {
                if (cell.CellType == FlowTilemapCellType.Floor)
                {
                    var cellData = DataManager_Cell.Instance.GetData(cell.TileCoord);
                    var nodeCoord = cell.NodeCoord;
                    var room = roomList.Where(x => x.spawnNodeCoord == nodeCoord).FirstOrDefault();
                    if (room == null)
                    {
                        var roomTypeDA = DungeonController.Instance.GetRoomType(nodeCoord);
                        var roomName = "";
                        var roomType = RoomType.Undefined;
                        switch (roomTypeDA)
                        {
                            case DungeonArchitect.Flow.Impl.GridFlow.GridFlowLayoutNodeRoomType.Unknown:
                                roomName = "Unknown" + nodeCoord.ToVector2().ToString();
                                roomType = RoomType.OriginaCave;
                                break;
                            case DungeonArchitect.Flow.Impl.GridFlow.GridFlowLayoutNodeRoomType.Room:
                                roomName = "EmptyRoom" + nodeCoord.ToVector2().ToString();
                                roomType = RoomType.EmptyRoom;
                                break;
                            case DungeonArchitect.Flow.Impl.GridFlow.GridFlowLayoutNodeRoomType.Corridor:
                                roomName = "Corridor" + nodeCoord.ToVector2().ToString();
                                roomType = RoomType.OriginaCave;
                                break;
                            case DungeonArchitect.Flow.Impl.GridFlow.GridFlowLayoutNodeRoomType.Cave:
                                roomName = "OriginaCave" + nodeCoord.ToVector2().ToString();
                                roomType = RoomType.OriginaCave;
                                break;
                            default:
                                break;
                        }
                        room = MakeRoom(roomName, roomType, nodeCoord);
                    }





  
                    room.AddCell(cellData);
                    map[cell.TileCoord] = room;
                }
            }
            Inited = true;
        }

        public void UnInit()
        {
            map.Clear();
            roomList.Clear();
            Inited = false;
        }



        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                foreach (var item in roomList)
                {
                    item.DrawGizmos();
                }
            }

        }


    }
}
