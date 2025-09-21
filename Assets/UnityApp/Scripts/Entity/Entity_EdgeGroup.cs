using DungeonArchitect;
using DungeonArchitect.Flow.Domains.Tilemap;
using SoulGames.EasyGridBuilderPro;
using System.Collections.Generic;
using UnityEngine;

namespace Johnny.SimDungeon
{
    [System.Serializable]
    public class RoomA
    {
        public Room room;
        public FourDirectionalRotation directional;
    }
    public class Entity_EdgeGroup : Entity
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

        public List<Vector3> floorPos = new List<Vector3>();

        protected override void Start()
        {
            base.Start();
            if (m_MainCam == null)
            {
                m_MainCam = CameraController.Instance.MainCamera.transform;
            }
            if (Application.isPlaying)
            {
                primaryMaterial = primary.GetComponentInChildren<Renderer>().material;
                secondaryMaterial = secondary.GetComponentInChildren<Renderer>().material;
            }
        }

        private void Update()
        {
            //bool hide = false;

            //foreach (var floorPo in floorPos)
            //{
            //    Vector3 camDir = m_MainCam.position - floorPo;
            //    camDir.y = 0; // 只考虑 XZ 平面

            //    Vector3 wallToCam = transform.position - floorPo;
            //    wallToCam.y = 0;

            //    float dot = Vector3.Dot(camDir.normalized, wallToCam.normalized);

            //    if (dot > 0f)
            //    {
            //        hide = true;
            //        break; // 一旦摄像机在正面，整面墙隐藏
            //    }
            //}

            //if (primary.currentObject)
            //{
            //    primary.currentObject.gameObject.SetActive(!hide);
            //}
            //if (secondary.currentObject)
            //{
            //    secondary.currentObject.gameObject.SetActive(!hide);
            //}

            //gameObject.SetActive(!hide);
        }

        public override void UpdateData()
        {
            Direction = DirectionUtility.GetDirectionForWorld(transform.rotation);

            var pElement = ElementManager_Cell.Instance.GetElement(primary.transform.position);
            var sElement = ElementManager_Cell.Instance.GetElement(secondary.transform.position);
            floorPos.Clear();

            if (pElement.room != null && pElement.room.roomType != RoomType.OriginaCave 
                && sElement.room != null && sElement.room.roomType != RoomType.OriginaCave)
            {
                if (pElement.room != null)
                {
                    //var a = new RoomA();
                    //a.room = pElement.room;
                    //a.directional = DirectionUtility.GetDirection(transform.position, a.room.center);
                    floorPos.Add(DungeonController.Instance.TileCoordToWorldPosition(pElement.Data.TileCoord));
                }
                if (sElement.room != null)
                {
                    //var b = new RoomA();
                    //b.room = sElement.room;
                    //b.directional = DirectionUtility.GetDirection(transform.position, b.room.center);
                    floorPos.Add(DungeonController.Instance.TileCoordToWorldPosition(sElement.Data.TileCoord));
                }
            }




            Element_Edge edgeElement;
            Element_Cell parentElement;
            if (Direction == FourDirectionalRotation.North || Direction == FourDirectionalRotation.South)
            {
                parentElement = pElement.Data.TileCoord.y > sElement.Data.TileCoord.y ? pElement : sElement;
                edgeElement = parentElement.horizontalEdge;

            }
            else
            {
                parentElement = pElement.Data.TileCoord.x > sElement.Data.TileCoord.x ? pElement : sElement;
                edgeElement = parentElement.verticalEdge;
            }

            m_WallPos = (primary.transform.position + secondary.transform.position) / 2f;
            SetParentCellElement_JustUseThisFunction(parentElement);
            edgeElement.wall = this;
        }

        public override void CreateOrUpdateModel()
        {
            primary.CreateOrUpdateModel();
            secondary.CreateOrUpdateModel();
        }

        protected override void SetParentCellElement_JustUseThisFunction(Element_Cell element)
        {
            base.SetParentCellElement_JustUseThisFunction(element);
            name = $"WallGroup - {element.Data.TileCoord.x},{element.Data.TileCoord.y} - {Direction}";
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

        public override bool TryDestroy()
        {
            var p = primary.TryDestroy();
            var s = secondary.TryDestroy();
            if (p && s)
            {
                Destroy(gameObject);
                return true;
            }
            Debug.LogError($"TryDestroy fail - <{gameObject.name}>", gameObject);
            return false;
        }

        public void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                var parentPosition = DungeonController.Instance.TileCoordToWorldPosition(ParentElement.Data.TileCoord);
                GizmoUnitily.DrawLine(transform.position, parentPosition, Color.beige);
            }
        }
    }
}
