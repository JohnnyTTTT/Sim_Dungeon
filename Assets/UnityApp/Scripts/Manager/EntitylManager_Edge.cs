using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class EntitylManager_Edge : EntityManager<FlowTilemapEdge,EdgeEntity>
    {
        public static EntitylManager_Edge Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<EntitylManager_Edge>();
                }
                return s_Instance;
            }

        }
        private static EntitylManager_Edge s_Instance;
        public override void Regist(EdgeEntity edgeEntity)
        {
            map.Add(edgeEntity.Data, edgeEntity);
        }
    }
}
