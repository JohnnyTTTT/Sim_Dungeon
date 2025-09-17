using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class BuildableRoom : MonoBehaviour
    {
        public RoomType roomType;
        public GameObject proxy;
        private void Start()
        {
            DataManager_Room.Instance.RegistRoomType(roomType);
        }

        public void Hide()
        {
            Debug.Log(222);
            proxy.gameObject.SetActive(false);
        }
    }
}
