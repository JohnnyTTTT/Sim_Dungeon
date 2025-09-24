using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    public class RuntimeSimSceneObjectInstantiator : IDungeonSceneObjectInstantiator
    {
        public GameObject Instantiate(GameObject template, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            GameObject reslut = null;
            if (Application.isPlaying)
            {
                if (template.TryGetComponent<Entity>(out var entity))
                {
                    var buildableObjectSO = entity.buildableObjectSO;
                    entity.Direction = DirectionUtility.GetDirectionForWorld(rotation);
                    if (entity is Entity_Wall)
                    {
                        var so = buildableObjectSO as BuildableEdgeObjectSO;
                        if (so == null)
                        {
                            so = SpawnManager.Instance.defaultWall;
                        }
                        if (SpawnManager.Instance.TryInitializeBuildableEdgeObjectSinglePlacement(position, rotation, so, out var buildable, null))
                        {
                            reslut = buildable.gameObject;
                        }
                    }
                    else if (entity is Entity_Ground)
                    {
                        var so = buildableObjectSO as BuildableGridObjectSO;
                        if (so == null)
                        {
                            so = SpawnManager.Instance.defaultGround;
                        }
                        var coord = CoordUtility.WorldPositionToTileCoord(position);
                        rotation = RandomUtility.GetRandomRotation(coord);
                        if (SpawnManager.Instance.TryInitializeBuildableGridObjectSinglePlacement(position, rotation, so, out var buildable, null))
                        {

                            reslut = buildable.gameObject;
                        }
                    }
                    else if (entity is Entity_Corner)
                    {
                        var so = buildableObjectSO as BuildableCornerObjectSO;
                        if (so == null)
                        {
                            so = SpawnManager.Instance.defaultCorner;
                        }
                        if (SpawnManager.Instance.TryInitializeBuildableCornerObjectSinglePlacement(position, so, out var buildable, null))
                        {
                            reslut = buildable.gameObject;
                        }
                    }
                    else if (entity is Entity_Door)
                    {
                        var so = buildableObjectSO as BuildableFreeObjectSO;
                        if (so == null)
                        {
                            so = SpawnManager.Instance.defaultDoor;
                        }
                        if (SpawnManager.Instance.TryInitializeBuildableFreeObjectSinglePlacement(position, rotation, so, out var buildable, null))
                        {
                            reslut = buildable.gameObject;
                        }
                    }

                }
                else
                {
                    //reslut = InstantiatePrefab(template, position, rotation, scale, parent);
                }
                SpawnManager.Instance.spwanedEntity.Add(reslut.GetComponent<Entity>());
            }
            //Editor
            else
            {

                reslut = InstantiateEditor(template, position, rotation, scale, parent);
            }
            reslut.transform.parent = parent;
            return reslut;
        }
        public GameObject InstantiatePrefab(GameObject template, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            if (template.TryGetComponent<Entity_Wall>(out _))
            {
                var dir = DirectionUtility.GetDirectionForWorld(rotation);
                Debug.Log(rotation.eulerAngles);
            }

            var gameObj = Object.Instantiate(template, position, rotation);
            gameObj.transform.SetParent(parent);
            gameObj.transform.localScale = scale;
            return gameObj;
        }

#if UNITY_EDITOR
        public GameObject InstantiateEditor(GameObject template, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            if (template.TryGetComponent<Entity>(out var entity))
            {
                var gameObj = PrefabUtility.InstantiatePrefab(template) as GameObject;
                gameObj.transform.SetParent(parent);
                gameObj.transform.position = position;
                gameObj.transform.rotation = rotation;
                gameObj.transform.localScale = scale;

                SpawnManager.Instance.spwanedEntity.Add(gameObj.GetComponent<Entity>());
                return gameObj;
            }

            //if (gameObj.TryGetComponent<BuildableObject>(out var buildableObject))
            //{
            //    buildableObject.SetIsActiveSceneObject(true);
            //}
            return null;
        }
#endif
    }
}
