using DungeonArchitect.Flow.Domains.Tilemap;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class DataManager_Edge : EntityManager<FlowTilemapEdge,Data_Edge>
    {
        public static DataManager_Edge Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<DataManager_Edge>();
                }
                return s_Instance;
            }

        }
        private static DataManager_Edge s_Instance;

        public void Init(FlowTilemapEdgeDatabase edges)
        {
            map.Clear();
            foreach (var edge in edges)
            {
                var data = new Data_Edge(edge);
            }
        }

    }
}
