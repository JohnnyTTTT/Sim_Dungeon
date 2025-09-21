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

    public class ElementManager_Room : ElementManager<Room>
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
        private RoomType m_CurrentRoomType;

        private void Start()
        {
            //GridManager.Instance.OnGridObjectBoxPlacementStarted += OnGridObjectBoxPlacementStarted;
            //GridManager.Instance.OnGridObjectBoxPlacementFinalized += OnGridObjectBoxPlacementFinalized;
            //GridManager.Instance.OnGridObjectBoxPlacementUpdated += OnGridObjectBoxPlacementUpdated;

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

      

        private IEnumerator AddEdgeForCells(List<Element_Cell> cells, Room room)
        {
            yield return new WaitForEndOfFrame();
            SpawnManager.Instance.CreateWallForCells(cells, room);
        }

        public static IntVector2 Vector2IntToIntVector2(Vector2Int coord)
        {
            return new IntVector2(coord.x, coord.y);
        }

        private void OnBuildableObjectPlaced(EasyGridBuilderPro easyGridBuilderPro, BuildableObject buildableObject)
        {
            //if (m_CurrentRoomType != RoomType.Undefined)
            //{
            //    if (buildableObject.TryGetComponent<BuildableRoom>(out var buildableRoom))
            //    {
            //        buildableRoom.Hide();
            //    }
            //}
        }

        public Room CreateRoom(RoomType roomType, IntVector2? spawnNodeCoord = null)
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

        public Room CreateSingleCellRoom(Element_Cell cell, RoomType roomType)
        {
            var room = CreateRoom(roomType);
            room.AddCell(cell);
            map[cell.Data.TileCoord] = room;
            return room;
        }

        public void Init(FlowTilemapCellDatabase cells)
        {
            if (Inited) return;
            map.Clear();
            roomList.Clear();
            //foreach (var cell in cells)
            //{
            //    if (cell.CellType == FlowTilemapCellType.Floor)
            //    {
            //        var cellData = ElementManager_Cell.Instance.GetElement(cell.TileCoord);
            //        var nodeCoord = cell.NodeCoord;
            //        var room = roomList.Where(x => x.spawnNodeCoord == nodeCoord).FirstOrDefault();
            //        if (room == null)
            //        {
            //            var roomTypeDA = DungeonController.Instance.GetRoomType(nodeCoord);
            //            var roomType = RoomType.OriginaCave;
            //            room = CreateRoom(roomType, nodeCoord);
            //        }

            //        room.AddCell(cellData);
            //        map[cell.TileCoord] = room;
            //    }
            //}
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
