using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_EdgeGroup : Entity<Data_Edge>
    {
        public static int CullPlaneHeight = Shader.PropertyToID("_CullPlaneHeight");
        public GameObject modelRoot;

        public Entity_SubEdge primary;
        public Entity_SubEdge secondary;


        public List<Entity_Corner> corners = new List<Entity_Corner>();
        public bool isHide;

        public BuildableEdgeObject primaryEdgeObject;
        public BuildableEdgeObject secondaryEdgeObject;

        private Material primaryMaterial;
        private Material secondaryMaterial;
        private Transform m_MainCam;
        private Vector3 m_WallPos;

        private void Start()
        {
            if (m_MainCam == null)
            {
                //m_MainCam = DungeonController.Instance.m_Camera.transform;
            }
            if (Application.isPlaying)
            {
                primaryMaterial = primary.GetComponentInChildren<Renderer>().material;
                secondaryMaterial = secondary.GetComponentInChildren<Renderer>().material;
            }

        }

        private void Update()
        {
            //var primaryRoom = primary.ParentData.parentRoom;
            //var secondaryRoom = secondary.ParentData.parentRoom;

            //if (primaryRoom != null || secondaryRoom != null)
            //{
            //    var hideA = ShouldHideWith(primaryRoom);
            //    var hideB = ShouldHideWith(secondaryRoom);

            //    bool shouldHide;
            //    if (primaryRoom != null && secondaryRoom != null)
            //    {
            //        shouldHide = hideA && hideB;
            //    }
            //    else
            //    {
            //        shouldHide = hideA || hideB;
            //    }

            //    var value = shouldHide ? 1.01f : 10f;
            //    if (parentData.corners.Count > 0)
            //    {
            //        foreach (var item in parentData.corners)
            //        {
            //            item.SetWallHide(value);
            //        }
            //    }
            //    primaryMaterial.SetFloat(CullPlaneHeight, value);
            //    secondaryMaterial.SetFloat(CullPlaneHeight, value);
            //}
        }

        public override void UpdateData()
        {
            primary.UpdateData();
            secondary.UpdateData();

            primary.parent = this;
            secondary.parent = this;

            primary.relativeEdge = secondary;
            secondary.relativeEdge = primary;
            
            primary.direction = DirectionUtility.GetDirection(primary.transform.position, transform.position);
            secondary.direction = DirectionUtility.GetDirection(secondary.transform.position, transform.position);

            var pCoord = primary.ParentData.Data.TileCoord;
            var sCoord = secondary.ParentData.Data.TileCoord;

            var orientation = DirectionUtility.GetOrientation(transform);
            Data_Edge edgeData = null;

            switch (orientation)
            {
                case Orientation.Horizontal:
                    var parentCoordHorizontal = pCoord.y > sCoord.y ? pCoord : sCoord;
                    edgeData = DataManager_Edge.Instance.GetHorizontal(parentCoordHorizontal);
                    break;
                case Orientation.Vertical:
                    var parentCoordVertical = pCoord.x > sCoord.x ? pCoord : sCoord;
                    edgeData = DataManager_Edge.Instance.GetVertical(parentCoordVertical);
                    break;
            }

            m_WallPos = (primary.transform.position + secondary.transform.position) / 2f;
            SetParentCellData_JustUseThisFunction(edgeData);
        }

        public override void SetTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            base.SetTransform(position, rotation, scale);
            primary.SetTransform(primary.transform.position, primary.transform.rotation, primary.transform.localScale);
            secondary.SetTransform(secondary.transform.position, secondary.transform.rotation, secondary.transform.localScale);
        }

        public override void ApplyBiomeRule()
        {
            primary.ApplyBiomeRule();
            secondary.ApplyBiomeRule();
        }

        private bool ShouldHideWith(Room room)
        {
            if (room != null)
            {
                var camToRoom = m_MainCam.position - room.center;
                camToRoom.y = 0f;
                camToRoom.Normalize();

                var roomToWall = m_WallPos - room.center;
                roomToWall.y = 0f;
                roomToWall.Normalize();

                var dot = Vector3.Dot(camToRoom, roomToWall);
                return dot > DungeonController.Instance.wallDotThreshold;
            }
            return false;
        }

        protected override void SetParentCellData_JustUseThisFunction(Data_Edge data)
        {
            base.SetParentCellData_JustUseThisFunction(data);
            data.entity = this;
        }

        public override bool TryDestroy()
        {
            var p= primary.TryDestroy();
            var s= secondary.TryDestroy();
            return p && s;
        }

        public void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                var parentPosition = DungeonController.Instance.TileCoordToWorldPosition(ParentData.Data.EdgeCoord);
                GizmoUnitily.DrawLine(transform.position, parentPosition, Color.beige);
            }
        }


    }
}
