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
        [ShowInInspector]
        public FourDirectionalRotation fourDirectionalRotation
        {
            get
            {
                return DirectionUtility.ToEdgeFourDirectionalRotation(Direction);
            }
        }
        public WallModelGroup[] originWalls = new WallModelGroup[2];

        public bool isHidden;
        private Transform m_Camera;
        public Vector3 wallNormal = Vector3.forward;
        public WallRenderMode wallRenderMode;
        public WallHideMode WallHideMode;
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
            //if (edgeElement.adjacentCells[0].Data.CellType != FlowTilemapCellType.Floor || edgeElement.adjacentCells[1].Data.CellType != FlowTilemapCellType.Floor)
            //{
            //    return;
            //}

            //if (m_Camera == null)
            //{
            //    m_Camera = CameraController.Instance.MainCamera.transform;
            //}
            //var camDir = (transform.position - m_Camera.position).normalized;
            //var dot = Vector3.Dot(camDir, wallNormal.normalized);
            //if (dot < 0)
            //{
            //    wallCutType = WallCutType.None;
            //}
            //else
            //{
            //    wallCutType = WallCutType.Half;
            //}
            ////Debug.Log(hidden,this);
            //primary.SetWallCutType(wallCutType);
            //secondary.SetWallCutType(wallCutType);
        }

        public override void UpdateData()
        {
            base.UpdateData();
            edgeElement.wall = this;
        }

        public void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 1f, 0f), edgeElement.worldPosition, Color.blue);
                GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 2f, 0f), edgeElement.adjacentCells[0].worldPosition, Color.yellow);
                GizmoUnitily.DrawLine(transform.position + transform.right + new Vector3(0f, 2f, 0f), edgeElement.adjacentCells[1].worldPosition, Color.green);
            }
        }
    }
}
