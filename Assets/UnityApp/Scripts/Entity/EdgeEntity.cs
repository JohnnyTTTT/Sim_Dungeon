using DungeonArchitect.Flow.Domains.Tilemap;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Johnny.SimDungeon
{
    [System.Serializable]
    public class EdgeEntity : BuildingEntity<FlowTilemapEdge>
    {
        public SubEdgeEntity[] subEdges;

        public override void Init(FlowTilemapEdge data)
        {
            base.Init(data);
        }

        public void SetReplaceableObjectSO(SubEdgeEntity edge, ReplaceableObjectSO replaceable)
        {
            edge.replaceableObjectSO = replaceable;
            if (Application.isPlaying)
            {
                Destroy(edge.model);
            }
            else
            {
                DestroyImmediate(edge.model);
            }
            GameObject newModel;
            if (replaceable.randomModel)
            {
                var index = UnityEngine.Random.Range(0, replaceable.Models.Length);
                newModel = replaceable.Models[index];
            }
            else
            {
                newModel = replaceable.Models[0];
            }
            if (Application.isPlaying)
            {
                edge.model = Instantiate(newModel, edge.transform);
            }
            else
            {
#if UNITY_EDITOR
                edge.model = PrefabUtility.InstantiatePrefab(newModel) as GameObject;
                edge.model.transform.parent = edge.transform;
#endif

            }

            edge.model.transform.localPosition = Vector3.zero;
            edge.model.transform.localRotation = Quaternion.identity;

        }
    }
}
