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
            proxy.gameObject.SetActive(false);
        }
    }
}
