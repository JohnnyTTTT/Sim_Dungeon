using UnityEngine;

namespace Johnny.SimDungeon
{
    public class Entity_EdgeGroup : MonoBehaviour
    {
        public GameObject modelRoot;

        public Transform primary;
        public Transform secondary;

        public Data_Cell primaryCell;
        public Data_Cell secondaryCell;

        public Room primaryRoom;
        public Room secondaryRoom;

        public bool drawGizmos;

        [Header("Dot 阈值")]
        [Range(0f, 1f)]
        public float dotThreshold = 0.5f;
        private Transform mainCam;

        private void Start()
        {
            if (Camera.main != null)
                mainCam = Camera.main.transform;
            else
                Debug.LogError("场景中没有主相机！");
        }

        private void Update()
        {
            if (mainCam == null || primaryCell == null || secondaryCell == null) return;

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            Vector3 wallPos;
            if (primaryCell != null && secondaryCell != null)
                wallPos = (primaryCell.worldPosition + secondaryCell.worldPosition) / 2f;
            else if (primaryCell != null)
                wallPos = primaryCell.worldPosition;
            else if (secondaryCell != null)
                wallPos = secondaryCell.worldPosition;
            else
                wallPos = transform.position;

            bool hideA = false;
            bool hideB = false;

            if (primaryRoom != null)
            {
                Vector3 camToRoomA = mainCam.position - primaryRoom.center;
                camToRoomA.y = 0f;
                camToRoomA.Normalize();

                Vector3 roomAToWall = wallPos - primaryRoom.center;
                roomAToWall.y = 0f;
                roomAToWall.Normalize();

                float dotA = Vector3.Dot(camToRoomA, roomAToWall);
                hideA = dotA > dotThreshold;
            }

            if (secondaryRoom != null)
            {
                Vector3 camToRoomB = mainCam.position - secondaryRoom.center;
                camToRoomB.y = 0f;
                camToRoomB.Normalize();

                Vector3 roomBToWall = wallPos - secondaryRoom.center;
                roomBToWall.y = 0f;
                roomBToWall.Normalize();

                float dotB = Vector3.Dot(camToRoomB, roomBToWall);
                hideB = dotB > dotThreshold;
            }


            bool shouldHide = false;
            if (primaryRoom != null && secondaryRoom != null)
            {
                shouldHide = hideA && hideB;
            }
            else
            {
                shouldHide = hideA || hideB;
            }

            modelRoot.SetActive(!shouldHide);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
            {
                primaryCell.DrawGizmos();
                secondaryCell.DrawGizmos();
            }
        }
    }
}
