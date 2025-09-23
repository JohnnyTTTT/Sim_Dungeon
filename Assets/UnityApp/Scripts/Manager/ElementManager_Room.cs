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

    public class ElementManager_Room : MonoBehaviour
    {
        public static ElementManager_Room Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<ElementManager_Room>();
                }
                return s_Instance;
            }
        }
        private static ElementManager_Room s_Instance;

        private static int s_RommID;

        public List<Room> roomList = new List<Room>();
        public bool drawGizmos;

        private void Start()
        {
            //GridManager.Instance.OnGridObjectBoxPlacementStarted += OnGridObjectBoxPlacementStarted;
            //GridManager.Instance.OnGridObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            //GridManager.Instance.OnGridObjectBoxPlacementUpdated += OnGridObjectBoxPlacementUpdated;

        }

        public Room CreateRoom(RoomType roomType)
        {
            var room = new Room();
            room.Init($"{roomType} - {s_RommID}", roomType);
            roomList.Add(room);
            s_RommID++;
            return room;
        }

        public void DestroyArea(Room room)
        {
            room.Clear();
            roomList.Remove(room);
        }

        public void Init(FlowTilemapCellDatabase cells)
        {
            roomList.Clear();
            var newRooms = new Dictionary<IntVector2, Room>();
            //foreach (var cell in cells)
            //{
            //    if (cell.CellType == FlowTilemapCellType.Floor)
            //    {
            //        var cellData = ElementManager_Cell.Instance.GetElement(cell.TileCoord);
            //        var nodeCoord = cell.NodeCoord;
            //        if (!newRooms.TryGetValue(nodeCoord, out var room))
            //        {
            //            room = CreateRoom(RoomType.OriginaCave);
            //            newRooms[nodeCoord] = room;
            //        }
            //        room.AddCell(cellData);
            //    }
            //}
        }

        public void UnInit()
        {
            roomList.Clear();
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
