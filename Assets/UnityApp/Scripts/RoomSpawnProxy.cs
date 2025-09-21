using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class RoomSpawnProxy : MonoBehaviour
    {
        public RoomType roomType;

        public BuildableGridObject m_BuildableGridObject;
        [SerializeField] private VirtualRoom m_VirtualRoom;


        private void Start()
        {
            if (m_BuildableGridObject.GetIsInstantiatedByGhostObject())
            {
                SpawnManager.Instance.m_CandidateRoomGhostProxies.AddRoomProxy(this);
            }
            else
            {
                SpawnManager.Instance.m_CandidateRoomProxies.AddRoomProxy(this);
            }
        }

        private void OnDestroy()
        {
            if (m_BuildableGridObject.GetIsInstantiatedByGhostObject())
            {
                SpawnManager.Instance.m_CandidateRoomGhostProxies.RemoveRoomProxy(this);
            }
            else
            {
                SpawnManager.Instance.m_CandidateRoomProxies.RemoveRoomProxy(this);
            }
        }


        public void CalculateEdges(List<IntVector2> candidateCoords)
        {
            m_VirtualRoom.CalculateEdges(candidateCoords);
        }


        public void Hide()
        {
            m_VirtualRoom.gameObject.SetActive(false);
        }
    }
}
