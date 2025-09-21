using DungeonArchitect;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class ElementManager : MonoBehaviour
    {
        protected bool Inited;
        [Title("Titles and Headers")]
        public bool drawGizmos;
    }
    public abstract class ElementManager<V> : ElementManager where V : Element
    {
        public Dictionary<IntVector2, V> map = new Dictionary<IntVector2, V>();

        public V GetElement(IntVector2 coord)
        {
            if (map.TryGetValue(coord, out var data))
            {
                return data;
            }
            return null;
        }

        public V GetElement(Vector3 worldPosition)
        {
            var coord = DungeonController.Instance.WorldPositionToTileCoord(worldPosition);
            return GetElement(coord);
        }
    }

}
