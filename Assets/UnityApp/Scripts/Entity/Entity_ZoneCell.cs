using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum ZoneType
    {
        Undefined,
        Tavern,
        Hostel
    }

    public class Entity_ZoneCell : MonoBehaviour
    {
        public ZoneType zoneType;
    }
}
