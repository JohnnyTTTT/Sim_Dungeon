using DungeonArchitect.Flow.Domains.Tilemap;
using UnityEngine;

namespace Johnny.SimDungeon
{
    [System.Serializable]
    public class EdgeEntitly
    {
        public FlowTilemapEdge edge;
        public Building_Edge buildingPart;

        public EdgeEntitly(FlowTilemapEdge edge)
        {
            this.edge = edge;
        }
    }
}
