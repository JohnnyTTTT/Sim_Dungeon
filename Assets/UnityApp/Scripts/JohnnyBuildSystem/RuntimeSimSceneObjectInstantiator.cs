using DungeonArchitect;
using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class RuntimeSimSceneObjectInstantiator : IDungeonSceneObjectInstantiator
    {
        public GameObject Instantiate(GameObject template, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            Debug.Log(template.name);
            GameObject reslut = null;
            if (template.TryGetComponent<BuildableObject>(out var buildableObject))
            {
                Debug.Log(buildableObject);
                if (buildableObject is BuildableGridObject buildableGridObject)
                {
                    EasyGridBuilderProController.Instance.ChangeCurrentGrid(GridType.SizeTwo);
                    var verticalGridIndex = EasyGridBuilderProController.Instance.m_CurrentGridBuilderPro.GetActiveVerticalGridIndex();
                    var buildableObjectSO = buildableGridObject.GetBuildableObjectSO() as BuildableGridObjectSO;
                    Debug.Log(buildableObjectSO);
                    if (EasyGridBuilderProController.Instance.TryInitializeBuildableGridObjectSinglePlacement(position, buildableObjectSO,
                        FourDirectionalRotation.North, true, true, verticalGridIndex, true, out var buildReslut, null, null))
                    {
                        reslut = buildReslut.gameObject;
                    }
                }

            }
            return reslut;
        }
    }
}
