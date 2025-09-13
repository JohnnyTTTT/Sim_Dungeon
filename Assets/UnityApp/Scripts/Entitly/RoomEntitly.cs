using System;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class RoomEntitly : MonoBehaviour
    {
        public void Init(RoomType currentRoomType, Transform parent)
        {
            transform.parent = parent;
        }
    }
}
