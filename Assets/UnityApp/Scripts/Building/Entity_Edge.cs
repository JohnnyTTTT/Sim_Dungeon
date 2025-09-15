using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Edge : Entity
    {
        public GameObject model;
        public ReplaceableObjectSO replaceableObjectSO;
        public Data_Cell parentCellData;
        public Room parentRoom;

    }
}
