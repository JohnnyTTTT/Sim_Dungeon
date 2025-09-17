using DungeonArchitect;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class EntityManager : MonoBehaviour
    {
        protected bool Inited;
        [Title("Titles and Headers")]
        public bool drawGizmos;
    }
    public abstract class EntityManager<V> : EntityManager where V : ElementData
    {
        public Dictionary<IntVector2, V> map = new Dictionary<IntVector2, V>();

        public V GetData(IntVector2 coord)
        {
            if (map.TryGetValue(coord, out var data))
            {
                return data;
            }
            return null;
        }

        public V GetData(Vector3 worldPosition)
        {
            var coord = DungeonController.Instance.WorldPositionToTileCoord(worldPosition);
            return GetData(coord);
        }
    }

}
