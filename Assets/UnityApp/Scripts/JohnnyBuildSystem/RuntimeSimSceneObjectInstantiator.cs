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
            if (template.TryGetComponent<Entity_Edge>(out _))
            {

            }
            if (Application.isPlaying)
            {
                var buildableObject = template.GetComponent<BuildableObject>();
                if (buildableObject != null && buildableObject.TryGetComponent<Entity>(out var entity))
                {

                    entity.Direction = DirectionUtility.GetDirectionForWorld(rotation);
                    var fourDirectionalRotation = DirectionUtility.ToFourDirectionalRotation(entity.Direction);
                    if (buildableObject is BuildableEdgeObject buildableEdgeObject)
                    {
                        switch (entity.Direction)
                        {
                            case Direction.Up:
                                position += new Vector3(-1f, 0f, 0f);
                                break;
                            case Direction.Right:
                                position += new Vector3(0f, 0f, 1f);
                                break;
                            case Direction.Down:
                                position += new Vector3(1f, 0f, 0f);
                                break;
                            case Direction.Left:
                                position += new Vector3(0f, 0f, -1f);
                                break;
                            default:
                                break;
                        }
                        var so = entity.currentSO as BuildableEdgeObjectSO;
                        var coord = DungeonController.Instance.WorldPositionToTileCoord(position);
                        var randomPrefab = RandomUtility.UpdateBuildableObjectSORandomPrefab(coord, so);

                        if (EasyGridBuilderProController.Instance.TryInitializeBuildableEdgeObjectSinglePlacement(position, so, fourDirectionalRotation, out var buildable, randomPrefab))
                        {                          

                            reslut = buildable.gameObject;
                        }
                    }
                    else if (buildableObject is BuildableGridObject buildableGridObject)
                    {
                        var so = entity.currentSO as BuildableGridObjectSO;
                        var coord = DungeonController.Instance.WorldPositionToTileCoord(position);
                        var randomPrefab = RandomUtility.UpdateBuildableObjectSORandomPrefab(coord, so);
                        if (EasyGridBuilderProController.Instance.TryInitializeBuildableGridObjectSinglePlacement(position, so, fourDirectionalRotation, out var buildable, randomPrefab))
                        {
 
                            reslut = buildable.gameObject;
                        }
                    }
                }
                else
                {
                    reslut = InstantiatePrefab(template, position, rotation, scale, parent);
                }

            }
            else
            {
                reslut = InstantiateEditor(template, position, rotation, scale, parent);
            }
            reslut.transform.parent = parent;
            return reslut;
        }
        public GameObject InstantiatePrefab(GameObject template, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            if (template.TryGetComponent<Entity_Edge>(out _))
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
            if (template.TryGetComponent<Entity_Edge>(out _))
            {
                var dir = DirectionUtility.GetDirectionForWorld(rotation);
                switch (dir)
                {
                    case Direction.Up:
                        position += new Vector3(-1f, 0f, 0f);
                        break;
                    case Direction.Right:
                        position += new Vector3(0f, 0f, 1f);
                        break;
                    case Direction.Down:
                        position += new Vector3(1f, 0f, 0f);
                        break;
                    case Direction.Left:
                        position += new Vector3(0f, 0f, -1f);
                        break;
                    default:
                        break;
                }
            }

            var gameObj = PrefabUtility.InstantiatePrefab(template) as GameObject;
            gameObj.transform.SetParent(parent);
            gameObj.transform.position = position;
            gameObj.transform.rotation = rotation;
            gameObj.transform.localScale = scale;

            //if (gameObj.TryGetComponent<BuildableObject>(out var buildableObject))
            //{
            //    buildableObject.SetIsActiveSceneObject(true);
            //}
            return gameObj;
        }
#endif
    }
}
