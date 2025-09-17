using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_EdgeGroup : Entity
    {
        public static int CullPlaneHeight = Shader.PropertyToID("_CullPlaneHeight");
        public GameObject modelRoot;

        public Entity_Edge primary;
        public Entity_Edge secondary;

        public Data_Cell primaryCellData;
        public Data_Cell secondaryCellData;
        public Data_Edge parentData;

        private Room primaryRoom;
        private Room secondaryRoom;

        public List<Entity_Corner> corners = new List<Entity_Corner>();
        public bool isHide;

        public BuildableEdgeObject primaryEdgeObject;
        public BuildableEdgeObject secondaryEdgeObject;

        private Material primaryMaterial;
        private Material secondaryMaterial;
        private Transform m_MainCam;
        private Vector3 m_WallPos;
        public bool isRim;

        private void Start()
        {
            if (m_MainCam == null)
            {
                m_MainCam = DungeonController.Instance.m_Camera.transform;
            }
            if (Application.isPlaying)
            {
                primaryMaterial = primary.GetComponentInChildren<Renderer>().material;
                secondaryMaterial = secondary.GetComponentInChildren<Renderer>().material;
            }
        }

        private void Update()
        {
            if (!isRim && (primaryRoom != null || secondaryRoom != null))
            {
                var hideA = ShouldHideWith(primaryRoom);
                var hideB = ShouldHideWith(secondaryRoom);

                bool shouldHide;
                if (primaryRoom != null && secondaryRoom != null)
                {
                    shouldHide = hideA && hideB;
                }
                else
                {
                    shouldHide = hideA || hideB;
                }

                var value = shouldHide ? 1.01f : 10f;
                if (parentData.corners.Count > 0)
                {
                    foreach (var item in parentData.corners)
                    {
                        item.SetWallHide(value);
                    }
                }
                primaryMaterial.SetFloat(CullPlaneHeight, value);
                secondaryMaterial.SetFloat(CullPlaneHeight, value);
            }
        }

        public override void UpdateData()
        {
            var pPosition = primary.transform.position;
            primary.relativeEdge = secondary;
            primary.parent = this;

            var sPosition = secondary.transform.position;
            secondary.relativeEdge = primary;
            secondary.parent = this;

            primary.direction = DirectionUtility.GetDirection(pPosition, transform.position);
            secondary.direction = DirectionUtility.GetDirection(sPosition, transform.position);

            //获取相邻两个Cell
            var pCell = DataManager_Cell.Instance.GetData(pPosition);
            var sCell = DataManager_Cell.Instance.GetData(sPosition);

            primary.SetParentCellData(pCell);
            secondary.SetParentCellData(sCell);

            var orientation = DirectionUtility.GetOrientation(transform);


            var pCoord = pCell.Data.TileCoord;
            var sCoord = sCell.Data.TileCoord;

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

            SetCellsAndEdge(pCell, sCell, edgeData);
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

        public void SetCellsAndEdge(Data_Cell p, Data_Cell s, Data_Edge parent)
        {
            parentData = parent;
            parentData.entity = this;

            primaryCellData = p;
            primaryRoom = primaryCellData.parentRoom;

            secondaryCellData = s;
            secondaryRoom = secondaryCellData.parentRoom;

            m_WallPos = (primaryCellData.worldPosition + secondaryCellData.worldPosition) / 2f;

            if (primaryCellData.Data.CellType == FlowTilemapCellType.Custom || secondaryCellData.Data.CellType == FlowTilemapCellType.Custom)
            {
                isRim = true;
            }
        }


        public void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                primaryCellData.DrawGizmos();
                secondaryCellData.DrawGizmos();

                var parentPosition = DungeonController.Instance.TileCoordToWorldPosition(parentData.Data.EdgeCoord);
                GizmoUnitily.DrawLine(transform.position, parentPosition, Color.beige);
                if (primaryRoom != null)
                {
                    GizmoUnitily.DrawLabel(transform.position, $"{primaryRoom.name},IsRim: {isRim.ToString()}");
                }
                if (secondaryRoom != null)
                {
                    GizmoUnitily.DrawLabel(transform.position + new Vector3(0f, 2f, .0f), secondaryRoom.name);
                }
            }
        }


    }
}
