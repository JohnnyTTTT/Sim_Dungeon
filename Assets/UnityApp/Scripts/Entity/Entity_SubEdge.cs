using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_SubEdge : Entity
    {
        public Entity_Edge parent;
        public Entity_SubEdge relativeEdge;

    }
}
