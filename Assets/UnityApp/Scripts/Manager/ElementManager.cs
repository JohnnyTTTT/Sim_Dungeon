using DungeonArchitect;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class ElementManager : MonoBehaviour
    {
        [Title("Titles and Headers")]
        public bool drawGizmos;
    }
    public abstract class ElementManager<V> : ElementManager where V : Element
    {
        public Dictionary<Vector2Int, V> map = new Dictionary<Vector2Int, V>();

        private void OnDestroy()
        {
            map.Clear();
        }

        public V GetElement(Vector2Int coord)
        {
            if (map.TryGetValue(coord, out var data))
            {
                return data;
            }
            return null;
        }

        public virtual V GetElement(Vector3 worldPosition)
        {
            var coord = CoordUtility.WorldPositionToLargeCoord(worldPosition);
            return GetElement(coord);
        }

        public  IEnumerable<V> GetAllElements()
        {
            return map.Values;
        }
    }

}
