using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public abstract class EntityManager<K, V> : MonoBehaviour 
    {
        protected bool Inited;
        public Dictionary<K, V> map = new Dictionary<K, V>();
    }

}
