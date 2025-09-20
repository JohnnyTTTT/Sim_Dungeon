using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections;
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
        Tavern,
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

        private static int s_RommID;

        public List<Room> roomList = new List<Room>();
        private RoomType m_CurrentRoomType;

        private void Start()
        {
            GridManager.Instance.OnGridObjectBoxPlacementStarted += OnGridObjectBoxPlacementStarted;
            GridManager.Instance.OnGridObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            GridManager.Instance.OnGridObjectBoxPlacementUpdated += OnGridObjectBoxPlacementUpdated;
            GridManager.Instance.OnBuildableObjectPlaced += OnBuildableObjectPlaced;
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
                        //Debug.Log(item.Key, item.Value);
                    }
                }
            }
        }

        private void OnGridObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            if (m_CurrentRoomType != RoomType.Undefined)
            {
                var room = CreateRoom(m_CurrentRoomType);

                var roomCells = new List<Data_Cell>();
                if (GridManager.Instance.TryGetBuildableGridObjectGhost(out var buildableGridObjectGhost)
                    && buildableGridObjectGhost.TryGetGhostObjectVisualDictionary(out var ghostObjects))
                {
                    foreach (var buildableObject in ghostObjects)
                    {
                        var coord = Vector2IntToIntVector2(buildableObject.Key);
                        var cellData = DataManager_Cell.Instance.GetData(coord);
                        var oldParent = map[coord];
                        oldParent.RemoveCell(cellData);
                        room.AddCell(cellData);
                        map[coord] = room;
                        roomCells.Add(cellData);
                    }
                    m_CurrentRoomType = RoomType.Undefined;
                    StartCoroutine(AddEdgeForCells(roomCells, room));
                }
            }
        }

        private IEnumerator AddEdgeForCells(List<Data_Cell> cells, Room room)
        {
            yield return new WaitForEndOfFrame();
            DungeonController.Instance.AddEdgeForCells(cells, room);
        }

        public static IntVector2 Vector2IntToIntVector2(Vector2Int coord)
        {
            return new IntVector2(coord.x, coord.y);
        }
        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            if (m_CurrentRoomType != RoomType.Undefined)
            {
                if (buildableObject.TryGetComponent<BuildableRoom>(out var buildableRoom))
                {
                    buildableRoom.Hide();
                }
            }
        }

        private Room CreateRoom(RoomType roomType, IntVector2? spawnNodeCoord = null)
        {
            var room = new Room();
            room.Init($"{roomType} - {s_RommID}", roomType);
            if (spawnNodeCoord != null)
            {
                room.spawnNodeCoord = spawnNodeCoord.Value;
            }
            if (Application.isPlaying)
            {
                room.biome = SpawnManager.Instance.spawnRulesDic[roomType].Biome;
            }
            roomList.Add(room);
            s_RommID++;
            return room;
        }

        public void CreateSingleCellRoom(Data_Cell cell, RoomType roomType)
        {
            var room = CreateRoom(roomType);
            room.AddCell(cell);
            map[cell.Data.TileCoord] = room;
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

                                roomType = RoomType.OriginaCave;
                                break;
                            case DungeonArchitect.Flow.Impl.GridFlow.GridFlowLayoutNodeRoomType.Room:

                                roomType = RoomType.EmptyRoom;
                                break;
                            case DungeonArchitect.Flow.Impl.GridFlow.GridFlowLayoutNodeRoomType.Corridor:

                                roomType = RoomType.OriginaCave;
                                break;
                            case DungeonArchitect.Flow.Impl.GridFlow.GridFlowLayoutNodeRoomType.Cave:

                                roomType = RoomType.OriginaCave;
                                break;
                            default:
                                break;
                        }
                        room = CreateRoom(roomType, nodeCoord);
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
