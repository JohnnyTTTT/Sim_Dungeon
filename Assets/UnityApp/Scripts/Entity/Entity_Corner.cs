using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_Corner : Entity
    {
        private List<Data_Edge> edges = new List<Data_Edge>();
        private Material cornerMaterial;
        private BuildableFreeObject currentObject;

        private void Start()
        {
            if (Application.isPlaying)
            {
                cornerMaterial = GetComponentInChildren<Renderer>().material;
            }
        }

        public override void UpdateData()
        {
            var coord = DungeonController.Instance.WorldPositionToTileCoord(transform.position);
            var coordLeft = new IntVector2(coord.x - 1, coord.y);
            var coordDown = new IntVector2(coord.x, coord.y - 1);

            var EdgeData = DataManager_Edge.Instance;

            var a = EdgeData.GetVertical(coord);
            if (a.Data.EdgeType != FlowTilemapEdgeType.Empty)
            {
                edges.Add(a);
                a.corners.Add(this);
            }
            var b = EdgeData.GetHorizontal(coord);
            if (b.Data.EdgeType != FlowTilemapEdgeType.Empty)
            {
                edges.Add(b);
                b.corners.Add(this);
            }
            var c = EdgeData.GetVertical(coordDown);
            if (c.Data.EdgeType != FlowTilemapEdgeType.Empty)
            {
                edges.Add(c);
                c.corners.Add(this);
            }
            var d = EdgeData.GetHorizontal(coordLeft);
            if (d.Data.EdgeType != FlowTilemapEdgeType.Empty)
            {
                edges.Add(d);
                d.corners.Add(this);
            }
            if (edges.Count < 2)
            {
                Debug.Log("未找到拐角对应的两条边，请检查 Corner 坐标或边数据是否对齐", gameObject);
            }
        }

        public override bool TryReplace(BuildableObjectSO temelpte, BuildableObjectSO.RandomPrefabs prefabs = null)
        {
            if (temelpte is BuildableFreeObjectSO buildableFreeObject)
            {
                if (currentObject == null || currentObject.GetBuildableObjectSO() != buildableFreeObject)
                {
                    if (EasyGridBuilderProController.Instance.ReplaceCorner(this, buildableFreeObject, currentObject, out var buildable))
                    {
                        currentObject = buildable;
                        DestroyTelempte();
                        return true;
                    }
                }
            }
            return false;
        }

        public void SetWallHide(float value)
        {
            cornerMaterial.SetFloat(Entity_EdgeGroup.CullPlaneHeight, value);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                GizmoUnitily.DrawLine(transform.position + new Vector3(0f, 4f, 0f), edges[0].entity.transform.position, Color.red);
                GizmoUnitily.DrawLine(transform.position + new Vector3(0f, 4f, 0f), edges[1].entity.transform.position, Color.yellow);
            }
        }


    }
}
