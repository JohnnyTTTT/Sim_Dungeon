using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum RoomType
    {
        None,
        Hotel,
        HotelRoom,
    }

    public class RoomEntitiyManager : MonoBehaviour
    {
        public static RoomEntitiyManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<RoomEntitiyManager>();
                }
                return s_Instance;
            }
        }
        private static RoomEntitiyManager s_Instance;

        [SerializeField] private RoomEntitly m_RoomEntitlyPrefab;
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

        }

        private void OnGridObjectBoxPlacementFinalized(EasyGridBuilderPro easyGridBuilderPro)
        {
            if (m_CurrentRoomType != RoomType.None)
            {
                var room = Instantiate(m_RoomEntitlyPrefab);
                room.Init(m_CurrentRoomType, transform);
                var cellEntitlies = new List<CellEntity>();
                if (GridManager.Instance.TryGetGridBuiltObjectsManager(out GridBuiltObjectsManager gridBuiltObjectsManager))
                {
                    foreach (var buildableObject in gridBuiltObjectsManager.GetBuiltObjectsList())
                    {
                        var buildableRoom = buildableObject.GetComponent<BuildableRoom>();
                        var cell = DungeonController.Instance.GetCellFromWorldPosition(buildableObject.transform.position);
                        Debug.Log(CellEntitiyManager.Instance);
                        var cellEntitly = CellEntitiyManager.Instance.GetCellEntitly(cell);
                        cellEntitly.room = room;
                        cellEntitlies.Add(cellEntitly);

                        buildableRoom.Hide();
                    }
                }
                DungeonController.Instance.AddEdgeForCells(cellEntitlies, room);
            }
        }




    }
}
