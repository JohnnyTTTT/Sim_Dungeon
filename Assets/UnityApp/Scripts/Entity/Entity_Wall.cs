using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using Sirenix.OdinInspector;
using SoulGames.EasyGridBuilderPro;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public enum WallHideMode
    {
        Hide,
        Full,
        Shorten,
        SlopeShorten
    }


    public enum WallRenderMode
    {
        Origin,
        DoorCuted,
    }
    [System.Serializable]
    public class WallModelGroup
    {

        public GameObject full;
        public GameObject shorten;
        public GameObject slopeShorten;

        public void Destroy()
        {
            if (full != null)
                GameObject.Destroy(full);
            if (shorten != null)
                GameObject.Destroy(shorten);
            if (slopeShorten != null)
                GameObject.Destroy(slopeShorten);
        }

        public void SetWallHideMode(WallHideMode wallHideMode)
        {
            if (full != null)
                full.SetActive(wallHideMode != WallHideMode.Hide && wallHideMode == WallHideMode.Full);
            if (shorten != null)
                shorten.SetActive(wallHideMode != WallHideMode.Hide && wallHideMode == WallHideMode.Shorten);
            if (slopeShorten != null)
                slopeShorten.SetActive(wallHideMode != WallHideMode.Hide && wallHideMode == WallHideMode.SlopeShorten);
        }

    }

    public class Entity_Wall : Entity_Edge
    {
        //[ShowInInspector]
        //public EdgeObjectCellDirection  edgeObjectCell
        //{
        //    get
        //    {
        //        return DirectionUtility.ToEdgeFourDirectionalRotation(Direction);
        //    }
        //}
        public WallModelGroup[] originWalls = new WallModelGroup[2];

        public bool isHidden;
        private Transform m_Camera;
        public Vector3 wallNormal = Vector3.forward;
        public WallRenderMode wallRenderMode;
        public WallHideMode WallHideMode;
        public Entity_SubEdge primary;
        public Entity_SubEdge secondary;
        public bool isFront;

        protected override void Start()
        {
            base.Start();
            wallNormal = transform.forward;
        }

        private void Update()
        {
            //if (!DungeonController.Instance.worldDataInited)
            //{
            //    return;
            //}
      
            //if (edgeElement.adjacentLargeCells[0].Data.CellType != FlowTilemapCellType.Floor || edgeElement.adjacentLargeCells[1].Data.CellType != FlowTilemapCellType.Floor)
            //{
            //    return;
            //}
            //var hide = false;
            //if (m_Camera == null)
            //{
            //    m_Camera = CameraController.Instance.MainCamera.transform;
            //}
            //var camDir = (transform.position - m_Camera.position).normalized;
            //var dot = Vector3.Dot(camDir, wallNormal.normalized);
            //if (dot < 0)
            //{
            //    hide = true;
            //}
            //else
            //{
            //    hide = false;
            //}
            ////Debug.Log(hidden,this);
            //primary.gameObject.SetActive(hide);
            //secondary.gameObject.SetActive(hide);
        }

        public override void UpdateData()
        {
            base.UpdateData();
            edgeElement.SetWallEntity(this);
        }

        public void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 1f, 0f), edgeElement.worldPosition, Color.blue);
                //GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 2f, 0f), edgeElement.adjacentLargeCells[0].worldPosition, Color.yellow);
                //GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 2f, 0f), edgeElement.adjacentLargeCells[1].worldPosition, Color.green);

                //if(edgeElement.containedSmallCells[0] != null)
                //GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 3f, 0f), edgeElement.containedSmallCells[0].worldPosition, Color.red);

                //if (edgeElement.containedSmallCells[1] != null)
                //    GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 3f, 0f), edgeElement.containedSmallCells[1].worldPosition, Color.red);

                //if (edgeElement.containedSmallCells[2] != null)
                //    GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 3f, 0f), edgeElement.containedSmallCells[2].worldPosition, Color.red);
            }
        }
    }
}
