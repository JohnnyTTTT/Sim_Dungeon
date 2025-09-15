using DungeonArchitect.Flow.Domains.Tilemap;
using UnityEngine;
using DungeonArchitect;

namespace Johnny.SimDungeon
{
    public class Data_Edge : ElementData<FlowTilemapEdge>
    {
        public Data_Edge(FlowTilemapEdge data) : base(data)
        {

        }

        //public void DrawGizmos()
        //{
        //    foreach (var item in m_ContainedCells)
        //    {
        //        GizmoUnitily.DrawFourSizeCube(item.Data.TileCoord, roomColor, false);
        //    }
        //    GizmoUnitily.DrawLabel(worldCenter, name);
        //}
        //protected override void Start()
        //{


        //    //SubEdgeEntity
        //    //foreach (var subEdgeEntity in edgeEntitly.subEdges)
        //    //{
        //    //    
        //    //    
        //    //    switch (dir)
        //    //    {
        //    //        case Direction.Left:
        //    //            realCellEntity.edges[0] = subEdgeEntity;
        //    //            break;
        //    //        case Direction.Up:
        //    //            realCellEntity.edges[1] = subEdgeEntity;
        //    //            break;
        //    //        case Direction.Right:
        //    //            realCellEntity.edges[2] = subEdgeEntity;
        //    //            break;
        //    //        case Direction.Down:
        //    //            realCellEntity.edges[3] = subEdgeEntity;
        //    //            break;
        //    //    }
        //    //}
        //}
        //public override void Init(FlowTilemapEdge data)
        //{
        //    base.Init(data);
        //    EntityManager_Edge.Instance.Register(this);
        //    registered = true;
        //}
        //protected override void OnDestroy()
        //{
        //    if (registered)
        //    {
        //        EntityManager_Edge.Instance.UnRegister(this);
        //    }
        //}

        //        public void SetReplaceableObjectSO(SubEdgeEntity edge, ReplaceableObjectSO replaceable)
        //        {
        //            edge.replaceableObjectSO = replaceable;
        //            if (Application.isPlaying)
        //            {
        //                Destroy(edge.model);
        //            }
        //            else
        //            {
        //                DestroyImmediate(edge.model);
        //            }
        //            GameObject newModel;
        //            if (replaceable.randomModel)
        //            {
        //                var index = UnityEngine.Random.Range(0, replaceable.Models.Length);
        //                newModel = replaceable.Models[index];
        //            }
        //            else
        //            {
        //                newModel = replaceable.Models[0];
        //            }
        //            if (Application.isPlaying)
        //            {
        //                edge.model = Instantiate(newModel, edge.transform);
        //            }
        //            else
        //            {
        //#if UNITY_EDITOR
        //                edge.model = PrefabUtility.InstantiatePrefab(newModel) as GameObject;
        //                edge.model.transform.parent = edge.transform;
        //#endif

        //            }

        //            edge.model.transform.localPosition = Vector3.zero;
        //            edge.model.transform.localRotation = Quaternion.identity;

        //        }

    }
}
