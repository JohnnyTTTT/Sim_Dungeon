using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class EntityManager<K, V>:MonoBehaviour
    {
        protected Dictionary<K, V> map = new Dictionary<K, V>();
        public abstract void Regist(V edgeEntity);
    }
}
