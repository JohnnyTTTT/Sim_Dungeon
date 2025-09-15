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
        None,
        Hotel,
        HotelRoom,
    }

    public class DataManager_Room : EntityManager<Data_Cell, Room>
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

        [Title("Titles and Headers")]
        public bool drawGizmos;

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

        }

        private void OnGridObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            if (m_CurrentRoomType != RoomType.None)
            {
                var room = new Room();
                room.Init(m_CurrentRoomType.ToString());
                roomList.Add(room);
                var cellDatas = new List<Data_Cell>();
                if (GridManager.Instance.TryGetGridBuiltObjectsManager(out GridBuiltObjectsManager gridBuiltObjectsManager))
                {
                    foreach (var buildableObject in gridBuiltObjectsManager.GetBuiltObjectsList())
                    {
                        var buildableRoom = buildableObject.GetComponent<BuildableRoom>();
                        var cellData = DataManager_Cell.Instance.GetData(buildableObject.transform.position);
                        room.AddCell(cellData);
                        cellDatas.Add(cellData);
                        if (map.ContainsKey(cellData))
                        {
                            map.Remove(cellData);
                        }
                        map.Add(cellData,room);
                        buildableRoom.Hide();
                    }
                }
                DungeonController.Instance.AddEdgeForCells(cellDatas, room);
            }
        }

        public void Init(FlowTilemapCellDatabase cells)
        {
            map.Clear();
            roomList.Clear();
            foreach (var cell in cells)
            {
                if (cell.CellType == FlowTilemapCellType.Floor)
                {
                    var cellData = DataManager_Cell.Instance.GetData(cell);
                    var nodeCoord = cell.NodeCoord;
                    var roomName = "Room - " + nodeCoord.ToVector2().ToString();
                    var room = roomList.Where(x => x.name == roomName).FirstOrDefault();
                    if (room == null)
                    {
                        room = new Room();
                        room.Init(roomName);
                        roomList.Add(room);
                    }
                    room.AddCell(cellData);
                    map.Add(cellData, room);
                }
            }
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
