using SoulGames.EasyGridBuilderPro;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Building_Wall : BuildingPart
    {
        private void Start()
        {
            var easyGridBuilderPro = GridManager.Instance.GetActiveEasyGridBuilderPro();
            Debug.Log(easyGridBuilderPro);
            var index = easyGridBuilderPro.GetActiveVerticalGridIndex();
            if (easyGridBuilderPro.TryInitializeBuildableEdgeObjectSinglePlacement(transform.position, BuildableAssets.Instance.wallStone,
                FourDirectionalRotation.South, false, true, true, index, true, out BuildableEdgeObject buildableGridObject, null, null))
            {
                //buildableGridObject.transform.parent = transform;
            }

        }
    }
}
