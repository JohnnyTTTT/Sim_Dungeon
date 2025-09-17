using DungeonArchitect;
using DungeonArchitect.Themeing;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class SimDungeonSceneProvider : DungeonSceneProvider
    {
        Dictionary<string, Queue<GameObject>> pooledObjects = new Dictionary<string, Queue<GameObject>>();
        public override GameObject AddGameObject(GameObjectDungeonThemeItem gameObjectProp, Matrix4x4 transform, IDungeonSceneObjectInstantiator objectInstantiator)
        {
            if (gameObjectProp == null) return null;
            var MeshTemplate = gameObjectProp.Template;
            string NodeId = gameObjectProp.NodeId;

            if (MeshTemplate == null)
            {
                return null;
            }

            GameObject item = null;
            // Try to reuse an object from the pool
            if (pooledObjects.ContainsKey(NodeId) && pooledObjects[NodeId].Count > 0)
            {
                item = pooledObjects[NodeId].Dequeue();
                if (item != null)
                {
                    SetTransform(item.transform, transform);
                }
            }

            if (item == null)
            {
                // Pool is exhausted for this object
                item = BuildGameObject(gameObjectProp, transform, objectInstantiator);
            }

            return item;
        }
    }
}
