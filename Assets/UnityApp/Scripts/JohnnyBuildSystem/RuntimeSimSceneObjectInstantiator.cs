using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class RuntimeSimSceneObjectInstantiator : IDungeonSceneObjectInstantiator
    {
        public GameObject Instantiate(GameObject template, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            GameObject reslut = null;
            //var buildableObjects = template.GetComponentsInChildren<BuildableObject>();
            //if (buildableObjects != null && buildableObjects.Length > 0)
            //{
            //    foreach (var buildableObject in buildableObjects)
            //    {
            //        var transform = buildableObject.transform;
            //        if (buildableObject is BuildableGridObject buildableGridObject)
            //        {
            //            var so = buildableGridObject.GetBuildableObjectSO() as BuildableGridObjectSO;
            //            var prefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(so);
            //            var fourDirectionalRotation = DirectionUtility.GetDirectionForWorld(transform.rotation);
            //            EasyGridBuilderProController.Instance.TryInitializeBuildableGridObjectSinglePlacement(transform.position, so,
            //               fourDirectionalRotation, true, true, 0, true, out _, prefabs, null);
            //        }
            //        else if (buildableObject is BuildableFreeObject buildableFreeObject)
            //        {
            //            var so = buildableFreeObject.GetBuildableObjectSO() as BuildableFreeObjectSO;
            //            var prefabs = RandomUtility.UpdateBuildableObjectSORandomPrefab(so);
            //            var fourDirectionalRotation = DirectionUtility.GetDirectionForWorld(transform.rotation);
            //            EasyGridBuilderProController.Instance.TryInitializeBuildableFreeObjectSinglePlacement(transform.position, so,
            //               fourDirectionalRotation, EightDirectionalRotation.North, 0f,Vector3.zero, true, 0, true, out _, prefabs, null);
            //        }

            //    }
            //}
            //else
            {
                reslut = InstantiatePrefab(template, position, rotation, scale, parent);
            }

            return reslut;
        }
        public GameObject InstantiatePrefab(GameObject template, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            var gameObj = Object.Instantiate(template, position, rotation);
            gameObj.transform.SetParent(parent);
            gameObj.transform.localScale = scale;
            return gameObj;
        }


    }
}
