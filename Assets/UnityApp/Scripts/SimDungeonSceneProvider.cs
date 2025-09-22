using DungeonArchitect;
using DungeonArchitect.Themeing;
using DungeonArchitect.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class SimDungeonSceneProvider : DungeonSceneProvider
    {
        Dictionary<string, Queue<GameObject>> pooledObjects = new Dictionary<string, Queue<GameObject>>();

        public override GameObject AddGameObject(GameObjectDungeonThemeItem gameObjectProp, Matrix4x4 matrix, IDungeonSceneObjectInstantiator objectInstantiator)
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
                    SetPooledTransform(item.transform, matrix);
                }
            }

            if (item == null)
            {
                // Pool is exhausted for this object
                item = BuildGameObject(gameObjectProp, matrix, objectInstantiator);
            }

            return item;
        }

        private void SetPooledTransform(Transform target, Matrix4x4 matrix)
        {
            Matrix.DecomposeMatrix(ref matrix, out var position, out var rotation, out var scale);
            //var old
            if (target.TryGetComponent<Entity>(out var entity))
            {
                //entity.SetTransform(position, rotation, scale);
            }
            else
            {
                target.position = position;
                target.rotation = rotation;
                //base.SetTransform(target,matrix);
            }

        }

        //protected void SetTransform(Transform transform, Matrix4x4 matrix)
        //{
        //    Matrix.DecomposeMatrix(ref matrix, out _position, out _rotation, out _scale);

        //    transform.position = _position;
        //    transform.rotation = _rotation;
        //    transform.localScale = _scale;
        //}

        public override void OnDungeonBuildStart()
        {
            base.OnDungeonBuildStart();
            pooledObjects.Clear();
            var items = GameObject.FindObjectsOfType<DungeonSceneProviderData>();
            foreach (var item in items)
            {
                if (item == null || item.externallyManaged) continue;
                if (item.dungeon != this.dungeon) continue;
                if (item.NodeId == null) continue;

                if (!pooledObjects.ContainsKey(item.NodeId))
                {
                    pooledObjects.Add(item.NodeId, new Queue<GameObject>());
                }
                pooledObjects[item.NodeId].Enqueue(item.gameObject);
            }
        }

        public override void OnDungeonBuildStop()
        {
            //// Destroy all unused objects from the pool
            //foreach (var objects in pooledObjects.Values)
            //{
            //    foreach (var obj in objects)
            //    {
            //        if (Application.isPlaying)
            //        {
            //            if (obj.TryGetComponent<Entity>(out var entity))
            //            {
            //                Debug.Log(entity.name, entity);
            //                if (!entity.TryDestroy())
            //                {
            //                    Debug.Log($"Destroy entity error - {entity}", entity);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            DestroyImmediate(obj);
            //        }
            //    }
            //}

            //pooledObjects.Clear();
        }

        public override void InvalidateNodeCache(string NodeId)
        {
            if (pooledObjects.ContainsKey(NodeId))
            {
                foreach (var obj in pooledObjects[NodeId])
                {
                    if (Application.isPlaying)
                    {

                        Destroy(obj);
                    }
                    else
                    {
                        DestroyImmediate(obj);
                    }
                }
                pooledObjects[NodeId].Clear();
            }
        }
    }
}
